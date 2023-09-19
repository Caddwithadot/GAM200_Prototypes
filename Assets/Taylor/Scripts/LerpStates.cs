using UnityEngine;
using Unity.VisualScripting;

public class LerpStates : MonoBehaviour
{
    private Vector3 initialScale;
    private Vector3 targetScale;
    public float scaleUpSpeed = 150f; // Speed when mouse button is held down
    public float scaleDownSpeed = 50f;    // Speed when mouse button is released

    private bool isLerping = false;
    private bool reverseLerp = false; // Added a flag for reverse lerp
    private float lerpStartTime;

    private void Start()
    {
        initialScale = transform.localScale;
        targetScale = Variables.Object(this).Get<Vector3>("EndLight");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isLerping = true;
            reverseLerp = false; // Set to false for normal lerp
            lerpStartTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isLerping = true;
            reverseLerp = true; // Set to true for reverse lerp
            lerpStartTime = Time.time;
        }

        if (isLerping)
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
            }
        }
    }
}