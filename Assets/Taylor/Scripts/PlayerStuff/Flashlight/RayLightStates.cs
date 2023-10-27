using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLightStates : MonoBehaviour
{
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

    private bool currentlyFocusing = false;
    private bool currentlyUnfocusing = false;

    private float focusCooldownTime = 0f;
    public float focusCooldown = 0.5f;
    private float unfocusCooldownTime = 0f;
    public float unfocusCooldown = 0.5f;

    public float focusSpeed = 150f;
    public float unfocusSpeed = 150f;
    public float overheatSpeed = 15f;

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
        if (!mouseControls.focus && unfocusCooldownTime <= 0)
        {
            UnfocusLight();
        }
        else if (mouseControls.focus && focusCooldownTime <= 0)
        {
            FocusLight();
        }

        if (currentlyFocusing)
        {
            FocusLight();
        }
        else if(!currentlyFocusing && mouseControls.kill && unfocusCooldownTime <= 0)
        {
            OverheatLight();
        }
        else if (!mouseControls.kill )
        {
            RevertLight();
        }

        if (currentlyUnfocusing)
        {
            UnfocusLight();
        }

        if (isFocused && unfocusCooldownTime > 0)
        {
            unfocusCooldownTime -= Time.deltaTime;
        }

        if (!isFocused && focusCooldownTime > 0)
        {
            focusCooldownTime -= Time.deltaTime;
        }
    }

    public void UnfocusLight()
    {
        if (isFocused)
        {
            focusCooldownTime = focusCooldown;

            audioSource.PlayOneShot(unfocusSound);
            currentlyUnfocusing = true;
            isFocused = false;
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

        currentSuperAngle = 0;
        superRayLight.SetFOV(currentSuperAngle);

        if (currentDist == startDist && currentAngle == startAngle)
        {
            currentlyUnfocusing = false;
        }
    }

    public void FocusLight()
    {
        if (!isFocused)
        {
            unfocusCooldownTime = unfocusCooldown;

            audioSource.PlayOneShot(focusSound);
            currentlyFocusing = true;
            isFocused = true;
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

        if (currentDist == endDist && currentAngle == endAngle)
        {
            currentlyFocusing = false;
        }
    }

    public void OverheatLight()
    {
        #region Overheat lerp
        float targetAngle = endAngle;
        float journeyLengthAngle = Mathf.Abs(targetAngle - currentSuperAngle);

        float stepAngle = overheatSpeed / journeyLengthAngle * Time.deltaTime;

        currentSuperAngle = Mathf.Lerp(currentSuperAngle, targetAngle, stepAngle);
        superRayLight.SetFOV(currentSuperAngle);
        #endregion 
    }

    public void RevertLight()
    {
        #region Revert lerp
        float targetAngle = 0;
        float journeyLengthAngle = Mathf.Abs(targetAngle - currentSuperAngle);

        float stepAngle = overheatSpeed / journeyLengthAngle * Time.deltaTime;

        currentSuperAngle = Mathf.Lerp(currentSuperAngle, targetAngle, stepAngle);
        superRayLight.SetFOV(currentSuperAngle);
        #endregion 
    }
}