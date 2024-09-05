using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;


public class Movement : MonoBehaviour
{
    [Header("Pull and Push")]
    [SerializeField] private LayerMask PullingMasks;
    [SerializeField] private float pullForce = 10f;
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float playerPullForce = 5f;
    [SerializeField] private float MaxPullRadius = 4f;
    [SerializeField] private GameObject PointingDotPrefab;
    [SerializeField] private GameObject enemy;
    
    [SerializeField] private float cooldown;
    private GameObject PointingDot;

    private bool IsPulling = false;
    private bool alreadyPushed;


    [Header("Movement")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float linearDrag = 2f;
    [SerializeField] private float BounceVelocityReduce = 0.7f;
    [SerializeField] private float MaxOverallSpeed = 20f;
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

        if (rb.velocity.magnitude > MaxOverallSpeed)
        {
            rb.velocity = rb.velocity.normalized * MaxOverallSpeed;
        }

        
    }

    private void FixedUpdate()
    {
        Pull();
        DrawPoint();
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

    private void Pull()
    {
        if (IsPulling)
        {
            Vector2 point = FindNearestPoint();
            if (point != Vector2.zero)
            {

                Vector3 direction = transform.position - new Vector3(point.x, point.y, 0);


                direction.Normalize();

                Collider2D colliderAtPoint = Physics2D.OverlapPoint(point);


                if (colliderAtPoint != null)
                {
                    colliderAtPoint.GetComponent<Rigidbody2D>().AddForce(direction * pullForce);
                }


                GetComponent<Rigidbody2D>().AddForce(-direction * playerPullForce);
            }
        }
    }

    public void SetPull(bool pull)
    {
        IsPulling = pull;
    }

    public void Push()
    {
        if (!alreadyPushed)
        {
            Vector3 direction = enemy.transform.position - transform.position;
            direction.Normalize();
            enemy.GetComponent<Rigidbody2D>().AddForce(direction * pushForce);
            alreadyPushed = true;
            Invoke(nameof(Reset), cooldown);
        }
    }

    public void Reset()
    {
        alreadyPushed = false;
    }
    private Vector2 FindNearestPoint()
    {
        // Get all colliders within the search radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, MaxPullRadius, PullingMasks);

        Vector2 nearestPoint = new Vector2();
        float nearestDistance = Mathf.Infinity;

        // Iterate through all colliders to find the nearest one
        foreach (Collider2D hitCollider in hitColliders)
        {
            Vector2 closestPoint = Physics2D.ClosestPoint(transform.position, hitCollider);
            float distance = Vector2.Distance(transform.position, closestPoint);

            // Check if this collider is closer than the previous nearest
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPoint = closestPoint;
            }
        }
        if (hitColliders.Length == 0)
        {
            return Vector2.zero;
        }
        else
        {
            return nearestPoint;
        }
    }

    private void DrawPoint()
    {
        if (PointingDotPrefab != null)
        {
            Vector2 point = FindNearestPoint();
            if (point != Vector2.zero)
            {
                if (PointingDot == null)
                {
                    PointingDot = GameObject.Instantiate(PointingDotPrefab, FindNearestPoint(), Quaternion.identity);
                }
                else
                {
                    PointingDot.transform.position = FindNearestPoint();
                }
            }
            else
            {
                if (PointingDot != null)
                {
                    Destroy(PointingDot);
                }
            }
        }
    }
}