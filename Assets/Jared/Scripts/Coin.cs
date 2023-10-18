using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Coin : MonoBehaviour
{
    public CoinCanvas CC;
    public float CoinTimer = 0f;
    public float CoinLifetime = 0.25f;
    public bool PickUp = false;
    public AudioSource AudioSource;
    public AudioClip SFX;

    void Start()
    {
        CC = FindObjectOfType<CoinCanvas>();
        AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (PickUp)
        {
            CoinTimer += Time.deltaTime;

            if (CoinTimer >= CoinLifetime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger)
        {
            PickUp = true;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("Coin_PickUp");
            AudioSource.PlayOneShot(SFX);
            CC.CoinScore++;
        }
    }
}