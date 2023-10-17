using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLightEnergy : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private MouseControls mouseControls;
    private RayLightNEW rayLight;

    private Animator animator;

    public float startAngle = 45f;
    public float startDist = 4f;

    public float endAngle = 8f;
    public float endDist = 12f;

    public Material flashLightMat;
    public Color defaultColor;
    public Color focusColor;

    private float overheatTimer;
    public float timeUntilOverheat = 5;

    private bool overheat = false;

    private float cooldownTimer;
    public float timeOfCooldown = 3;

    private bool animCheckOne = false;
    private bool animCheckTwo = false;

    private void Start()
    {
        rayLight = GetComponent<RayLightNEW>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!overheat)
        {
            if (!mouseControls.focus)
            {
                UnfocusLight();
            }
            else
            {
                FocusLight();
            }

            if (!mouseControls.kill)
            {
                flashLightMat.color = defaultColor;
                overheatTimer = timeUntilOverheat;
            }
            else
            {
                flashLightMat.color = focusColor;
                overheatTimer -= Time.deltaTime;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;

            if(cooldownTimer <= 0)
            {
                overheat = false;

                UnfocusLight();
            }
        }


        if (overheatTimer <= 2 && !animCheckOne)
        {
            animator.ResetTrigger("reset");
            animator.SetTrigger("trigger");
            animCheckOne = true;
        }

        if (overheatTimer <= 1 && !animCheckTwo)
        {
            animator.ResetTrigger("reset");
            animator.SetTrigger("trigger");
            animCheckTwo = true;
        }

        if (overheatTimer <= 0)
        {
            UnfocusLight();

            overheat = true;
            cooldownTimer = timeOfCooldown;

            rayLight.SetFOV(0);
            rayLight.SetViewDistance(0);

            animCheckOne = false;
            animCheckTwo = false;

            playerHealth.TakeDamage(1);
            overheatTimer = timeUntilOverheat;
        }
    }

    public void UnfocusLight()
    {
        animator.ResetTrigger("trigger");
        animator.SetTrigger("reset");
        GetComponent<MeshRenderer>().enabled = true;

        rayLight.SetFOV(startAngle);
        rayLight.SetViewDistance(startDist);
    }

    public void FocusLight()
    {
        rayLight.SetFOV(endAngle);
        rayLight.SetViewDistance(endDist);
    }
}
