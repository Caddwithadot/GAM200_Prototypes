using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    private float previousPositionX;
    private float startingScaleX;

    public Transform detectors;

    void Start()
    {
        // Initialize the previous position to the initial position
        previousPositionX = transform.position.x;
        startingScaleX = detectors.localScale.x;
    }

    void Update()
    {
        // Get the current position in the X-axis
        float currentPositionX = transform.position.x;

        // Check if the object has moved to the left (decreasing X position)
        if (currentPositionX < previousPositionX)
        {
            detectors.localScale = new Vector2(startingScaleX, detectors.localScale.y);
        }
        // Check if the object has moved to the right (increasing X position)
        else if (currentPositionX > previousPositionX)
        {
            detectors.localScale = new Vector2(-startingScaleX, detectors.localScale.y);
        }

        // Update the previous position
        previousPositionX = currentPositionX;
    }
}