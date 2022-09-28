using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOSSPHASECONTROLLER : MonoBehaviour
{
    public CustomPlayerController player;
    public GameObject boss1, boss2, boss3;
    public GameObject ui1, ui2, ui3;
    public Image health1, health2, health3;
    public LineRenderer lineRenderer;
    public GameObject ending;
    public int currentBossIndex = 0;
    GameObject currentBoss;
    // Start is called before the first frame update
    void Start()
    {
        ui1.SetActive(false);
        ui2.SetActive(false);
        ui3.SetActive(false);
        ending.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBoss == null)
        {
            switch (currentBossIndex)
            {
                case 0:
                    currentBossIndex += 1;
                    SpawnPhase1();
                    break;
                case 1:
                    currentBossIndex += 1;
                    SpawnPhase2();
                    break;
                case 2:
                    currentBossIndex += 1;
                    SpawnPhase3();
                    break;
                case 3:
                    currentBossIndex += 1;
                    SpawnExit();
                    break;
                default:
                    break;
            }
        }
    }

    public void SpawnPhase1()
    {
        ui1.SetActive(true);
        currentBoss = Instantiate(boss1);
        currentBoss.GetComponent<BOSS1>().HPUI = health1;
    }
    public void SpawnPhase2()
    {
        ui1.SetActive(false);
        ui2.SetActive(true);
        currentBoss = Instantiate(boss2);
        currentBoss.GetComponent<BOSS2>().HPUI = health2;
        currentBoss.GetComponent<BOSS2>().target = player;
        currentBoss.GetComponent<BOSS2>().lineRenderer = lineRenderer;

        player.Heal(100f);
    }
    public void SpawnPhase3()
    {
        ui2.SetActive(false);
        ui3.SetActive(true);
        currentBoss = Instantiate(boss3);
        currentBoss.GetComponent<BOSS3>().HPUI = health3;
        currentBoss.GetComponent<BOSS3>().target = player;
        currentBoss.GetComponent<BOSS3>().lineRenderer = lineRenderer;
        player.Heal(100f);
    }
    public void SpawnExit()
    {
        ui3.SetActive(false);
        ending.SetActive(true);
    }


}
