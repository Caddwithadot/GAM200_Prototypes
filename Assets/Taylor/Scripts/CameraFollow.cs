using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public float yOffset;

    public float xOffsetInterpolant = 0.15f;
    public float yOffsetInterpolant = 0.05f;

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Calculate the desired x and y positions separately.
        float targetX = followTarget.position.x;
        float targetY = followTarget.position.y + yOffset;

        // Interpolate the x and y positions separately.
        float newX = Mathf.Lerp(transform.position.x, targetX, xOffsetInterpolant);
        float newY = Mathf.Lerp(transform.position.y, targetY, yOffsetInterpolant);

        // Set the new position.
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
