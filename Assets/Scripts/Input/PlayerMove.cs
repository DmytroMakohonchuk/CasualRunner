using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    private float yVelocity;
    public CharacterController player;
    public float jumpForce = 10.0f;
    public float moveForce = 5.0f;
    public float gravity = 1.0f;

    private void Start()
    {
        player = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 direction = new Vector3(0, 0, 1);
        Vector3 velocity = direction * speed;

        // Add left/right movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity += Vector3.left * moveForce;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity += Vector3.right * moveForce;
        }

        if (player.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = jumpForce;
            }
        }
        else
        {
            yVelocity -= gravity;
        }

        velocity.y = yVelocity;
        player.Move(velocity * Time.deltaTime);
    }
}