using UnityEngine;


public class Movement : MonoBehaviour
{

    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float linearDrag = 2f;

    private float horizontal = 0;
    private float vertical = 0;

    [SerializeField] private int index = 0;

    private Rigidbody2D rb;

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

        Vector2 inputDirection = new Vector2(horizontal, vertical);
 
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(inputDirection * acceleration);
        }

    }
}