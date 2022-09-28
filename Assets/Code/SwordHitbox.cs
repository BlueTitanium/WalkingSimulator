using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public CustomPlayerController player;
    public ParticleSystem swordStrike;
    public ParticleSystem environStrike;
    public bool healOnKill = false;
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
        if (other.gameObject.CompareTag("Boss"))
        {
            var b = FindObjectOfType<BOSS2>();
            if (b!= null)
            {
                b.GetComponent<BOSS2>().TakeDamage(player.damage);
            }
            var c = FindObjectOfType<BOSS3>();
            if (c != null)
            {
                c.GetComponent<BOSS3>().TakeDamage(player.damage);
            }
            swordStrike.transform.position = other.gameObject.transform.position;
            swordStrike.Play();
            environStrike.transform.position = transform.position;
            environStrike.Play();
            player.shakeDuration = .1f;
            player.shakeMagnitude = .2f;
            player.curTimeflow = 0f;
            player.zipTimeLeft = 0f;
            if (healOnKill)
            {
                player.Heal(.5f);
                if (c != null)
                {
                    player.Heal(2f);
                }
            }
        }
        if (other.gameObject.CompareTag("BossSword"))
        {
            var b = FindObjectOfType<BOSS2>();
            if (b != null)
            {
                var bscript = b.GetComponent<BOSS2>();
                bscript.TakeDamage(player.damage);
                bscript.state = BOSS2.BossState.isStunned;
                bscript.currentTimer = bscript.stunTime;
            }
            var c = FindObjectOfType<BOSS3>();
            if (c != null)
            {
                var bscript = c.GetComponent<BOSS3>();
                bscript.TakeDamage(player.damage);
                bscript.state = BOSS3.BossState.isStunned;
                bscript.currentTimer = bscript.stunTime;
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(player.damage);
            swordStrike.transform.position = other.gameObject.transform.position;
            swordStrike.Play();
            player.shakeDuration = .1f;
            player.shakeMagnitude = .2f;
            player.curTimeflow = 0f;
            player.zipTimeLeft = 0f;
            if (healOnKill)
            {
                player.Heal(3f);
            }
        } else
        if (other.gameObject.CompareTag("EnemyProjectile"))
        {
            if(other.gameObject.GetComponent<Projectile>().counterSpawned == false)
            {
                player.SpawnProjectile(other.gameObject.transform.position);
                swordStrike.transform.position = other.gameObject.transform.position;
                swordStrike.Play();
                player.curTimeflow = 0f;
                player.shakeDuration = .1f;
                player.shakeMagnitude = .2f;
                player.zipTimeLeft = 0f;
                other.gameObject.GetComponent<Projectile>().counterSpawned = true;
            }
            Destroy(other.gameObject);
        } else
        {
            environStrike.transform.position = transform.position;
            environStrike.Play();
        }
    }
}
