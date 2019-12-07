using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbiltyUI : MonoBehaviour
{
    public GameObject hudImage;

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            hudImage.SetActive(true);
        }
    }
}
