using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoward : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyPatrolWithCoward enemyPatrol;
    private EnemyFlip enemyFlip;

    public float jumpForce = 9f;
    public float jumpSpeedBoost = 10f;

    private float jumpTimer = 0f;
    public float jumpCooldown = 1f;

    public float playerOffset = -1f;

    public EnemyDetection groundDetector;
    public EnemyDetection voidDetector;
    public EnemyDetection wallformDetector;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyPatrol = GetComponent<EnemyPatrolWithCoward>();
        enemyFlip = GetComponent<EnemyFlip>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpTimer <= 0)
        {
            if (wallformDetector.detected && groundDetector.detected)
            {
                Jump();
            }

            if (voidDetector.detected == false && groundDetector.detected)
            {
                Jump();
            }
        }
        else
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    public void Jump()
    {
        rb.AddForce(new Vector2(rb.velocity.x + (jumpSpeedBoost * enemyFlip.direction), jumpForce), ForceMode2D.Impulse);
        jumpTimer = jumpCooldown;
    }
}
