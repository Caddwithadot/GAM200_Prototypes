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

    private void Start()
    {
        tile = GetComponent<SpriteRenderer>();
        tiles.Add(tile);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, overlapSize, 0.0f, tileLayer);

        foreach(Collider2D collider in colliders)
        {
            if(collider.gameObject != null && collider.gameObject.tag == "Tile")
            {
                tiles.Add(collider.GetComponent<SpriteRenderer>());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Light" || collision.tag == "ChargeLight" || collision.tag == "PlayerAura")
        {
            tile.color = new Color(0, 0, 0, lightAlpha);
            /*
            foreach(SpriteRenderer renderer in tiles)
            {
                renderer.color = lightColor;
            }
            */
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Light" || collision.tag == "ChargeLight" || collision.tag == "PlayerAura")
        {
            tile.color = new Color(0, 0, 0, darkAlpha);
            /*
            foreach (SpriteRenderer renderer in tiles)
            {
                renderer.color = darkColor;
            }
            */
        }
    }
}
