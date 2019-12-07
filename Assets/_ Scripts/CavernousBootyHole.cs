using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernousBootyHole : MonoBehaviour
{
    public AudioManager _AudioManager;
    private bool enteredBooty;

    private void Start()
    {
        enteredBooty = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _AudioManager.Play("Cave");
            enteredBooty = true;
        }
    }

    private void Update()
    {
        if(enteredBooty)
        {
            _AudioManager.sounds[1].pitch--;
            _AudioManager.sounds[1].volume--;

            _AudioManager.sounds[10].pitch--;
            _AudioManager.sounds[10].volume--;
        }
    }
}
