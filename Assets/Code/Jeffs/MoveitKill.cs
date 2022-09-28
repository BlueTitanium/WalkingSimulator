using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MoveitKill : MonoBehaviour
{
    float speed = 5;
    bool d = true;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform2"))
        {
            if(d){
                    d = false;
                
            }
            else{
                    d = true;
                    
            }

        }
        if(collision.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (d == true){
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;
            transform.position = pos;
        }
        else {
            Vector3 pos = transform.position;
            pos.x -= speed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
