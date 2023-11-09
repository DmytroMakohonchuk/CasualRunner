using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private SwipeController _swipeController;

    public event Action<bool> OnJump;

    public event Action<bool> OnBend;

    public event Action<bool> OnDash;

    public event Action<bool> OnGroundChanged;

    [SerializeField] private bool isGrounded;
    public bool IsGrounded => isGrounded;

    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private float distance = 2f;

    RaycastHit result = new RaycastHit();
    [SerializeField] LayerMask platformLayerMask;

    private float minXpos = -3;
    private float maxXpos = 3;


    [SerializeField] float extraHeightText = 0.1f;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckIfGrounded();
        GetInput();
    }

    private void GetInput()
    {
        if (SwipeController.swipeUp && isGrounded)
        {
            PlayerJump();
        }

        if(SwipeController.swipeDown && isGrounded)
        {
            PlayerBend();
        }

        if (SwipeController.swipeDown && !isGrounded)
        {

            _rb.velocity = Vector3.zero;
            PlayerDash();
        }
    }

    public void PlayerDash()
    {
        OnDash?.Invoke(true);
    }

    public void PlayerBend()
    {
        OnBend?.Invoke(true);
    }

    public void PlayerJump()
    {
        OnJump?.Invoke(true);
    }

    private void CheckIfGrounded()
    {
        
        var startPoint = transform.position + groundCheckOffset;
        isGrounded = Physics.Raycast(startPoint, Vector3.down, out result, extraHeightText, platformLayerMask);

        //isGrounded = Physics.Raycast(startPoint, Vector3.down * distance, out result, platformLayerMask);
        //Debug.DrawRay(startPoint, Vector3.down * distance);
        //GetMinMaxPos(extraHeightText, startPoint);


        //Debug.DrawRay(startPoint, Vector3.down * extraHeightText, Color.blue);
        OnGroundChanged?.Invoke(isGrounded);
    }

   
    private void GetMinMaxPos(float extraHeightText, Vector3 startPoint)
    {
        if (Physics.Raycast(startPoint, Vector3.down, out result, extraHeightText, platformLayerMask))
        {
            minXpos = result.collider.bounds.min.x;
            maxXpos = result.collider.bounds.max.x;
        }
    }
}
