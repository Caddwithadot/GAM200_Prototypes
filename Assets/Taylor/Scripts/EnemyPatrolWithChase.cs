using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolWithChase : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 5.0f; // Speed while chasing
    public float followDistance = 5.0f;
    public float followDuration = 10.0f;
    private Transform targetToFollow;

    private float journeyLength;
    private float startTime;
    public bool isFollowingTarget;
    private float followTimer;
    public bool isReturningToPatrol;
    private float patrolDirection = 1f;
    private Vector3 targetPosition;
    private Vector3 chaseStartPosition;

    void Start()
    {
        targetToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(pointA.position.x, transform.position.y, pointA.position.z);
        journeyLength = Mathf.Abs(pointA.position.x - pointB.position.x);
        startTime = Time.time;
        isFollowingTarget = false;
        followTimer = followDuration;
        isReturningToPatrol = false;
        targetPosition = transform.position;
        chaseStartPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!isFollowingTarget && !isReturningToPatrol)
        {
            Patrol();
        }
        else if (isFollowingTarget)
        {
            FollowTarget();
        }
        else if (isReturningToPatrol)
        {
            ReturnToPatrol();
        }
    }

    private void Patrol()
    {
        float distanceCovered = (Time.time - startTime) * patrolSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        targetPosition = new Vector3(Mathf.Lerp(pointA.position.x, pointB.position.x, fractionOfJourney), transform.position.y, transform.position.z);

        transform.position = targetPosition;

        if (fractionOfJourney >= 1f)
        {
            var temp = pointA;
            pointA = pointB;
            pointB = temp;

            startTime = Time.time;

            patrolDirection *= -1f;
        }

        if (targetToFollow != null && Vector3.Distance(transform.position, targetToFollow.position) < followDistance)
        {
            isFollowingTarget = true;
            followTimer = followDuration;
        }
    }

    private void FollowTarget()
    {
        if (targetToFollow != null)
        {
            targetPosition = new Vector3(targetToFollow.position.x, transform.position.y, transform.position.z);

            // Use chaseSpeed while chasing
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);

            followTimer -= Time.deltaTime;

            if (followTimer <= 0f)
            {
                isFollowingTarget = false;
                isReturningToPatrol = true;
            }
        }
        else
        {
            isFollowingTarget = false;
        }
    }

    private void ReturnToPatrol()
    {
        float direction = Mathf.Sign(pointB.position.x - pointA.position.x);

        // Use patrolSpeed when returning to patrol
        targetPosition = new Vector3(pointA.position.x + direction * journeyLength, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        if (Mathf.Approximately(transform.position.x, targetPosition.x))
        {
            isReturningToPatrol = false;
        }
    }
}