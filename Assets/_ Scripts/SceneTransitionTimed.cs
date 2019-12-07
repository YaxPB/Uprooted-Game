using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTimed : MonoBehaviour
{
    public string nextScene;
    public float creditsDuration;

    void Start()
    {
        Invoke("NextScene", creditsDuration); 
    }

    void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
