using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformJump : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] float gravityScale = 5;
    [SerializeField] float distance = 2;
    [SerializeField] LayerMask groundLayer;

    float velocity;

    public bool isGrounded;
    [SerializeField] private Vector3 groundCheckOffset;
    RaycastHit result = new RaycastHit();

    [SerializeField] float floorHeight = 0.5f;
    [SerializeField] Transform feet;
    [SerializeField] Collider charCollider;
    Vector3 groundHitPoint;

    // Update is called once per frame
    void Update()
    {
        var gravity = gravityScale;

        velocity += Physics.gravity.y * gravity * Time.deltaTime;

        if(isGrounded)
        {
            //var collider = results[0];
            velocity = 0;
            ResetYPosition();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity = Mathf.Sqrt(jumpHeight * 2 * (Physics.gravity.y * gravityScale));
            }
            //if(collider != null)
            //{
            //Vector3 surface = Physics.ClosestPoint(transform.position, collider, collider.transform.position, collider.transform.rotation) + Vector3.up * floorHeight;
            //transform.position = new Vector3(transform.position.x, surface.y, transform.position.z);

            //}
        }

        
        else
        {
        var startPoint = transform.position + groundCheckOffset;
        isGrounded = Physics.Raycast(startPoint, Vector3.down * distance, out result, groundLayer);
        Debug.DrawRay(startPoint, Vector3.down * distance);
        //isGrounded = Physics.CapsuleCast(startPoint, Vector3.down, out result, extraHeightText);
        groundHitPoint = result.point;
        gravity = isGrounded ? 0 : gravityScale;
        Debug.Log(velocity);
        }
    }

    private void ResetYPosition()
    {
        transform.position = new Vector3(transform.position.x, groundHitPoint.y + charCollider.bounds.size.y / 2, transform.position.z);
    }
}
