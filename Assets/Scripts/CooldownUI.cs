using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    public Image imageCooldown;
    public float cooldown = 5;
    bool isCooldown;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire3") && !isCooldown)
        {
            imageCooldown.fillAmount = 1;
            isCooldown = true;
        }

        if (isCooldown)
        {
            imageCooldown.fillAmount -= 1 / cooldown * Time.deltaTime;

            if (imageCooldown.fillAmount <= 0)
            {
                imageCooldown.fillAmount = 0;
                isCooldown = false;
            }
        }
    }
}
