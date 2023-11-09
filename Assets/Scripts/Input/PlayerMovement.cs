//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UIElements;
//using Zenject;

//[Serializable]
//public enum SIDE { Left, Middle, Right }

//public class PlayerMovement : MonoBehaviour
//{
//    [SerializeField] private Animator _animator;
//    [SerializeField] AnimationController _animController;
//    [SerializeField] Rigidbody _rb;
//    [SerializeField] GameObject _fullBodyCollider;
//    [SerializeField] GameObject _bendCollider;
//    [Inject] private PlayerController _playerController;

//    public SIDE m_Side = SIDE.Middle;
//    private float _newXPos = 0f;
//    public bool SwipeLeft;
//    public bool SwipeRight;
//    public bool SwipeUp;
//    public bool SwipeDown;
//    public float XValue;
//    private float _xTransition;
//    public float SpeedDodge;
//    public float jumpPower = 7;
//    private float _yTransition;
//    public bool InJump;
//    public bool InRoll;
//    private float ColHeight;
//    private float ColCenterY;
//    public bool isBending;
//    private bool isSwitching;
//    private float bendDuration = 1f;
//    private bool requestBend;

//    private float xTransition;

//    // Start is called before the first frame update
//    void Start()
//    {
//        _playerController.OnBend += RequestBend;
//        _animController.OnDodgeFinished += SwitchDodgeState;
//        transform.position = Vector3.zero;
//        //_bendCollider.SetActive(false);
//    }

//    private void SwitchDodgeState()
//    {
//        _animator.SetBool("IsDodgeRight", false);
//        _animator.SetBool("IsDodgeLeft", false);
//    }

//    private void RequestBend(bool isBend)
//    {
//        requestBend = isBend;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        MovePlayerBetweenLines();
//    }

//    private void MoveSmooth(Vector3 side)
//    {
//        transform.position = Vector3.Lerp(transform.position, side, Time.deltaTime * 4f);
//    }


//    private void MovePlayerBetweenLines()
//    {
//        SwipeLeft = SwipeController.swipeLeft;
//        SwipeRight = SwipeController.swipeRight;
//        SwipeUp = SwipeController.swipeUp;
//        SwipeDown = SwipeController.swipeDown;

//        if (SwipeLeft)
//        {
//            if (m_Side == SIDE.Middle)
//            {
//                _newXPos = -XValue;
//                m_Side = SIDE.Left;
//                _animator.Play("Dodge_left");
//            }
//            else if (m_Side == SIDE.Right)
//            {
//                _newXPos = 0;
//                m_Side = SIDE.Middle;
//                _animator.Play("Dodge_left");
//            }
//        }

//        if (SwipeRight)
//        {
//            if (m_Side == SIDE.Middle)
//            {
//                _newXPos = XValue;
//                m_Side = SIDE.Right;
//                _animator.Play("Dodge_right");

//            }
//            else if (m_Side == SIDE.Left)
//            {
//                _newXPos = 0;
//                m_Side = SIDE.Middle;
//                _animator.Play("Dodge_right");
//            }

//        }

//        Vector3 newPos = new Vector3(_newXPos, transform.position.y, 0);
//        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 2 * SpeedDodge); //TODO a
//        Roll();
//    }

//    internal float RollCounter;

//    public void Roll()
//    {
//        RollCounter -= Time.deltaTime;
//        if (RollCounter <= 0f)
//        {
//            RollCounter = 0f;
//            InRoll = false;
//        }

//        if (SwipeDown)
//        {
//           BendButtonClicked();
//        }
//    }

//    private void SwitchCollider()
//    {
//        if (_fullBodyCollider.activeInHierarchy)
//        {
//            _fullBodyCollider.SetActive(false);
//            _bendCollider.SetActive(true);
//        }
//        else
//        {
//            _fullBodyCollider.SetActive(true);
//            _bendCollider.SetActive(false);
//        }
//    }

//    public IEnumerator SwitchColliderWithDelay()
//    {
//        isBending = true;
//        isSwitching = true;

//        SwitchCollider();

//        RollCounter = 0.2f;
//        //_yTransition -= jumpPower;
//        //_animator.CrossFadeInFixedTime("Roll", 0.1f);
//        InRoll = true;
//        InJump = false;

//        yield return new WaitForSeconds(bendDuration); // Затримка в 1 секунди

//        SwitchCollider();

//        isSwitching = false;
//        isBending = false;
//    }

//    public void BendButtonClicked()
//    {
//        if (requestBend && !isBending && !isSwitching && _playerController.IsGrounded)
//        {
//            StartCoroutine(SwitchColliderWithDelay());
//        }
//    }
//}