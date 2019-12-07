using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squish : MonoBehaviour
{
    [SerializeField]
    private Collider2D playerCollider;

    [SerializeField]
    private PlayerController controller;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(playerCollider.Equals(col))
        {
            controller.Respawn();
        }
    }
}
