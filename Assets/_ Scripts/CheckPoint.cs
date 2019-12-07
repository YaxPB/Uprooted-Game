using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public PlayerController controller;

    public void setCheckpoint()
    {
        if (controller.canSetCheckPoint == true)
        {
            FindObjectOfType<AudioManager>().Play("Plant");
            gameObject.SetActive(false);
            controller.spawnPoint = controller.GetComponent<Transform>().position;
        }
    }
}
