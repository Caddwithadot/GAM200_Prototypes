using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementNEW : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontalInput;
    public float moveSpeed = 4f;
    public float jumpForce = 9f;

    private bool isGrounded;
    public float coyoteTime = 0.1f;
    private float coyoteTimer = 0f;
    public float juffTime = 0.1f;
    private float juffTimer = 0f;

    private AudioSource audioSource;
    public AudioClip[] walkSounds;
    public AudioClip jumpSound;
    public List<string> ignoreTags;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
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
        rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        audioSource.PlayOneShot(jumpSound);
        coyoteTimer = 0f;
        juffTimer = 0f;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y);

        if(horizontalInput != 0 && isGrounded)
        {
            int randomIndex = Random.Range(0, walkSounds.Length);
            AudioClip randomClip = walkSounds[randomIndex];
            audioSource.PlayOneShot(randomClip);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ignoreTags.Contains(collision.tag))
        {
            isGrounded = true;
            coyoteTimer = coyoteTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!ignoreTags.Contains(collision.tag))
        {
            isGrounded = false;
        }
    }
}
