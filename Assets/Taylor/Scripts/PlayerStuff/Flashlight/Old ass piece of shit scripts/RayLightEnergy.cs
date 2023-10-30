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
    public Color superChargeColor;

    private float overheatTimer;
    public float timeUntilOverheat = 5;

    private bool overheat = false;

    private float cooldownTimer;
    public float timeOfCooldown = 3;

    private bool animCheckOne = false;
    private bool animCheckTwo = false;

    private AudioSource audioSource;
    public AudioClip focusSound;
    public AudioClip unfocusSound;

    private bool canFocusSound = false;
    private bool canUnfocusSound = true;

    public AudioClip chargeSound;
    public AudioClip unchargeSound;

    private bool canChargeSound = false;
    private bool canUnchargeSound = true;

    public AudioClip overheatStartOne;
    public AudioClip overheatStartTwo;

    public AudioClip overheated;

    private void Start()
    {
        rayLight = GetComponent<RayLightNEW>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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

                if (canUnchargeSound)
                {
                    audioSource.PlayOneShot(unchargeSound);
                    canChargeSound = true;
                    canUnchargeSound = false;
                }
            }
            else
            {
                flashLightMat.color = superChargeColor;
                overheatTimer -= Time.deltaTime;

                if (canChargeSound)
                {
                    audioSource.PlayOneShot(chargeSound);
                    canChargeSound = false;
                    canUnchargeSound = true;
                }
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


        if (overheatTimer <= 2)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(overheatStartOne, 1f);
            }

            if (!animCheckOne)
            {
                animator.ResetTrigger("reset");
                animator.SetTrigger("trigger");
                animCheckOne = true;
            }
        }

        if (overheatTimer <= 1)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(overheatStartTwo, 1f);
            }

            if (!animCheckTwo)
            {
                animator.ResetTrigger("reset");
                animator.SetTrigger("trigger");
                animCheckTwo = true;
            }
        }

        if (overheatTimer <= 0)
        {
            UnfocusLight();

            audioSource.PlayOneShot(overheated, 1);

            overheat = true;
            cooldownTimer = timeOfCooldown;

            rayLight.SetFOV(0);
            rayLight.SetViewDistance(0);

            animCheckOne = false;
            animCheckTwo = false;

            if(playerHealth.health > 1)
            {
                playerHealth.TakeDamage(1);
            }

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

        if (canUnfocusSound)
        {
            audioSource.PlayOneShot(unfocusSound);
            canFocusSound = true;
            canUnfocusSound = false;
        }
    }

    public void FocusLight()
    {
        rayLight.SetFOV(endAngle);
        rayLight.SetViewDistance(endDist);

        if (canFocusSound)
        {
            if (!mouseControls.kill)
            {
                audioSource.PlayOneShot(focusSound);
            }
            canFocusSound = false;
            canUnfocusSound = true;
        }
    }
}
