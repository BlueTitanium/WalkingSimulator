using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetsCount : MonoBehaviour
{
    private List<TargetHit.TargetType> targetList;
    private bool doorUnlocked = false;

    // Start is called before the first frame update
    private void Awake() {
        targetList = new List<TargetHit.TargetType>();
    }

    public void AddTarget(TargetHit.TargetType thisTarget) {
        targetList.Add(thisTarget);
    }

    public bool ContainsTarget(TargetHit.TargetType thisTarget) {
        return targetList.Contains(thisTarget);
    }

    public string levelName = "Level2"; //change when combining levels

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && doorUnlocked)
        {
            SceneManager.LoadScene(levelName);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(targetList.Contains(TargetHit.TargetType.bottomLeft) && targetList.Contains(TargetHit.TargetType.upperLeft) && targetList.Contains(TargetHit.TargetType.bottomRight) && targetList.Contains(TargetHit.TargetType.upperRight)) {
            doorUnlocked = true;
        }
    }
}
