using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public CoinCanvas CC;
    public AudioSource AudioSource;
    public AudioClip SFX;
    public Sprite ClosedSprite;
    public Sprite OpenSprite;
    private const float Score = 5f;

    public AudioSource LittleGuyAS;
    public AudioClip LittleGuySFX;

    void Start()
    {
        CC = FindObjectOfType<CoinCanvas>();
        AudioSource = GetComponent<AudioSource>();
        GetComponent<SpriteRenderer>().sprite = ClosedSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = OpenSprite;
            GetComponent<Animator>().SetTrigger("Chest_PickUp");
            LittleGuyAS.PlayOneShot(LittleGuySFX, 0.15f);
        }

        AudioSource.PlayOneShot(SFX);

        CC.CoinScore += 5f;
        CC.ChestCharlesPanel.GetComponent<Animator>().SetTrigger("ChestUI");
    }
}
