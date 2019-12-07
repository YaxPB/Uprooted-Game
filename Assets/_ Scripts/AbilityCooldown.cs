using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    public Image cooldownImage;
    public float cooldownTime;
    bool isCooldown;
    public PlayerController controller;

    void Update()
    {
        if (controller.isDashing)
        {
            isCooldown = true;
        }

        if (isCooldown)
        {
            cooldownImage.fillAmount += 1 / cooldownTime * Time.deltaTime;

            if (cooldownImage.fillAmount >= 1)
            {
                cooldownImage.fillAmount = 0;
                isCooldown = false;
            }
        }
    }
}
