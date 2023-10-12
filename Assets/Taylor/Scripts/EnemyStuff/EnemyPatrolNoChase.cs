using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolNoChase : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2.0f; // Fixed speed for patrolling

    private float journeyLength;
    private float startTime;
    private bool isReversing; // Indicates whether the enemy is reversing direction
    private Vector3 startLerpPosition; // Starting position for lerping back

    public EnemyDetection voidDetector;
    public EnemyDetection wallformDetector;

    void Start()
    {
        transform.position = new Vector3(pointA.position.x, transform.position.y, pointA.position.z); // Ignore Y position
        journeyLength = Vector3.Distance(pointA.position, pointB.position);
        startTime = Time.time;
    }

    private void Update()
    {
        if ((!voidDetector.detected || wallformDetector.detected) && !isReversing)
        {
            isReversing = true;
            startLerpPosition = transform.position;
            journeyLength = Vector3.Distance(startLerpPosition, isReversing ? pointA.position : pointB.position);
            startTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (isReversing)
        {
            float distanceCovered = (Time.time - startTime) * patrolSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            Vector3 newPosition = Vector3.Lerp(startLerpPosition, isReversing ? pointA.position : pointB.position, fractionOfJourney);

            // Ignore Y position
            newPosition.y = transform.position.y;

            transform.position = newPosition;

            if (fractionOfJourney >= 1f)
            {
                isReversing = false; // Reset the flag
                startLerpPosition = transform.position;
                journeyLength = Vector3.Distance(startLerpPosition, isReversing ? pointA.position : pointB.position);
                startTime = Time.time;
            }
        }
        else
        {
            float distanceCovered = (Time.time - startTime) * patrolSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            Vector3 newPosition = Vector3.Lerp(pointA.position, pointB.position, fractionOfJourney);

            // Ignore Y position
            newPosition.y = transform.position.y;

            transform.position = newPosition;

            if (fractionOfJourney >= 1f)
            {
                // Swap points
                var temp = pointA;
                pointA = pointB;
                pointB = temp;
                startTime = Time.time;
                journeyLength = Vector3.Distance(startLerpPosition, pointA.position);
            }
        }
    }
}