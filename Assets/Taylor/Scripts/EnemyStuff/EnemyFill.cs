using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFill : MonoBehaviour
{
    private MouseControls mouseControls;
    public SpriteRenderer spriteHighlight;

    public GameObject enemy;
    public Transform playerTrigger;

    private Rigidbody2D rb;
    private Animator anim;
    public SpriteRenderer sr;
    private ParticleSystem ps;

    private MonoBehaviour[] scriptsOnEnemy;

    public float bakeAmount = 0f;
    public float fillSpeed = 0.5f;
    public float revertSpeed = 2f;

    private bool startFill = false;
    private bool startRevert = false;

    private Color startColor;
    private Color currentColor;
    public Color targetColor;

    private Vector2 startSize;
    private Vector2 currentSize;
    public Vector2 targetSize;

    private void Start()
    {
        scriptsOnEnemy = enemy.GetComponents<MonoBehaviour>();
        mouseControls = GameObject.Find("MouseControls").GetComponent<MouseControls>();

        rb = enemy.GetComponent<Rigidbody2D>();
        anim = enemy.GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();

        startColor = sr.color;
        startSize = transform.localScale;
    }

    public void StartFilling()
    {
        startFill = true;
        startRevert = false;
    }

    public void StopFilling()
    {
        startFill = false;
        startRevert = true;
    }

    private void Update()
    {
        if (startFill)
        {
            sr.color = Color.Lerp(currentColor, targetColor, Time.deltaTime * fillSpeed);
            transform.localScale = Vector2.Lerp(currentSize, targetSize, Time.deltaTime * fillSpeed);
        }

        if (startRevert && bakeAmount > 0)
        {
            sr.color = Color.Lerp(currentColor, startColor, Time.deltaTime * revertSpeed);
            transform.localScale = Vector2.Lerp(currentSize, startSize, Time.deltaTime * revertSpeed);
        }

        currentColor = sr.color;
        currentSize = transform.localScale;
        bakeAmount = transform.localScale.x - startSize.x;

        if(bakeAmount >= 0.75)
        {
            anim.SetTrigger("Pop");
            ps.Play();

            rb.isKinematic = true;
            GetComponent<BoxCollider2D>().enabled = false;
            playerTrigger.GetComponent<BoxCollider2D>().enabled = false;

            foreach(var script in scriptsOnEnemy)
            {
                script.enabled = false;
            }

            GetComponent<EnemyFill>().enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Light" && mouseControls.kill)
        {
            StartFilling();
        }

        if (collision.tag == "Light" || collision.tag == "PlayerAura")
        {
            spriteHighlight.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Light" && mouseControls.kill)
        {
            StartFilling();
        }

        if (collision.tag == "Light" || collision.tag == "PlayerAura")
        {
            spriteHighlight.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!mouseControls.kill)
        {
            StopFilling();
        }

        if (collision.tag == "Light" || collision.tag == "PlayerAura")
        {
            spriteHighlight.enabled = false;
        }
    }
}
