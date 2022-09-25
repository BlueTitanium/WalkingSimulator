using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    float maxHP = 10f;
    public float HP = 10f;
    public Image HPBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        HPBar.fillAmount = HP/maxHP;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if(HP < 0)
        {
            Destroy(gameObject);
        }
    }
}
