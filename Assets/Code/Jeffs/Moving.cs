using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    bool d = true;
    public float speed = 5;
    public int distance = 20;
    Vector3 originalPos;
    void Start()
    {
        originalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z); //platform
        
    }
    // Update is called once per frame
    void Update()
    {
    
    
        if ((originalPos[0] + distance) >= transform.position.x){
            
            d = true;
        }
        else if ((originalPos[0]) >= transform.position.x){
            
            d = false;
        } 
        if (d == true){
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;
            transform.position = pos;
        }
        else{
            Vector3 pos = transform.position;
            pos.x -= speed * Time.deltaTime;
            transform.position = pos;
        }
            
    }
}