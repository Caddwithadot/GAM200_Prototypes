using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public GameObject playerParent;
    private AudioSource audioSource;
    public AudioClip death;

    private bool isDead = false;
    private float deathTimer = 0f;
    public float deathDelay = 2f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isDead)
        {
            deathTimer -= Time.deltaTime;

            if (deathTimer <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void PlayerDeathReload()
    {
        playerParent.SetActive(false);
        audioSource.PlayOneShot(death, 3);

        deathTimer = deathDelay;
        isDead = true;
    }
}
