using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GHIntro : MonoBehaviour
{
    public float delay;
    public string sceneName;

    void Start()
    {
        Invoke("LoadMenu", delay);
    }

    void LoadMenu()
    {
        SceneManager.LoadScene(sceneName);
    }
}
