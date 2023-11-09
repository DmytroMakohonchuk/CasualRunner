using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] float distance = 2;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    RaycastHit result = new RaycastHit();

    [SerializeField] private static bool isGrounded;
    public static bool IsGrounded => isGrounded;

    private void CheckIfGrounded()
    {
        var startPoint = transform.position + groundCheckOffset;
        isGrounded = Physics.Raycast(startPoint, Vector3.down * distance, out result, groundLayer);
        Debug.DrawRay(startPoint, Vector3.down * distance);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckIfGrounded();
    }
}
