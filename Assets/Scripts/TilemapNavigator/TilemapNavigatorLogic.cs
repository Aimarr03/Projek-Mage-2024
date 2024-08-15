using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TilemapNavigator
{
    public class TilemapNavigatorLogic : MonoBehaviour
    {
        public Tilemap tilemap;

        public List<TileNodeNav> OpenList;
        public List<TileNodeNav> CloseList;
        private Dictionary<Vector3Int, TileNodeNav> allTiles;

        public void Initialized()
        {
            OpenList = new List<TileNodeNav>();
            CloseList = new List<TileNodeNav>();
            allTiles = new Dictionary<Vector3Int, TileNodeNav>();
            /*Debug.Log(tilemap.cellBounds);
            Debug.Log(tilemap.cellBounds.allPositionsWithin);*/
            BoundsInt boundaryTilemap = tilemap.cellBounds;
            for(int X_Index = boundaryTilemap.xMin; X_Index < boundaryTilemap.xMax; X_Index++)
            {
                for(int Y_Index = boundaryTilemap.yMin; Y_Index < boundaryTilemap.yMax; Y_Index++)
                {
                    Vector3Int gridPosition = new Vector3Int(X_Index, Y_Index, 0);
                    TileBase currentTile = tilemap.GetTile(gridPosition);
                    if (currentTile == null) continue;
                    TileNodeNav newTile = new TileNodeNav(gridPosition, true);
                    allTiles.Add(gridPosition,newTile);
                }
            }
            foreach(TileNodeNav currentTile in allTiles.Values)
            {
                Debug.Log("Current Tile in " + currentTile.gridPosition);
            }
        }
        void Start()
        {
            Initialized();
        }
        void Update()
        {

        }
    }
}

