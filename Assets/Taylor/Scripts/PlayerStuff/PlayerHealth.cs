using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;

    public GameObject flashLight;
    public GameObject playerAura;

    public int health = 4;
    private int maxHealth;

    private float invTimer = 0f;
    public float invFrameCooldown = 3f;

    private float deathTimer = 0f;
    public float deathDelay = 1f;
    private bool isDead = false;

    private bool isHealing = false;
    private float healTimer = 0f;
    public float passHealDelay = 5f;
    public float lampHealDelay = 0.5f;

    private void Start()
    {
        maxHealth = health;
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

        // lamp healing
        if (isHealing && health < maxHealth)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= lampHealDelay)
            {
                health++;

                healTimer = 0f;
            }
        }

        // passive healing
        if(health < maxHealth - 1)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= passHealDelay)
            {
                health++;

                healTimer = 0f;
            }
        }

        // scales aura based on health
        if (health == maxHealth)
        {
            playerAura.transform.localScale = new Vector3(7.5f, 7.5f, 1);
            sr.color = new Color(1, 1, 1);
        }
        else if (health == 3)
        {
            playerAura.transform.localScale = new Vector3(6f, 6f, 1);
            sr.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else if (health == 2)
        {
            playerAura.transform.localScale = new Vector3(4.5f, 4.5f, 1);
            sr.color = new Color(0.6f, 0.6f, 0.6f);
        }
        else if (health == 1)
        {
            playerAura.transform.localScale = new Vector3(2.25f, 2.25f, 1);
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
        if (collision.CompareTag("Aura"))
        {
            isHealing = true;
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
        if (collision.CompareTag("Aura"))
        {
            isHealing = false;
            healTimer = 0f;
        }
    }
}