using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System;
using Zenject;
using System.Collections;

public class PlayerTouchMovement : MonoBehaviour, IPlayerTouchMovement
{
    [Inject] ITileSpawner _tileSpawner;

    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] Material _groundedMat;
    [SerializeField] Material _notGroundedMat;

    [SerializeField] private float minKnobDistance = 10f; // Мінімальна відстань, необхідна для активації руху гравця

    [SerializeField] LayerMask platformLayerMask;
    private LevelTile groundTile;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] LevelTile initialTile;
    private static bool isControlEnabled = true;
    public PlayerSettings playerSettings;
    public static bool IsControlEnabled
    {
        get => isControlEnabled;
        set => isControlEnabled = value;
    }

    [Header("References")]
    private GameOverEvent collisionDetector;
    [SerializeField] private Transform capsule;
    [SerializeField] private GameObject bendCollider;
    [SerializeField] private GameObject idleCollider;
    [SerializeField] private FloatingJoystick joystick;

    [Header("Settings")]
    private Vector3 joystickSize;
    private float speedMultiplier => playerSettings.speedMultiplier;
    private float smoothRotationSpeed;
    private float bendAmount;
    private float jumpPower;

    private Vector3 originalVelocity;
    private bool isJumpTweened;
    private bool wasJumpingBeforePause;
    private bool isBendTweened;
    private bool isDashTweened;
    private float jumpEndPosition;
    private Vector3 dashImpulse;
    private Finger movementFinger;
    private Vector3 movementAmount;
    private Vector3 originalRotation;
    private Vector3 bendRotation;
    private bool isPaused;
    private bool isGameOver;
    private Collider _currentGroundCollider;
    public event Action<LevelTile> OnGroundChanged;
    private float minXpos = -3;
    private float maxXpos = 3;
    private bool bendActivated;

    [SerializeField] private Animator animator;

    private bool timerOut { get => jumpTimer <= 0; }
    private bool isGroundedPosition { get => transform.position.y <= 0; }


    private float jumpTimer;
    private float upJumpTime = 0.5f;
    private float downJumpTime = 0.45f;

    private float upSpeed = 9;
    private float downSpeed = -9;

    private Vector3 _speed;
    [SerializeField] private float _airAcceleration;
    [SerializeField] private float _maxFallSpeed;

    RaycastHit result = new RaycastHit();


    public bool IsGameOver
    {
        get => isGameOver;
        set => isGameOver = value;
    }

    [SerializeField] private bool isGrounded;

    public bool IsGrounded => isGrounded;

    private float groundCheckDistance = 0.1f; // Відстань для перевірки, яка визначає "землю"

    private bool comingDown = false;
    private bool isJumping = false;

    private bool isSwitching;

    private Vector3 playerTransform;

    [SerializeField] private Vector3 groundCheckOffset;

    private void Start()
    {
        OnGroundChanged += UpdateGroundTile;
        groundTile = initialTile;
        joystickSize = playerSettings.joystickSize;
        //speedMultiplier = playerSettings.speedMultiplier;
        smoothRotationSpeed = playerSettings.smoothRotationSpeed;
        bendAmount = playerSettings.bendAmount;
        jumpPower = playerSettings.jumpPower;
        collisionDetector = GetComponent<GameOverEvent>();

        // Ініціалізувати m_XPos значенням поточної позиції гравця
        m_XPos = transform.position.x;
    }

    private void UpdateGroundTile(LevelTile tile)
    {
        groundTile = tile;
    }

    public Vector3 GetPlayerCurrentPosition()
    {
        return playerTransform;
    }

    public void PauseAnimator()
    {
        animator.speed = 0;
    }

    public void ResumeAnimator()
    {
        animator.speed = 1;
    }

    private void CheckIfGrounded()
    {
        float extraHeightText = 0.1f;
        var startPoint = transform.position + groundCheckOffset;
        isGrounded = Physics.Raycast(startPoint, Vector3.down, out result, extraHeightText, platformLayerMask);

        GetMinMaxPos(extraHeightText, startPoint);

        if (result.collider != null)
        {
            Debug.Log(result.collider);
            Debug.Log(result.collider.bounds.size.x);
        }

        Debug.DrawRay(startPoint, Vector3.down * extraHeightText, Color.blue);
        Debug.Log(isGrounded);

        if (!isGrounded)
        {
            _meshRenderer.sharedMaterial = _notGroundedMat;
        }
        else
        {
            _meshRenderer.sharedMaterial = _groundedMat;
        }
    }

    private void GetMinMaxPos(float extraHeightText, Vector3 startPoint)
    {
        if (Physics.Raycast(startPoint, Vector3.down, out result, extraHeightText, platformLayerMask))
        {
            minXpos = result.collider.bounds.min.x;
            maxXpos = result.collider.bounds.max.x;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
#endif

    public void ResetPlayerPosition()
    {
        m_XPos = 0;
        transform.position = Vector3.zero;
        //player.transform.position = Vector3.zero;
    }

    public bool GetIsControlEnabled()
    {
        return isControlEnabled;
    }

    public void JumpButtonClicked()
    {
        if (isControlEnabled && IsGrounded)
        {
            if (isJumping == false)
            {
                animator.SetBool("IsJumping", true);
                isJumping = true;
                jumpTimer = upJumpTime; //reset jump timer time
                StartCoroutine(JumpSequence());
            }
        }
    }

    private float colliderSwitchDelay = 1f;

    IEnumerator SwitchColliderWithDelay()
    {
        bendActivated = true;
        isSwitching = true;
        
        SwitchCollider();

        yield return new WaitForSeconds(colliderSwitchDelay); // Затримка в 1 секунди

        SwitchCollider(); 

        animator.speed = 1f;

        animator.SetBool("IsBend", false);

        isSwitching = false;
        bendActivated = false;
    }

    public void BendButtonClicked()
    {
        if (isControlEnabled && !bendActivated && !isSwitching && isGrounded)
        {
            animator.SetBool("IsBend", true);
            StartCoroutine(SwitchColliderWithDelay());
        }
    }

    private void SwitchCollider()
    {
        if (idleCollider.activeInHierarchy)
        {
            idleCollider.SetActive(false);
            bendCollider.SetActive(true);
        }
        else
        {
            idleCollider.SetActive(true);
            bendCollider.SetActive(false);
        }
    }

    private void OnBecameCompleted()
    {
        SwitchCollider();
    }

    private bool isDashing = false;
    private float dashDuration = 0.5f; // тривалість дешу
    private Vector3 dashDirection = Vector3.down;
    private float dashDistance = 10f; // відстань дешу вниз
    private float dashSpeed = 5f; // швидкість дешу

    public void DashButtonClicked()
    {
        if (!isDashing && !isGrounded)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash() //TODO fix dash stops animation
    {
        isDashing = true; // Позначаємо, що гравець робить деш.
        float startTime = Time.time; // Час початку дешу.
        Vector3 startPos = transform.position; // Початкова позиція гравця перед дешем.
        Vector3 endPos = transform.position + dashDirection * dashDistance; // Кінцева позиція для дешу.

        // Поки не минув час дешу і гравець не знову знаходиться на землі:
        while (Time.time < startTime + dashDuration && !isGrounded)
        {
            float progress = (Time.time - startTime) / dashDuration; // Обчислюємо прогрес дешу (значення від 0 до 1).
            transform.position = Vector3.Lerp(startPos, endPos, progress); // Інтерполяція між початковою і кінцевою позиціями для плавного руху гравця.
            yield return null; // Почекати одну ітерацію циклу перед продовженням.
        }

        if(isGrounded)
        {
            BendButtonClicked(); //When grounded switching colliders
        }

        isDashing = false; // Позначаємо, що деш завершено.
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Subscribe();
    }

    private void Subscribe()
    {
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        GlobalEventManager.OnPause += DisableMovement;
    }

    private void OnDisable()
    {
        Unsubscribe();
        EnhancedTouchSupport.Disable();
    }

    private void Unsubscribe()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            Vector3 maxMovement = new Vector3(joystickSize.x / 2f, joystickSize.y / 2f, 0f);
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            Vector3 touchDelta = (Vector3)currentTouch.screenPosition - joystick.transform.position;
            float distance = touchDelta.magnitude;

            Vector3 knobPosition = Vector3.ClampMagnitude(touchDelta, maxMovement.magnitude);
            joystick.knob.anchoredPosition = knobPosition;

            if (distance > minKnobDistance)
            {
                movementAmount = knobPosition / maxMovement.magnitude;
            }
            else
            {
                movementAmount = Vector3.zero; // Рух гравця вимикається, коли кноб джойстика занадто близько до центру
            }
        }
    }


    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
            joystick.knob.anchoredPosition = Vector3.zero;
            joystick.gameObject.SetActive(false);
            movementAmount = Vector3.zero;
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            movementFinger = touchedFinger;
            movementAmount = Vector3.zero;
            joystick.gameObject.SetActive(true);
            joystick.rectTransform.sizeDelta = joystickSize;

            Vector3 touchPosition = touchedFinger.screenPosition;
            Vector3 joystickPosition = new Vector3(touchPosition.x, touchPosition.y, joystick.transform.position.z);
            joystick.transform.position = joystickPosition;
        }
    }

    private void FixedUpdate()
    {
        if (isPaused)
        {
            PauseAnimator();
            // Закоментуйте наступні дві лінії, якщо ви використовуєте безпосередні зміщення
            //player.useGravity = false;
            //player.velocity = Vector3.zero;
        }
        else if (!isPaused)
        {
            ResumeAnimator();
            // Закоментуйте наступну лінію, якщо ви використовуєте безпосередні зміщення
            //player.useGravity = true;
        }
    }


    float m_XPos;

    private void Move(bool isMoveAvailable)
    {
        IsControlEnabled = isMoveAvailable;

        if (IsControlEnabled)
        {
            Vector3 movement = new Vector3(movementAmount.x, 0f, 0f);
            float newXPos = m_XPos + movementAmount.x * 0.1f;

            newXPos = Mathf.Clamp(newXPos, minXpos + 0.5f, maxXpos - 0.5f);

            m_XPos = newXPos;
            transform.position = new Vector3(m_XPos, transform.position.y, transform.position.z);
        }
    }


    public void DisableMovement(bool isDisabled)
    {
        if (isDisabled)
        {
            Move(false);
            isPaused = true;
        }
        else
        {
            Move(true);
            isPaused = false;
        }
    }

    private void Update()
    {
        CheckIfGrounded();

        HandleJumpTimer();
        JumpHandler();

        playerTransform = transform.position;

        //HandleVertical();
        //GetColliderWidth();
        Move(IsControlEnabled);
        RotateScaleMovement();
    }


    private void JumpHandler()
    {
        if (isJumping && !isPaused)
        {
            if (comingDown == false)
            {
                transform.position += Vector3.up * Time.deltaTime * upSpeed;
            }

            if (comingDown == true)
            {
                transform.position += Vector3.up * Time.deltaTime * downSpeed;
            }
        }
    }

    private void HandleJumpTimer()
    {
        if (!isPaused)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    //IEnumerator JumpSequence()
    //{
    //    yield return new WaitForSeconds(0.45f);           //float timer, timer -= deltatime, every jump timer = 0.45 original value
    //    yield return new WaitUntil(() => !isPaused);
    //    comingDown = true;
    //    yield return new WaitForSeconds(0.20f);
    //    isJumping = false;
    //    comingDown = false;
    //}

    IEnumerator JumpSequence()
    {
        //float timer, timer -= deltatime, every jump timer = 0.45 original value
        yield return new WaitUntil(() => timerOut);
        comingDown = true;
        jumpTimer = downJumpTime;
        //yield return new WaitUntil(() => timerOut);
        yield return new WaitUntil(() => isGroundedPosition);
        ResetYPosition();
        animator.SetBool("IsJumping", false);
        isJumping = false;
        comingDown = false;
    }

    private void ResetYPosition()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void RotateScaleMovement()
    {
        float speed = movementAmount.magnitude;

        if (speed > 0.1f)
        {
            Vector3 targetDirection = new Vector3(movementAmount.x, 0f, 0f);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothRotationSpeed * Time.deltaTime);
            Vector3 scaledMovement = speed * Time.deltaTime * speedMultiplier * targetDirection;
            transform.position += scaledMovement;
        }
    }
}