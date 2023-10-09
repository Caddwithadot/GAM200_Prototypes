using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFill : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private Animator anim;
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
        rb = transform.parent.GetComponent<Rigidbody2D>();
        bc = transform.parent.GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = transform.parent.GetComponent<Animator>();
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
            //rb.isKinematic = true;
            //bc.enabled = false;
            anim.SetTrigger("Pop");
            ps.Play();
            GetComponent<EnemyFill>().enabled = false;
            transform.parent.GetComponent<Rigidbody2D>().isKinematic = true;
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent.GetComponent<EnemyApproacher>().enabled = false;
            GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
    }
}
