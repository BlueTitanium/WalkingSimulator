using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    [SerializeField] private TargetType target;
    public enum TargetType {
        bottomLeft,
        upperLeft,
        bottomRight,
        upperRight,
    }
    public Material green;
    public TargetsCount door;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EnemyProjectile"))
        {
            door.AddTarget(target);
            gameObject.GetComponent<Renderer> ().material = green;
        }
    }
}
