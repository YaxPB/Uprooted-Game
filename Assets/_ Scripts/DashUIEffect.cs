using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUIEffect : MonoBehaviour
{
    public bool rising;
    public float risingTimer;
    public float risingTime = 1;
    public float startingScale = 0;

    // Start is called before the first frame update
    void Start()
    {
        rising = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(rising)
        {
            risingTimer += Time.deltaTime;
            if(risingTimer > risingTime)
            {
                rising = false;
            }

            Vector3 newScale = transform.localScale;
            //t must be a value of 0-1
            newScale.y = Mathf.Lerp(0,startingScale,risingTimer);
            transform.localScale = newScale;
        }
        else if(Input.GetButtonDown("Dash"))
        {
            StartTrigger();
        }
    }

    public void StartTrigger()
    {
        rising = true;
        Vector3 newScale = transform.localScale;
        startingScale = newScale.y;
        newScale.y = 0;
        transform.localScale = newScale;
        risingTimer = 0;
    }
}
