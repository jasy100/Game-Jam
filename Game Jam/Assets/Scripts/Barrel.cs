using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : MonoBehaviour
{

    [SerializeField] private float pushForce;
    [SerializeField] ParticleSystem explode;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit");
        Vector3 direction = collision.transform.position - transform.position;
        direction.Normalize();
        explode.Play();
        collision.transform.GetComponent<Rigidbody2D>().AddForce(direction * pushForce, ForceMode2D.Impulse);
        Invoke(nameof(Die), 0.2f);
    }
    void Die()
    {
        this.gameObject.SetActive(false);
    }
}
