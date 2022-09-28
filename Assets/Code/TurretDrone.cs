using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDrone : MonoBehaviour
{
    public AudioClip laser_sound;
    public AudioSource laser_audio;
    public GameObject projectile;
    public float timeLeft = 0f;
    float shootCD = 1f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Enemy>().HP = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        if (timeLeft <= 0)
        {
            SpawnProjectile();
            timeLeft += shootCD;
        }
    }

    public void SpawnProjectile()
    {
        GameObject b = Instantiate(projectile, transform.position, transform.rotation);
        b.GetComponent<Projectile>().timeToDie = 10f;
        GetComponent<AudioSource>().PlayOneShot(laser_sound);
    }

    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnTriggerExit(Collider other)
    {
    }
}
