using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public static int collectibleCount = 0;
    public Text collectibleText;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            FindObjectOfType<AudioManager>().Play("Collect");
            collectibleCount ++;
            UpdateCount();
            Destroy(gameObject);
        }
    }

    void UpdateCount()
    {
        collectibleText.text = "Collectibles: " + collectibleCount;
    }
}
