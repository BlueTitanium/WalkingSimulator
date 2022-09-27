using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    GameObject enemy;
    public string levelName = "Level1";
    private void OnTriggerEnter(Collider other) {
        enemy = GameObject.FindWithTag("Enemy");
        if(other.CompareTag("Player") && enemy == null)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
