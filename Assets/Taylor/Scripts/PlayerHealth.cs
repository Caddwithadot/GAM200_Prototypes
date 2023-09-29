using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject flashLight;
    public GameObject playerAura;
    private Vector3 initialScale;

    public int health = 4;
    private Animator animator;

    private float invTimer = 0f;
    public float invFrameCooldown = 3f;

    private float deathTimer = 0f;
    public float deathDelay = 1f;
    private bool isDead = false;

    private void Start()
    {
        initialScale = playerAura.transform.localScale;
        animator = GetComponent<Animator>();
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && invTimer <= 0)
        {
            health--;

            invTimer = invFrameCooldown;

            if (health <= 0)
            {
                playerAura.transform.localScale = Vector3.zero;

                GetComponent<ScriptMachine>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                flashLight.gameObject.SetActive(false);
                Time.timeScale = 0.25f;
                isDead = true;
            }
            else
            {
                if(health == 3)
                {
                    playerAura.transform.localScale = new Vector3(12, 12, 1);
                }
                else if(health == 2)
                {
                    playerAura.transform.localScale = new Vector3(9, 9, 1);
                }
                else
                {
                    playerAura.transform.localScale = new Vector3(4.5f, 4.5f, 1);
                }
            }
        }
    }
}