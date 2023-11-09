using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NewTransformJump : MonoBehaviour
{
    [SerializeField] private bool isGrounded;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] float distance = 2;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float gravityScale = 1;
    [SerializeField] Collider collider;
    [SerializeField] float maxJump = 4;

    private Vector3 groundHitPoint;
    private RaycastHit result = new RaycastHit();
    private float velocity;
    private float gravity = 9.81f;
    [SerializeField] private float jumpStrength = 5f;
    private float jumpHeight = 5f;
    private bool isJumping = false;
    private Vector3 jumpVelocity;
    private float startJumpPosY;
    private float maxPos;
    private float jumpPos;

    [SerializeField] private float gravityStrength = 9.81f; // Сила гравітації

    private void Update()
    {
        ApplyGravity();
        Jump();
        HeightCheck();
        StopFalling();
        //HandleJump();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (isJumping == false)
            {
                startJumpPosY = transform.position.y;
                jumpPos = startJumpPosY;
                maxPos = startJumpPosY + maxJump;
                jumpVelocity = Vector3.up * jumpStrength;
                StartCoroutine(LerpJump(maxPos, jumpStrength));
            }

            isJumping = true;         
        }

        if(isJumping)
        {
            //transform.position += jumpVelocity * Time.deltaTime;

            //transform.Translate(0, maxPos, 0);
            transform.position = new Vector3(transform.position.x, jumpPos, transform.position.z);
        }
    }

    IEnumerator LerpJump(float endValue, float duration)
    {
        float time = 0;
        float startValue = jumpPos;
        while (time < duration)
        {
            jumpPos = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        jumpPos = endValue;
    }



    private void FixedUpdate()
    {
        GroundCheck();
        
    }

    private void ApplyGravity()
    {
        Vector3 gravity = Vector3.down * gravityStrength * Time.deltaTime;
        transform.position += gravity;
    }

    private void StopFalling()
    {
        if(isGrounded)
        {
            ResetYPosition();
        }
    }

    //private void HandleJump()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        // Виконуємо стрибок
    //        Vector3 jumpVelocity = Vector3.up * jumpStrength;
    //        transform.position += jumpVelocity * Time.deltaTime;
    //        isJumping = true;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void HeightCheck()
    {
        var currentPos = transform.position.y;
        if(currentPos >= maxPos)
        {
            isJumping = false;
        }
    }

    private void GroundCheck()
    {
        var startPoint = transform.position + groundCheckOffset;
        isGrounded = Physics.Raycast(startPoint, Vector3.down, out result, distance, groundLayerMask);
        Debug.DrawRay(startPoint, Vector3.down * distance);
        groundHitPoint = result.point;
    }

    private void ResetYPosition()
    {
        if(!isJumping && isGrounded)
        {
            transform.position = new Vector3(transform.position.x, groundHitPoint.y + collider.bounds.size.y / 2, transform.position.z);
        }
    }

    
}
