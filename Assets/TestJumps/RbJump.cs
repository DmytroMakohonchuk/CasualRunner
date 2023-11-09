using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class RbJump : MonoBehaviour
{
    // Gravity Scale editable on the inspector
    // providing a gravity scale per object
    [Inject] private PlayerController playerController;
    public PlayerSettingsNew playerSettings;

    [SerializeField] private Animator _animator;

    private float defaultGravityScale => playerSettings.defaultGravityScale;
    private float gravityScale = 1f;
    private float fallGravityScale => playerSettings.fallGravityScale;
    private float jumpForce => playerSettings.jumpForce;
    private bool isGrounded;

    // Global Gravity doesn't appear in the inspector. Modify it here in the code
    // (or via scripting) to define a different default gravity for all objects.

    public static float globalGravity = -9.81f;

    private bool requestJump;

    Rigidbody m_rb;

    private void Start()
    {
        playerController.OnJump += RequestJump;
        //playerController.OnGroundChanged += RequestJump;
    }

    void FixedUpdate()
    {
        GravityHandler();
        isGrounded = playerController.IsGrounded;
        HandleFall();
    }

    void OnEnable()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.useGravity = false;
    }

    private void RequestJump(bool isJump)
    {
        requestJump = isJump;
    }

    private void GravityHandler()
    {
        if (m_rb.velocity.y > 0)
        {
            gravityScale = defaultGravityScale;
        }

        else
        {
            gravityScale = fallGravityScale;

            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("FallingIdle"))
            {
                _animator.Play("Landing");
            }

            //_animator.Play("FallingIdle");
        }
    }

    //private void JumpRequester()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    //    {
    //        requestJump = true;
    //    }
    //}

    

    private void HandleFall()
    {
        if (requestJump && isGrounded)
        {
            _animator.CrossFadeInFixedTime("Jump", 0.1f);
            m_rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            requestJump = false;
        }

        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        m_rb.AddForce(gravity, ForceMode.Acceleration);
    }


}
    

