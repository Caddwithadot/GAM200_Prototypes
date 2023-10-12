using UnityEngine;
using System.Collections;

public class FlashlightStates : MonoBehaviour
{
    private Vector3 initialScale;
    public Vector3 targetScale;
    public Vector3 overheatingScale;

    public float scaleUpSpeed = 100f; // Speed when mouse button is held down
    public float scaleDownSpeed = 50f; // Speed when mouse button is released
    public float overheatSpeed = 10f;

    private bool isLerping = false;
    private bool reverseLerp = true; // Added a flag for reverse lerp
    private float lerpStartTime;

    private float activatedTimer = 0f;
    public float timeToOverheat = 3f;

    private Animator animator;

    private bool isOverheatingLerp = false; // Flag to track overheating lerp
    private bool hasStartedOverheatingLerp = false; // Flag to track if overheating lerp has started

    private float cooldownTimer = 0f;
    public float overheatCooldown = 3f; // Cooldown time after overheating

    private bool isCooldown = false; // Flag to track cooldown state
    private bool recovering = false;

    public bool useEnergy = true;
    public bool useHealth = false;
    private GameObject player;

    private void Start()
    {
        initialScale = transform.localScale;

        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!isCooldown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isLerping = true;
                reverseLerp = false; // Set to false for normal lerp
                lerpStartTime = Time.time;

                animator.ResetTrigger("Enable");
                animator.ResetTrigger("Dying");
                recovering = false;
            }

            if (Input.GetMouseButtonUp(0) && !recovering)
            {
                isLerping = true;
                reverseLerp = true; // Set to true for reverse lerp
                lerpStartTime = Time.time;

                animator.ResetTrigger("Activate");
                animator.ResetTrigger("Dying");
                animator.SetTrigger("Deactivate");

                activatedTimer = 0f;

                // Stop the overheating lerp if it's in progress
                isOverheatingLerp = false;

                // Reset the flag to allow overheating lerp to start again
                hasStartedOverheatingLerp = false;
            }
        }
        else
        {
            // Player is in cooldown, increment the cooldown timer
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= overheatCooldown && GetComponent<FlashlightEnergy>().energy >= 1 && player.GetComponent<PlayerHealth>().health >= 1)
            {
                // Cooldown period is over, reset flags and timer
                isCooldown = false;
                cooldownTimer = 0f;

                transform.localScale = initialScale;

                animator.ResetTrigger("Activate");
                animator.ResetTrigger("Disable");
                animator.SetTrigger("Enable");

                activatedTimer = 0f;
                hasStartedOverheatingLerp = false;

                recovering = true;
            }
        }

        // Check if the overheating lerp flag is set and perform the overheating lerp
        if (isOverheatingLerp)
        {
            OverheatLerp();
        }

        // Activated flashlight
        if (isLerping && !isOverheatingLerp && !recovering)
        {
            FlashlightLerping();
        }
        else if (!reverseLerp && !isOverheatingLerp && !hasStartedOverheatingLerp && !recovering)
        {
            activatedTimer += Time.deltaTime;

            // Flashlight overheat
            if (activatedTimer >= timeToOverheat)
            {
                // Start the overheating lerp
                StartOverheatLerp();
            }
        }
    }

    private void FlashlightLerping()
    {
        float journeyLength = Vector3.Distance(initialScale, targetScale);
        float distanceCovered = (Time.time - lerpStartTime) * (reverseLerp ? scaleDownSpeed : scaleUpSpeed); // Use different speeds
        float fractionOfJourney = distanceCovered / journeyLength;

        // Depending on reverseLerp flag, lerp either from initialScale to targetScale or vice versa
        if (!reverseLerp)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, fractionOfJourney);
        }
        else
        {
            transform.localScale = Vector3.Lerp(targetScale, initialScale, fractionOfJourney);
        }

        // Check if we reached the target scale
        if (fractionOfJourney >= 1.0f)
        {
            if (!reverseLerp)
            {
                transform.localScale = targetScale; // Ensure it's exactly the target scale
            }
            else
            {
                transform.localScale = initialScale; // Ensure it's exactly the initial scale
            }

            isLerping = false; // Stop lerping

            // Fully extended
            if (!reverseLerp)
            {
                animator.ResetTrigger("Deactivate");
                animator.SetTrigger("Activate");
            }
        }
    }

    private void StartOverheatLerp()
    {
        isOverheatingLerp = true;
        hasStartedOverheatingLerp = true; // Set flag to true to prevent repeated starts
        lerpStartTime = Time.time;

        animator.SetTrigger("Dying");

        // Reset the recovering flag
        recovering = false;
    }

    private void OverheatLerp()
    {
        float journeyLength = Vector3.Distance(targetScale, overheatingScale);
        float distanceCovered = (Time.time - lerpStartTime) * overheatSpeed; // Use overheat cooldown speed
        float fractionOfJourney = distanceCovered / journeyLength;

        transform.localScale = Vector3.Lerp(targetScale, overheatingScale, fractionOfJourney);

        // Check if we reached the overheating position
        if (fractionOfJourney >= 1.0f)
        {
            // Stop the overheating lerp
            isOverheatingLerp = false;

            // Reset the lerp start time for cooldown
            lerpStartTime = Time.time;

            // Start the cooldown period
            isCooldown = true;

            animator.ResetTrigger("Dying");
            animator.SetTrigger("Disable");

            if (useEnergy)
            {
                GetComponent<FlashlightEnergy>().energy--;
            }

            if (useHealth)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(1);
            }
        }
    }
}