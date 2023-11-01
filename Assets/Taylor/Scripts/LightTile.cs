using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightTile : MonoBehaviour
{
    private List<SpriteRenderer> tiles = new List<SpriteRenderer>();
    private Vector2 overlapSize = new Vector2(1.0f, 1.0f);
    public LayerMask tileLayer;

    private void Start()
    {
        tiles.Add(GetComponent<SpriteRenderer>());

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
            foreach(SpriteRenderer renderer in tiles)
            {
                renderer.color = new Color(0, 0, 0, 0f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Light" || collision.tag == "ChargeLight" || collision.tag == "PlayerAura")
        {
            foreach (SpriteRenderer renderer in tiles)
            {
                renderer.color = new Color(0, 0, 0, 0.5f);
            }
        }
    }
}
