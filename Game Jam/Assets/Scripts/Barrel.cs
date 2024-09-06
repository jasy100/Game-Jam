using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float pushForce;
    [SerializeField] private float distance;
    [SerializeField] GameObject[] players;
    [SerializeField] ParticleSystem explode;
    [SerializeField] private bool notsploded=true;
    // Start is called before the first frame update
    void Start()
    {
         players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        foreach( GameObject player in players)
        {
            if(((Vector2.Distance(transform.position, player.transform.position))<=radius))
            {

                Debug.Log("hit");
                Vector3 direction = player.transform.position - transform.position;
                direction.Normalize();
                explode.Play();
                player.GetComponent<Rigidbody2D>().AddForce(direction * pushForce, ForceMode2D.Impulse);
                Invoke(nameof(Die), 0.2f);
            }
        }
    }
    void Die()
    {
        this.gameObject.SetActive(false);
    }
}
