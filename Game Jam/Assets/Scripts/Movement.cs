using Unity.VisualScripting;
using UnityEngine;


public class Movement : MonoBehaviour
{
    [Header("Pull and Push")]
    [SerializeField] private LayerMask PullingMasks;
    [SerializeField] private float pullForce = 10f;
    [SerializeField] private float playerPullForce = 5f;
    [SerializeField] private float MaxPullRadius = 4f;
    [SerializeField] private GameObject PointingDotPrefab;
    private GameObject PointingDot;

    private bool IsPulling = false;
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float PushCooldown;
    private bool alreadyPushed;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float enemyDistance;
    [SerializeField] private ParticleSystem mag;
    private bool partActive = false;

    

    [Header("Dash")]
    [SerializeField] private float DashCooldown;
    [SerializeField] private float DashForce;
    private bool alreadyDashed = false;

    [Header("Movement")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float linearDrag = 2f;
    [SerializeField] private float BounceVelocityReduce = 0.7f;
    [SerializeField] private float MaxOverallSpeed = 20f;
    [SerializeField] private LayerMask BounceLayer;

    [Header("Game Flow")]
    [SerializeField] private GameObject GameFlow;
    private GameFlow GameFlowScript;
    [SerializeField] private LayerMask LayerHoles;

    private float horizontal = 0;
    private float vertical = 0;

    [SerializeField] private int index = 0;

    private Rigidbody2D rb;

    private Vector2 LastTrueVelocity;

    private Vector2 inputDirection;

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
        GameFlowScript = GameFlow.GetComponent<GameFlow>();
    }

    void Update()
    {
        LastTrueVelocity = rb.velocity;
        inputDirection = new Vector2(horizontal, vertical);
 
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(inputDirection * acceleration);
        }

        if (rb.velocity.magnitude > MaxOverallSpeed)
        {
            rb.velocity = rb.velocity.normalized * MaxOverallSpeed;
        }
        enemyDistance = Vector2.Distance(transform.position, enemy.transform.position);
        
    }

    private void FixedUpdate()
    {
        Pull();
        DrawPoint();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & LayerHoles) != 0)
        {
            Death();
        }
        
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

                if (colliderAtPoint != null && colliderAtPoint.TryGetComponent<Rigidbody2D>(out Rigidbody2D rbb))
                {
                    
                    rbb.AddForce(direction * pullForce);
                }
                Vector2 partDir = new Vector3(point.x, point.y) - transform.position;
                direction.Normalize();


                GetComponent<Rigidbody2D>().AddForce(-direction * playerPullForce);
                mag.transform.rotation = Quaternion.LookRotation(partDir);

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

            if (enemyDistance <= 5)
            {
                Vector3 pushDirection = enemy.transform.position - transform.position;
                pushDirection.Normalize();
                enemy.GetComponent<Rigidbody2D>().AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                alreadyPushed = true;
                Invoke(nameof(Reset), PushCooldown);
            }

        }
    }

    private void Reset()
    {
        alreadyPushed = false;
    }

    public void Desh()
    {
        if (!alreadyDashed)
        {
            rb.AddForce(inputDirection.normalized * DashForce, ForceMode2D.Impulse);
            
            alreadyDashed = true;
            Invoke(nameof(DashReset), DashCooldown);
        }
    }

    private void DashReset()
    {
        alreadyDashed = false;
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

    private void Death()
    {
        GameFlowScript.PlayerDied(index);
        //PLACE FOR DEATH PARTILES
        Destroy(gameObject);
    }
}