using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject flashLight;
    public GameObject playerAura;

    public int health = 4;
    private Animator animator;
    private SpriteRenderer sr;

    private float invTimer = 0f;
    public float invFrameCooldown = 3f;

    private float deathTimer = 0f;
    public float deathDelay = 1f;
    private bool isDead = false;

    private bool isHealing = false;
    private float healTimer = 0f;
    public float healDelay = 2f;

    public bool flashlightRegenerate = false;
    public bool passiveHealing = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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

        if (isDead)
        {
            deathTimer += Time.deltaTime;

            if (deathTimer >= deathDelay)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (isHealing && health < 4 || passiveHealing && health < 4)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= healDelay)
            {
                health++;

                healTimer = 0f;
            }
        }

        if (health == 4)
        {
            playerAura.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            sr.color = new Color(1, 1, 1);
        }
        else if (health == 3)
        {
            playerAura.transform.localScale = new Vector3(1.2f, 1.2f, 1);
            sr.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else if (health == 2)
        {
            playerAura.transform.localScale = new Vector3(.9f, .9f, 1);
            sr.color = new Color(0.6f, 0.6f, 0.6f);
        }
        else if (health == 1)
        {
            playerAura.transform.localScale = new Vector3(.45f, .45f, 1);
            sr.color = new Color(0.2f, 0.2f, 0.2f);
        }
    }

    public void TakeDamage(int lostHealth)
    {
        health -= lostHealth;

        invTimer = invFrameCooldown;

        if (health <= 0)
        {
            playerAura.transform.localScale = Vector3.zero;
            sr.color = new Color(0, 0, 0);

            Time.timeScale = 0.25f;
            isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Aura") && !passiveHealing)
        {
            isHealing = true;

            if (flashlightRegenerate)
            {
                flashLight.GetComponent<FlashlightEnergy>().passiveRegenerate = true;
            }
        }

        if (collision.gameObject.tag == ("Enemy") && invTimer <= 0)
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Enemy") && invTimer <= 0)
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Aura") && !passiveHealing)
        {
            isHealing = false;
            healTimer = 0f;

            if (flashlightRegenerate)
            {
                flashLight.GetComponent<FlashlightEnergy>().passiveRegenerate = false;
            }
        }
    }
}