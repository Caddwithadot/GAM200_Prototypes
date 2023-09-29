using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlashlightEnergy : MonoBehaviour
{
    public int energy = 3;

    public GameObject spriteOne;
    public GameObject spriteTwo;
    public GameObject spriteThree;

    public bool passiveRegenerate;
    private float regenTimer = 0f;
    public float regenCooldown = 3f;

    // Update is called once per frame
    void Update()
    {
        if (energy == 3)
        {
            spriteOne.SetActive(true);
            spriteTwo.SetActive(true);
            spriteThree.SetActive(true);
        }

        if (energy < 3)
        {
            spriteOne.SetActive(true);
            spriteTwo.SetActive(true);
            spriteThree.SetActive(false);
        }
        
        if(energy < 2)
        {
            spriteOne.SetActive(true);
            spriteTwo.SetActive(false);
            spriteThree.SetActive(false);
        }
        
        if(energy < 1)
        {
            spriteOne.SetActive(false);
            spriteTwo.SetActive(false);
            spriteThree.SetActive(false);
        }

        if (passiveRegenerate && energy < 3)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenCooldown)
            {
                energy++;

                regenTimer = 0f;
            }
        }
    }
}
