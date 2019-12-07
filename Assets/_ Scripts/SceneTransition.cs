using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string nextScene;
    public float sceneTransitionDelay;
    public FadeOut fadeScreen;
    public BoxCollider2D colliderScene;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(DelayedSceneLoad());
            fadeScreen.Fade();
            Debug.Log("Loading Scene");
            colliderScene.enabled = false;
        }
    }

    IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(sceneTransitionDelay);
        SceneManager.LoadScene(nextScene);
    }
}
    
