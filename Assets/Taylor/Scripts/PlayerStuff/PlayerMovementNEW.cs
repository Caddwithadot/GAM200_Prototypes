using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementNEW : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontalInput;
    public float moveSpeed = 4f;
    public float jumpForce = 9f;

    public bool isGrounded;
    public float coyoteTime = 0.5f;
    private float coyoteTimer = 0f;
    public float juffTime = 0.15f;
    private float juffTimer = 0f;

    private AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip jumpSound;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if(horizontalInput != 0 && isGrounded)
        {
            anim.SetTrigger("Walk");
        }
        else if(horizontalInput == 0 && isGrounded)
        {
            anim.SetTrigger("Idle");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            juffTimer = juffTime;

            if (isGrounded || coyoteTimer > 0f)
            {
                Jump();
            }
        }

        if (isGrounded && juffTimer > 0f)
        {
            Jump();
        }

        if (!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
            juffTimer -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        if(Time.timeScale > 0)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpSound, 0.2f);
            coyoteTimer = 0f;
            juffTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y);

        if(horizontalInput != 0 && isGrounded && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walkSound, 0.2f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Environment" || collision.tag == "JumpThrough")
        {
            isGrounded = true;
            coyoteTimer = coyoteTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Environment" || collision.tag == "JumpThrough")
        {
            isGrounded = false;
        }
    }
}
