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

    private void Update()
    {
        if (startFill)
        {
            Fill();
        }

        if (startRevert && currentColor != startColor)
        {
            Revert();
        }

        currentColor = sr.color;
        currentSize = transform.localScale;

        if( currentColor == targetColor)
        {
            rb.isKinematic = true;
            GetComponent<BoxCollider2D>().enabled = false;
            playerTrigger.GetComponent<BoxCollider2D>().enabled = false;

            //anim.SetTrigger("Pop");
            //ps.Play();

            foreach(var script in scriptsOnEnemy)
            {
                script.enabled = false;
            }

            GetComponent<EnemyFill>().enabled = false;
        }
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

    public void Fill()
    {
        float journeyLengthScale = Mathf.Abs(targetSize.x - currentSize.x);
        float stepScale = fillSpeed / journeyLengthScale * Time.deltaTime;

        float newSize = Mathf.Lerp(currentSize.x, targetSize.x, stepScale);
        transform.localScale = new Vector2(newSize, newSize);

        sr.color = Color.Lerp(currentColor, targetColor, stepScale);
    }

    public void Revert()
    {
        float journeyLengthScale = Mathf.Abs(startSize.x - currentSize.x);
        float stepScale = revertSpeed / journeyLengthScale * Time.deltaTime;

        float newSize = Mathf.Lerp(currentSize.x, startSize.x, stepScale);
        transform.localScale = new Vector2(newSize, newSize);

        sr.color = Color.Lerp(currentColor, startColor, stepScale);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ChargeLight" && mouseControls.kill)
        {
            StartFilling();
        }

        if (collision.tag == "Light" || collision.tag == "PlayerAura" || collision.tag == "ChargeLight")
        {
            spriteHighlight.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!mouseControls.kill || collision.tag == "ChargeLight")
        {
            StopFilling();
        }

        if (collision.tag == "Light" || collision.tag == "PlayerAura" || collision.tag == "ChargeLight")
        {
            spriteHighlight.enabled = false;
        }
    }
}
