using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightTile : MonoBehaviour
{
    private SpriteRenderer tile;
    private List<SpriteRenderer> tiles = new List<SpriteRenderer>();
    private Vector2 overlapSize = new Vector2(1.0f, 1.0f);
    public LayerMask tileLayer;

    public float lightAlpha = 0f;
    public float darkAlpha = 1f;

    private float lightTime = 0f;
    public float lightDelay = 0.5f;

    private bool checkLight = false;

    private void Start()
    {
        tile = GetComponent<SpriteRenderer>();
        tiles.Add(tile);
    }

    private void Update()
    {
        if (checkLight)
        {
            lightTime -= Time.deltaTime;

            if(lightTime <= 0)
            {
                Dark();
            }
        }
    }

    public void Light()
    {
        tile.color = new Color(0, 0, 0, lightAlpha);
    }

    public void Dark()
    {
        tile.color = new Color(0, 0, 0, darkAlpha);

        checkLight = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Light" || collision.tag == "ChargeLight" || collision.tag == "PlayerAura")
        {
            lightTime = lightDelay;
            Light();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Light" || collision.tag == "ChargeLight" || collision.tag == "PlayerAura")
        {
            checkLight = true;
        }
    }
}
