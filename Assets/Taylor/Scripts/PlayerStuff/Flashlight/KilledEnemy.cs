using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledEnemy : MonoBehaviour
{
    [SerializeField] private MouseControls mouseControls;
    private RayLightEnergy rayLightEnergy;

    private bool killedSomething = false;

    private float timerForCooldown = 0f;
    public float cooldownTime = 1f;

    [SerializeField] private PlayerHealth playerHealth;

    private float waitTimer = 0f;
    private float timeToWait = 0.5f;

    private bool finishedWaiting = true;

    private void Start()
    {
        rayLightEnergy = GetComponent<RayLightEnergy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (finishedWaiting)
        {
            timerForCooldown -= Time.deltaTime;
        }

        if(timerForCooldown <= 0)
        {
            mouseControls.canFocus = true;
            killedSomething = false;
        }

        if (!finishedWaiting)
        {
            waitTimer -= Time.deltaTime;
        }

        if(!finishedWaiting && waitTimer < 0)
        {
            timerForCooldown = cooldownTime;
            mouseControls.canFocus = false;
            rayLightEnergy.UnfocusLight();

            if(playerHealth.health > 1)
            {
                playerHealth.TakeDamage(1);
            }

            killedSomething = true;
            finishedWaiting = true;
        }
    }

    public void Killed()
    {
        if (!killedSomething)
        {
            waitTimer = timeToWait;
            finishedWaiting = false;
        }
    }
}
