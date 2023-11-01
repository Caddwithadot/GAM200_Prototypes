using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public GameObject LightTilePrefab;
    public Transform TileParent;

    private Tilemap tilemap; // Assign your Tilemap in the Inspector.
    public Vector3 bottomLeft; // Define these positions or assign them in the Inspector.
    public Vector3 topRight;   // Define these positions or assign them in the Inspector.

    public Vector3 tileOffset = new Vector3(0.25f, 0.25f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        GridLayout grid = tilemap.layoutGrid;
        Vector3Int bottomLeftCell = grid.WorldToCell(bottomLeft);
        Vector3Int topRightCell = grid.WorldToCell(topRight);

        Vector3Int min = Vector3Int.Min(bottomLeftCell, topRightCell);
        Vector3Int max = Vector3Int.Max(bottomLeftCell, topRightCell);
        Vector3Int size = max - min + Vector3Int.one;

        BoundsInt bounds = new BoundsInt(min, size);

        TileBase[] tiles = tilemap.GetTilesBlock(bounds);

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);
            if (tile != null)
            {
                Vector3 worldPosition = grid.CellToWorld(position) + tileOffset;

                Instantiate(LightTilePrefab, worldPosition, Quaternion.identity, TileParent);
            }
        }
    }
}
