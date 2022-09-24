using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public CustomPlayerController player;
    //on trigger enter
    //if enemy 
    //deal damage
    //if projectile
    //destroy projectile and then spawn counter projectile in same spot in direction of crosshair

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(player.damage);
            player.shakeDuration = .1f;
            player.shakeMagnitude = .2f;
            player.curTimeflow = 0f;
            player.zipTimeLeft = 0f;
        }
        if (other.gameObject.CompareTag("EnemyProjectile"))
        {
            if(other.gameObject.GetComponent<Projectile>().counterSpawned == false)
            {
                player.SpawnProjectile(other.gameObject.transform.position);
                player.curTimeflow = 0f;
                player.shakeDuration = .1f;
                player.shakeMagnitude = .2f;
                player.zipTimeLeft = 0f;
                other.gameObject.GetComponent<Projectile>().counterSpawned = true;
            }
            Destroy(other.gameObject);
        }
    }
}
