using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOSS1 : MonoBehaviour
{
    public float maxAmount = 1;
    public float currentAmount = 1;
    public Image HPUI;
    // Start is called before the first frame update
    void Start()
    {
        maxAmount = FindObjectsOfType<Drone>().Length;
        currentAmount = maxAmount;
        HPUI.fillAmount = currentAmount / maxAmount;
    }

    // Update is called once per frame
    void Update()
    {
        currentAmount = FindObjectsOfType<Drone>().Length;
        HPUI.fillAmount = currentAmount / maxAmount;
        if(currentAmount == 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
