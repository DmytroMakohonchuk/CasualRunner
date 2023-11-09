using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SIDE { Left, Middle, Right }

public class CharacterMovement : MonoBehaviour
{
    //[SerializeField] private Animator _animator;
    [SerializeField] AnimationController _animController;
    [SerializeField] private CharacterController character;

    public SIDE m_Side = SIDE.Middle;
    private float _newXPos = 0f;
    public bool SwipeLeft;
    public bool SwipeRight;
    public bool SwipeUp;
    public bool SwipeDown;
    public float XValue;
    private float x;
    public float speedDodge;
    public float jumpPower = 7f;
    private float y;
    public bool inJump;
    public bool inRoll;
    private float ColHeight;
    private float ColCenterY;
    [SerializeField] private float colliderYCenterOffset;

    private float xTransition;

    // Start is called before the first frame update
    void Start()
    {
        //_animController.OnDodgeFinished += SwitchDodgeState;
        ColHeight = character.height;
        ColCenterY = character.center.y + colliderYCenterOffset;
        transform.position = Vector3.up;
        //_bendCollider.SetActive(false);
    }

    //private void SwitchDodgeState()
    //{
    //    _animator.SetBool("IsDodgeRight", false);
    //    _animator.SetBool("IsDodgeLeft", false);
    //}

    //private void RequestBend(bool isBend)
    //{
    //    requestBend = isBend;
    //}

    //// Update is called once per frame
    void Update()
    {
        SwipeLeft = SwipeController.swipeLeft;
        SwipeRight = SwipeController.swipeRight;
        SwipeUp = SwipeController.swipeUp;
        SwipeDown = SwipeController.swipeDown;
        MovePlayerBetweenLines();
    }


    //private void MoveSmooth(Vector3 side)
    //{
    //    transform.position = Vector3.Lerp(transform.position, side, Time.deltaTime * 4f);
    //}


    private void MovePlayerBetweenLines()
    {

        if (SwipeLeft)
        {
            //if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            //{
            //    _animator.SetBool("DodgeLeftInFall", true);
            //}
            //else
            //{
            //    _animator.SetBool("DodgeLeftInFall", false);
            //}

            if (m_Side == SIDE.Middle)
            {
                _newXPos = -XValue;
                m_Side = SIDE.Left;
                //if (character.isGrounded)
                    //_animator.Play("Dodge_left");
            }
            else if (m_Side == SIDE.Right)
            {
                _newXPos = 0;
                m_Side = SIDE.Middle;
                //if(character.isGrounded)
                    //_animator.Play("Dodge_left");
            }
        }

        if (SwipeRight)
        {
            //if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            //{
            //    _animator.SetBool("DodgeRightInFall", true);
            //}
            //else
            //{
            //    _animator.SetBool("DodgeRightInFall", false);
            //}

            if (m_Side == SIDE.Middle)
            {
                _newXPos = XValue;
                m_Side = SIDE.Right;
                //if (character.isGrounded)
                //    _animator.Play("Dodge_right");

            }
            else if (m_Side == SIDE.Left)
            {
                _newXPos = 0;
                m_Side = SIDE.Middle;
                //if (character.isGrounded)
                //    _animator.Play("Dodge_right");
            }

        }

        //x = x * (1 - Time.deltaTime * speedDodge) + _newXPos * (Time.deltaTime * speedDodge);
        //Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, 0);
        ////character.Move(moveVector);
        //transform.position = new Vector3(x, transform.position.y, transform.position.z);

        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, 0);
        x = Mathf.Lerp(x, _newXPos, Time.deltaTime * speedDodge);
        character.Move(moveVector); 

        Jump();
        Roll();
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);        
    }

    private void Jump()
    {
        if (character.isGrounded)
        {
            //if(_animator.GetCurrentAnimatorStateInfo(0).IsName("FallingIdle"))
            //{
            //    _animator.Play("Landing");
            //    inJump = false;
            //}

            if(SwipeUp)
            {
                y = jumpPower;
                //_animator.CrossFadeInFixedTime("Jump", 0.1f);
                inJump = true;
            }
            
        }
        else
        {
            y -= jumpPower * 2 * Time.deltaTime;
            //if(character.velocity.y < -0.1f)
            //_animator.Play("FallingIdle");
        }
    }

    private float RollCounter;

    private void Roll()
    {
        RollCounter -= Time.deltaTime;
        if(RollCounter <= 0f)
        {
            RollCounter = 0f;
            character.center = new Vector3(0, ColCenterY, 0);
            character.height = ColHeight;
            inRoll = false;
        }

        if(SwipeDown)
        {
            y -= 10f;
            character.center = new Vector3(0, ColCenterY/2f, 0);
            character.height = ColHeight/2f;
            //_animator.CrossFadeInFixedTime("Bend", 0.1f);
            inRoll = true;
        }
    }

    //internal float RollCounter;

    //public void Roll()
    //{
    //    RollCounter -= Time.deltaTime;
    //    if (RollCounter <= 0f)
    //    {
    //        RollCounter = 0f;
    //        InRoll = false;
    //    }

    //    if (SwipeDown)
    //    {
    //        BendButtonClicked();
    //    }
    //}

    //private void SwitchCollider()
    //{
    //    if (_fullBodyCollider.activeInHierarchy)
    //    {
    //        _fullBodyCollider.SetActive(false);
    //        _bendCollider.SetActive(true);
    //    }
    //    else
    //    {
    //        _fullBodyCollider.SetActive(true);
    //        _bendCollider.SetActive(false);
    //    }
    //}

    //public IEnumerator SwitchColliderWithDelay()
    //{
    //    isBending = true;
    //    isSwitching = true;

    //    SwitchCollider();

    //    RollCounter = 0.2f;
    //    //_yTransition -= jumpPower;
    //    //_animator.CrossFadeInFixedTime("Roll", 0.1f);
    //    InRoll = true;
    //    InJump = false;

    //    yield return new WaitForSeconds(bendDuration); // Затримка в 1 секунди

    //    SwitchCollider();

    //    isSwitching = false;
    //    isBending = false;
    //}

    //public void BendButtonClicked()
    //{
    //    if (requestBend && !isBending && !isSwitching && _playerController.IsGrounded)
    //    {
    //        StartCoroutine(SwitchColliderWithDelay());
    //    }
    //}
}
