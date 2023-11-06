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
    public SpriteRenderer sr;
    private ParticleSystem ps;
    private Animator anim;

    private MonoBehaviour[] scriptsOnEnemy;

    public float fillSpeed = 0.25f;
    public float revertSpeed = 1f;

    private bool startFill = false;
    private bool startRevert = false;

    private Color startColor;
    private Color currentColor;
    public Color targetColor;

    private Vector2 startSize;
    private Vector2 currentSize;
    public Vector2 targetSize;

    private bool filled = false;

    private AudioSource audioSource;
    public AudioClip burningSound;
    public AudioClip deadSound;

    private void Start()
    {
        scriptsOnEnemy = enemy.GetComponents<MonoBehaviour>();
        mouseControls = GameObject.Find("MouseControls").GetComponent<MouseControls>();

        rb = enemy.GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        startColor = sr.color;
        startSize = transform.localScale;
    }

    private void Update()
    {
        currentColor = sr.color;
        currentSize = transform.localScale;

        if (startFill && !filled)
        {
            Fill();
        }

        if (startRevert && currentColor != startColor && !filled)
        {
            Revert();
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
        if (!ps.isPlaying)
        {
            ps.Play();
        }

        #region Fill lerp
        float journeyLengthScale = Mathf.Abs(targetSize.x - currentSize.x);
        float stepScale = fillSpeed / journeyLengthScale * Time.deltaTime;

        float newSize = Mathf.Lerp(currentSize.x, targetSize.x, stepScale);
        transform.localScale = new Vector2(newSize, newSize);

        sr.color = Color.Lerp(currentColor, targetColor, stepScale);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(burningSound, 0.5f);
        }
        #endregion

        if (currentColor == targetColor)
        {
            ps.Stop();
            anim.enabled = true;
            rb.gravityScale = 0.5f;
            GetComponent<BoxCollider2D>().enabled = false;
            playerTrigger.GetComponent<BoxCollider2D>().enabled = false;

            audioSource.PlayOneShot(deadSound, 0.25f);

            foreach (var script in scriptsOnEnemy)
            {
                script.enabled = false;
            }

            filled = true;
            GetComponent<EnemyFill>().enabled = false;
        }
    }

    public void Revert()
    {
        #region Revert lerp
        float journeyLengthScale = Mathf.Abs(startSize.x - currentSize.x);
        float stepScale = revertSpeed / journeyLengthScale * Time.deltaTime;

        float newSize = Mathf.Lerp(currentSize.x, startSize.x, stepScale);
        transform.localScale = new Vector2(newSize, newSize);

        sr.color = Color.Lerp(currentColor, startColor, stepScale);
        #endregion

        if (ps.isPlaying)
        {
            ps.Stop();
        }
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
