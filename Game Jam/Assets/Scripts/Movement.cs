using UnityEngine;

public class IceMovement : MonoBehaviour
{
    public float acceleration = 5f;   // How quickly the character accelerates
    public float maxSpeed = 10f;      // Maximum speed character can reach
    public float friction = 2f;       // How quickly the character slows down when no input is given
    public float directionChangeFriction = 4f; // Extra friction applied when changing direction

    private Rigidbody2D rb;
    private Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the desired direction
        Vector2 inputDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // Calculate friction and acceleration for the horizontal axis
        if (Mathf.Abs(horizontalInput) > 0)
        {
            float horizontalAngle = Vector2.Angle(new Vector2(velocity.x, 0), new Vector2(horizontalInput, 0));
            float horizontalFrictionFactor = CalculateFrictionFactor(horizontalAngle);

            // Apply friction to the horizontal velocity
            velocity.x = Mathf.Lerp(velocity.x, velocity.x * Mathf.Cos(horizontalAngle * Mathf.Deg2Rad), horizontalFrictionFactor * Time.deltaTime);

            // Accelerate on the horizontal axis
            velocity.x += horizontalInput * acceleration * Time.deltaTime;
        }
        else
        {
            // Apply standard friction when no input is given on the horizontal axis
            velocity.x = Mathf.Lerp(velocity.x, 0, friction * Time.deltaTime);
        }

        // Calculate friction and acceleration for the vertical axis
        if (Mathf.Abs(verticalInput) > 0)
        {
            float verticalAngle = Vector2.Angle(new Vector2(0, velocity.y), new Vector2(0, verticalInput));
            float verticalFrictionFactor = CalculateFrictionFactor(verticalAngle);

            // Apply friction to the vertical velocity
            velocity.y = Mathf.Lerp(velocity.y, velocity.y * Mathf.Cos(verticalAngle * Mathf.Deg2Rad), verticalFrictionFactor * Time.deltaTime);

            // Accelerate on the vertical axis
            velocity.y += verticalInput * acceleration * Time.deltaTime;
        }
        else
        {
            // Apply standard friction when no input is given on the vertical axis
            velocity.y = Mathf.Lerp(velocity.y, 0, friction * Time.deltaTime);
        }

        // Clamp the velocity to the maximum speed
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        // Update the Rigidbody's velocity
        rb.velocity = velocity;
    }

    private float CalculateFrictionFactor(float angle)
    {
        float angleRadians = angle * Mathf.Deg2Rad;
        return Mathf.Cos(angleRadians) + Mathf.Sin(angleRadians) * directionChangeFriction;
    }
}