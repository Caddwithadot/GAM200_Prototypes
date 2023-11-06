using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;

    public RayAura rayAura;

    public int health = 4;
    private int maxHealth;

    private float invTimer = 0f;
    public float invFrameCooldown = 3f;

    private bool isHealing = false;
    private float healTimer = 0f;
    public float passHealDelay = 5f;
    public float lampHealDelay = 0.5f;

    public float maxAuraScale = 6.0f;

    private AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip healSound;

    public SceneHandler sceneHandler;

    private void Start()
    {
        maxHealth = health;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(invTimer > 0)
        {
            invTimer -= Time.deltaTime;

            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
        }

        // lamp healing
        if (isHealing && health < maxHealth)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= lampHealDelay)
            {
                audioSource.PlayOneShot(healSound, 0.25f);
                health++;
                healTimer = 0f;
            }
        }

        float auraDifference = maxAuraScale / maxHealth;
        float auraScale = health * auraDifference;

        // Update rayAura and playerAura
        rayAura.SetOrigin(transform.position);

        if (health != 1)
        {
            sr.color = new Color((auraScale - 0.15f) / maxAuraScale, (auraScale - 0.15f) / maxAuraScale, (auraScale - 0.15f) / maxAuraScale);
            rayAura.SetViewDistance((auraScale + 1.5f) / 3);
        }
        else
        {
            sr.color = Color.black;
            rayAura.SetViewDistance(1f);
        }
    }

    public void TakeDamage(int lostHealth)
    {
        invTimer = invFrameCooldown;
        health -= lostHealth;

        if (health <= 0)
        {
            sceneHandler.PlayerDeathReload();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Aura"))
        {
            isHealing = true;
        }

        if (collision.gameObject.tag == ("Enemy") && invTimer <= 0)
        {
            audioSource.PlayOneShot(hitSound, 3f);
            TakeDamage(1);
            healTimer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Enemy") && invTimer <= 0)
        {
            audioSource.PlayOneShot(hitSound, 3f);
            TakeDamage(1);
            healTimer = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Aura"))
        {
            isHealing = false;
            healTimer = 0f;
        }
    }
}