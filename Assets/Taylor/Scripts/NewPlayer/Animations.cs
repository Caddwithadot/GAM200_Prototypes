using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private float startingX;
    private float startingY;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        startingX = transform.localScale.x;
        startingY = transform.localScale.y;
    }
    void Update()
    {
        if(rb.velocity.x > 0)
        {
            transform.localScale = new Vector2(startingX, startingY);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector2(-startingX, startingY);
        }
    }
}
