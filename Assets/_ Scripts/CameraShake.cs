using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public RockBreak rockie;
    public float frequency = 25;
    float recoverySpeed = 1.5f;

    // We set trauma to 1 to trigger an impact when the scene is run,
    // for debug purposes. This will later be changed to initialize trauma at 0.
    private float trauma = 1;
    private float seed;

    Vector3 maximumTranslationShake = Vector3.one * 0.5f;
    Vector3 maximumAngularShake = Vector3.one * 2;

    private void Awake()
    {
        seed = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
       if(rockie.hit == true)
        {
            transform.localPosition = new Vector3(
                maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1),
                maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1)
            ) * trauma;

            transform.localRotation = Quaternion.Euler(new Vector3(
                maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
                maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1)
            ) * trauma);

            trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
            rockie.hit = false;
        }
    }
}
