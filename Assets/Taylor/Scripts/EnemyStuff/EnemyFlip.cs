using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    private float previousPositionX;
    private float detectorsStartX;
    private float spriteStartX;

    public Transform detectors;
    public Transform sprite;

    public int direction = 1;

    void Start()
    {
        // Initialize the previous position to the initial position
        if(detectors != null)
        {
            detectorsStartX = detectors.localScale.x;
        }
        previousPositionX = transform.position.x;
        spriteStartX = sprite.localScale.x;
    }

    void Update()
    {
        // Get the current position in the X-axis
        float currentPositionX = transform.position.x;

        // Check if the object has moved to the left (decreasing X position)
        if (currentPositionX < previousPositionX)
        {
            if(detectors != null)
            {
                detectors.localScale = new Vector2(detectorsStartX, detectors.localScale.y);
            }
            sprite.localScale = new Vector2(spriteStartX, sprite.localScale.y);
            direction = -1;
        }
        // Check if the object has moved to the right (increasing X position)
        else if (currentPositionX > previousPositionX)
        {
            if(detectors != null)
            {
                detectors.localScale = new Vector2(-detectorsStartX, detectors.localScale.y);
            }
            sprite.localScale = new Vector2(-spriteStartX, sprite.localScale.y);
            direction = 1;
        }

        // Update the previous position
        previousPositionX = currentPositionX;
    }
}