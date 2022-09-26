using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSPHASECONTROLLER : MonoBehaviour
{
    public CustomPlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
     *  - 10 drones shooting in a straight direction
     *  - All of the stuff from phase 2 as well
     *  - Fast as the player + more
     *  
     */
}
