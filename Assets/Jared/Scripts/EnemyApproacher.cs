using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyApproacher : MonoBehaviour
{
    public float moveSpeed = 0.75f;
    public float detectionRange = 4f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
