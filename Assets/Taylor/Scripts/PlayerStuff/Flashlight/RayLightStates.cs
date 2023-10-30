using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLightStates : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private MouseControls mouseControls;
    private RayLightNEW rayLight;
    public SuperRayLight superRayLight;

    public float startAngle = 45f;
    public float startDist = 4f;
    public float endAngle = 8f;
    public float endDist = 12f;

    private float currentAngle;
    private float currentDist;

    private float currentSuperAngle = 0f;

    public Material flashLightMat;
    public Color defaultColor;
    public Color superChargeColor;

    private AudioSource audioSource;
    public AudioClip focusSound;
    public AudioClip unfocusSound;

    private bool isFocused = false;
    private bool isKilling = true;

    private bool finishedFocusing = false;
    private bool finishedUnfocusing = false;
    private bool finishedOverheating = false;

    private float focusCooldownTime = 0f;
    public float focusCooldown = 0.5f;
    private float unfocusCooldownTime = 0f;
    public float unfocusCooldown = 0.5f;

    public float focusSpeed = 150f;
    public float unfocusSpeed = 150f;
    public float overheatSpeed = 5f;
    public float revertSpeed = 15f;

    private bool overheatStunPlaying = false;

    public Animator flickerAni;
    public MeshRenderer superMesh;

    private float time;


    private void Start()
    {
        rayLight = GetComponent<RayLightNEW>();
        audioSource = GetComponent<AudioSource>();

        currentAngle = startAngle;
        currentDist = startDist;

        superRayLight.SetFOV(currentSuperAngle);
        superRayLight.SetViewDistance(endDist);
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouseControls.focus && unfocusCooldownTime <= 0 || !finishedUnfocusing)
        {
            if (!overheatStunPlaying)
            {
                UnfocusLight();
            }
        }
        else if (mouseControls.focus && focusCooldownTime <= 0 || !finishedFocusing)
        {
            if (!overheatStunPlaying)
            {
                FocusLight();
            }
        }

        if (mouseControls.kill && unfocusCooldownTime <= 0 && !finishedOverheating)
        {
            OverheatLight();
        }
        else if (!mouseControls.kill || focusCooldownTime <= 0 || finishedOverheating)
        {
            RevertLight();
        }

        if (!isFocused && unfocusCooldownTime > 0)
        {
            unfocusCooldownTime -= Time.deltaTime;
        }

        if (isFocused && focusCooldownTime > 0)
        {
            focusCooldownTime -= Time.deltaTime;
        }
    }

    public void UnfocusLight()
    {
        if (!isFocused)
        {
            focusCooldownTime = focusCooldown;

            currentSuperAngle = 0;
            superRayLight.SetFOV(currentSuperAngle);
            finishedOverheating = false;
            isKilling = true;

            audioSource.PlayOneShot(unfocusSound);
            finishedUnfocusing = false;
            isFocused = true;
        }

        #region Unfocus lerp
        float targetAngle = startAngle;
        float targetDist = startDist;

        float journeyLengthAngle = Mathf.Abs(targetAngle - currentAngle);
        float journeyLengthDist = Mathf.Abs(targetDist - currentDist);

        float stepAngle = unfocusSpeed / journeyLengthAngle * Time.deltaTime;
        float stepDist = unfocusSpeed / journeyLengthDist * Time.deltaTime;

        currentAngle = Mathf.Lerp(currentAngle, targetAngle, stepAngle);
        rayLight.SetFOV(currentAngle);

        currentDist = Mathf.Lerp(currentDist, targetDist, stepDist);
        rayLight.SetViewDistance(currentDist);
        #endregion

        //fully unfocused
        if (currentDist == startDist && currentAngle == startAngle)
        {
            finishedUnfocusing = true;
        }
    }

    public void FocusLight()
    {
        if (isFocused)
        {
            unfocusCooldownTime = unfocusCooldown;

            audioSource.PlayOneShot(focusSound);
            finishedFocusing = false;
            isFocused = false;
        }

        #region Focus lerp
        float targetAngle = endAngle;
        float targetDist = endDist;

        float journeyLengthAngle = Mathf.Abs(targetAngle - currentAngle);
        float journeyLengthDist = Mathf.Abs(targetDist - currentDist);

        float stepAngle = focusSpeed / journeyLengthAngle * Time.deltaTime;
        float stepDist = focusSpeed / journeyLengthDist * Time.deltaTime;

        currentAngle = Mathf.Lerp(currentAngle, targetAngle, stepAngle);
        rayLight.SetFOV(currentAngle);

        currentDist = Mathf.Lerp(currentDist, targetDist, stepDist);
        rayLight.SetViewDistance(currentDist);
        #endregion

        //fully focused
        if (currentDist == endDist && currentAngle == endAngle)
        {
            finishedFocusing = true;
        }
    }

    public void OverheatLight()
    {
        if (isKilling)
        {
            finishedOverheating = false;
            isKilling = false;
        }

        #region Overheat lerp
        float targetAngle = endAngle;
        float journeyLengthAngle = Mathf.Abs(targetAngle - currentSuperAngle);

        float stepAngle = overheatSpeed / journeyLengthAngle * Time.deltaTime;

        currentSuperAngle = Mathf.Lerp(currentSuperAngle, targetAngle, stepAngle);
        superRayLight.SetFOV(currentSuperAngle);
        #endregion

        //handle the flickering
        if(currentSuperAngle >= (endAngle / 4) && currentSuperAngle < (endAngle / 3))
        {
            flickerAni.enabled = true;
        }
        else if(currentSuperAngle >= (endAngle / 2) && currentSuperAngle < (endAngle / 3) * 2)
        {
            flickerAni.enabled = true;
        }
        else
        {
            flickerAni.enabled = false;
            superMesh.enabled = true;
        }

        time += Time.deltaTime;

        //fully overheat
        if (currentSuperAngle == endAngle)
        {
            Debug.Log("Time to overheat: " + time);

            if (playerHealth.health > 1)
            {
                playerHealth.TakeDamage(1);
            }
            currentSuperAngle = 0;
            superRayLight.SetFOV(currentSuperAngle);

            //rayLight.SetFOV(startAngle * 1.5f);
            //rayLight.SetViewDistance(endDist);

            //overheatStunPlaying = true;
            finishedOverheating = true;
        }
    }

    public void RevertLight()
    {
        if(!isKilling)
        {
            isKilling = true;
        }

        #region Revert lerp
        float targetAngle = 0;
        float journeyLengthAngle = Mathf.Abs(targetAngle - currentSuperAngle);

        float stepAngle = revertSpeed / journeyLengthAngle * Time.deltaTime;

        currentSuperAngle = Mathf.Lerp(currentSuperAngle, targetAngle, stepAngle);
        superRayLight.SetFOV(currentSuperAngle);
        #endregion 

        flickerAni.enabled = false;
        superMesh.enabled = true;
    }
}