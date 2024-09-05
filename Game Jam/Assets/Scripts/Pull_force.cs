using UnityEngine;

public class Pull_force : MonoBehaviour
{
    public GameObject objectToPull;
    public float pullForce = 10f;
    public float playerPullForce = 5f; // Adjust this to control the player's pull strength
    private bool Pull = false;
    private bool Push = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
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
    }


}
