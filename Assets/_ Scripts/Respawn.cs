using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public PlayerController PlayerController;

    void OnTriggerEnter2D()
    {
		FindObjectOfType<AudioManager>().Play("grass_jump");
		PlayerController.Respawn();
    }
}
