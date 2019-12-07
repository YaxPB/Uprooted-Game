using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speed = 30.0f;
    
    void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }
}
