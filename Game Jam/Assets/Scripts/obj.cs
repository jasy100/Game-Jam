using UnityEngine;

public class Obj : MonoBehaviour
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
       
    }
}