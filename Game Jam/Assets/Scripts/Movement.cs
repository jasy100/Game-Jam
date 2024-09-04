using UnityEngine;

public class IceMovement : MonoBehaviour
{
    // Variables to adjust in the Unity Inspector
    public float acceleration = 5f; // How quickly the character accelerates
    public float maxSpeed = 10f;    // Maximum speed character can reach
    public float friction = 2f;     // How quickly the character slows down

    private Rigidbody2D rb;
    private Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from the player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the direction the player wants to move
        Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;

        // Accelerate in that direction
        velocity += inputDirection * acceleration * Time.deltaTime;

        // Clamp the velocity to the maximum speed
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        // Apply friction (decelerate when no input is given)
        if (inputDirection.magnitude == 0)
        {
            velocity = Vector2.Lerp(velocity, Vector2.zero, friction * Time.deltaTime);
        }

        // Update the Rigidbody's velocity
        rb.velocity = velocity;
    }
}
