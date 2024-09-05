using UnityEngine;

public class PullObject : MonoBehaviour
{
    public GameObject objectToPull;
    public float pullForce = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Calculate the direction vector from the object to pull to this object
            Vector3 direction = transform.position - objectToPull.transform.position;

            // Normalize the direction vector to get a unit vector
            direction.Normalize();

            // Apply the force to the object to pull
            objectToPull.GetComponent<Rigidbody2D>().AddForce(direction * pullForce);
        }
    }
}