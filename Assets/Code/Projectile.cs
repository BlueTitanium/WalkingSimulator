using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool againstPlayer;
    public float speed = 10f;
    public float pushAmount = 10f;
    public float timeToDie = 3f;
    public float damage = 10f;
    public bool counterSpawned = false;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && againstPlayer)
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * pushAmount, ForceMode.Impulse);
            collision.gameObject.GetComponent<CustomPlayerController>().TakeDamage(damage);
            print("Hit!");
        }
        if (collision.gameObject.CompareTag("Enemy") && !againstPlayer)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
            print("Hit!");
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}
