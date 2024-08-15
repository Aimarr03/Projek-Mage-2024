using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapNavigation : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Current Mouse Position " + mousePosition);
            Vector3Int tilePositionBasedOnMouse = tilemap.WorldToCell(mousePosition);
            
            Debug.Log("Tile Position from Grid Based on Mouse Position: " + tilePositionBasedOnMouse);
            TileBase tile =  tilemap.GetTile(tilePositionBasedOnMouse);
            Debug.Log(tile);
        }
    }
}
