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
    int currentBossIndex = 0;
    GameObject currentBoss;
    // Start is called before the first frame update
    void Start()
    {
        ui1.SetActive(false);
        ui2.SetActive(false);
        ui3.SetActive(false);
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
                    SpawnExit();
                    break;
                default:
                    SpawnExit();
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
        player.Heal(100f);
    }
    public void SpawnPhase3()
    {
        ui2.SetActive(false);
        player.Heal(100f);
    }
    public void SpawnExit()
    {
        ui3.SetActive(false);
        currentBoss = Instantiate(ending);
    }


    //3 PHASES
    /*
     * Phase 1: Spawn a swarm of enemies that move back and forth and shoot at the player 
     *  - Swarm HP is just the total amount of enemies remaining / the max amount of enemies that have been spawned
     * Phase 2: Spawn the Mad Scientist (guy has a sword)
     *  - Refill player health
     *  - Mad Scientist can dash to the player (fixed speed (fairly fast)) if the player is far enough away and the scientist will do a thrust attack
     *  - if the player is in range, the mad scientist will do a circular swing in the direction of the player.
     *  - If the player attacks during his attack at the right timing, the parry spot will appear and the blade can be parried and the scientist will be stunned.
     *  - The parry will also heal the player (maybe).
     * Phase 3: 
     *  - Refill player health
     *  - Six drones with swords 
     *  - 4 drones shooting in a straight direction
     *  - All of the stuff from phase 2 as well
     *  - Fast as the player + more
     *  
     */


}
