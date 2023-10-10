using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFill : MonoBehaviour
{
    public GameObject enemy;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Animator anim;
    private SpriteRenderer sr;
    private ParticleSystem ps;

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
        rb = enemy.GetComponent<Rigidbody2D>();
        bc = enemy.GetComponent<BoxCollider2D>();
        anim = enemy.GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();
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
        bc.size = transform.localScale;
        bakeAmount = transform.localScale.x - startSize.x;

        if(bakeAmount >= 0.75)
        {
            anim.SetTrigger("Pop");
            ps.Play();
            GetComponent<EnemyFill>().enabled = false;
            transform.parent.GetComponent<Rigidbody2D>().isKinematic = true;
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent.GetComponent<EnemyPatrol>().enabled = false;
            transform.parent.GetComponent<EnemyChase>().enabled = false;
            GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
    }
}