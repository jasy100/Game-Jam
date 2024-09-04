using UnityEngine;

public class IceMovementAddForce : MonoBehaviour
{
    // Variables to adjust in the Unity Inspector
    public float acceleration = 10f; // How quickly the character accelerates
    public float maxSpeed = 5f;      // Maximum speed character can reach
    public float linearDrag = 2f;    // Linear drag to simulate friction

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = linearDrag; // Set the Rigidbody's linear drag to the value defined
    }

    void Update()
    {
        // Get input from the player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the direction the player wants to move
        Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;

        // Apply force in that direction if the speed is less than max speed
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(inputDirection * acceleration);
        }

        // Optional: Clamp the velocity if it exceeds max speed (can happen in rare cases)
        /*
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        */
    }
}