using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public float timeToDie = 1f;
    public float scale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= scale;
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, timeToDie);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
