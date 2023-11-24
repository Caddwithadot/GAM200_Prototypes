using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLightTiles : MonoBehaviour
{
    public Sprite[] tileSprite;
    public GameObject lightTilePrefab;
    public Transform lightTileParent;

    void Start()
    {
        if(lightTilePrefab == null)
        {
            lightTileParent = this.transform;
        }

        SpriteRenderer[] allSprites = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer sprites in allSprites)
        {
            foreach (Sprite arraySprite in tileSprite)
            {
                if (sprites.sprite == arraySprite)
                {
                    Instantiate(lightTilePrefab, sprites.transform.position, sprites.transform.rotation, this.transform);
                }
            }
        }
    }
}
