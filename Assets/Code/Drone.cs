using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public GameObject target;
    public GameObject projectile;
    public LayerMask player;
    public float sphere = 20f;
    float timeLeft = 0f;
    public float shootCD = 1f; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        if (target != null)
        {
            transform.LookAt(target.transform.position);
            if(timeLeft <= 0)
            {
                SpawnProjectile();
                timeLeft += shootCD;
            }
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphere, player);
        if (colliders.Length > 0) {
            target = Camera.main.gameObject;
        }
    }

    public void SpawnProjectile()
    {
        GameObject b = Instantiate(projectile, transform.position, transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = null;
        }
    }
}
