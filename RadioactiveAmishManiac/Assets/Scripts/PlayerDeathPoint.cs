using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        pc.health = 0;
    }
}
