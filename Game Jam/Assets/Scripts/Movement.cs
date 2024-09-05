using Unity.VisualScripting;
using UnityEngine;


public class Movement : MonoBehaviour
{
    [Header("Pull and Push")]
    public GameObject objectToPull;
    public float pullForce = 10f;
    public float playerPullForce = 5f; 



    [Header("Movement")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float linearDrag = 2f;
    [SerializeField] private float BounceVelocityReduce = 0.7f;
    [SerializeField] private LayerMask BounceLayer;

    private float horizontal = 0;
    private float vertical = 0;

    [SerializeField] private int index = 0;

    private Rigidbody2D rb;

    private Vector2 LastTrueVelocity;

    public int GetPlayerIndex()
    {
        return index;
    }

    public void SetInputVector(Vector2 vector)
    {
        horizontal = vector.x;
        vertical = vector.y;
    } 
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = linearDrag; 
    }

    void Update()
    {
        LastTrueVelocity = rb.velocity;
        Vector2 inputDirection = new Vector2(horizontal, vertical);
 
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(inputDirection * acceleration);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // Check if the object we collided with is on the bounce layer
        if (((1 << collision.gameObject.layer) & BounceLayer) != 0)
        {
            
            // Reflect the velocity vector based on the collision normal
            Vector2 reflectedVelocity = Vector2.Reflect(LastTrueVelocity * BounceVelocityReduce, collision.GetContact(0).normal);
            
            // Set the Rigidbody's velocity to the reflected vector to maintain the same speed
            rb.velocity = reflectedVelocity;
        }
    }

    public void Pull()
    {
        // Calculate the direction vector from the object to pull to this object
        Vector3 direction = transform.position - objectToPull.transform.position;

        // Normalize the direction vector to get a unit vector
        direction.Normalize();

        // Apply the force to the object to pull continuously
        objectToPull.GetComponent<Rigidbody2D>().AddForce(direction * pullForce * Time.deltaTime);

        // Apply the force to the player (assuming you have a Rigidbody2D component on the player)
        GetComponent<Rigidbody2D>().AddForce(-direction * playerPullForce * Time.deltaTime);
    }

    public void Push()
    {

    }
}