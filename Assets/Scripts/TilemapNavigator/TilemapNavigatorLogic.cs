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
        private List<TileNodeNav> currentPath;
        [SerializeField] private Vector3 testStartPos = new Vector3(7.63f, 2.54f, 0);
        [SerializeField] private Vector3 testEndPos = new Vector3(-8.10f, 2.92f, 0);
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
                    newTile.worldPosition = tilemap.GetCellCenterLocal(gridPosition);
                    allTiles.Add(gridPosition,newTile);
                }
            }
            foreach (TileNodeNav currentTile in allTiles.Values)
            {
                Debug.Log("Current Tile in " + currentTile.gridPosition);
            }
        }
        public List<TileNodeNav> GetPathFromWorldPosition(Vector3 startWorldPos,  Vector3 endWorldPos)
        {
            Vector3Int startPos = tilemap.WorldToCell(startWorldPos);
            Vector3Int endPos = tilemap.WorldToCell(endWorldPos);

            TileNodeNav startNode = allTiles[startPos];
            TileNodeNav endNode = allTiles[endPos];

            if(startNode == null || endNode == null) return null;

            OpenList.Add(startNode);
            while(OpenList.Count > 0)
            {
                TileNodeNav currentNode = OpenList[0];
                for(int index = 1; index < OpenList.Count; index++)
                {
                    TileNodeNav newNode = OpenList[index];
                    if (newNode.FCost <= currentNode.FCost && newNode.HCost < currentNode.HCost)
                    {
                        currentNode = newNode;
                    }
                }

                OpenList.Remove(currentNode);
                CloseList.Add(currentNode);

                if(currentNode == endNode)
                {
                    return RetracePath(startNode, endNode);
                }

                foreach(var neighbor in GetNeighbours(currentNode))
                {
                    if(!neighbor.walkable || CloseList.Contains(neighbor)) continue;
                    int newDistanceFromCurrentNodeToNeighbor = currentNode.GCost + GetDistanceBetweenNode(currentNode, neighbor);
                    if(newDistanceFromCurrentNodeToNeighbor < neighbor.GCost || !OpenList.Contains(neighbor))
                    {
                        neighbor.GCost = newDistanceFromCurrentNodeToNeighbor;
                        neighbor.HCost = GetDistanceBetweenNode(neighbor, endNode);
                        neighbor.previousNode = currentNode;
                        if (!OpenList.Contains(neighbor))
                        {
                            OpenList.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }
        private List<TileNodeNav> GetNeighbours(TileNodeNav currentNode)
        {
            List<TileNodeNav> neighbours = new List<TileNodeNav>();
            Vector3Int currentNodePos = currentNode.gridPosition;
            
            for(int Index_X = -1; Index_X <=1; Index_X++)
            {
                for(int Index_Y = -1; Index_Y <= 1; Index_Y++)
                {
                    Vector3Int offSet =  new Vector3Int(Index_X, Index_Y, 0);
                    Vector3Int neighborPos = currentNodePos + offSet;
                    
                    TileNodeNav newNeighbourNode;

                    if (allTiles.TryGetValue(neighborPos, out newNeighbourNode))
                    {
                        neighbours.Add(newNeighbourNode);
                    }
                    
                }
            }

            return neighbours;

        }
        private int GetDistanceBetweenNode(TileNodeNav startNode, TileNodeNav endNode)
        {
            Vector3Int startNodePos = startNode.gridPosition;
            Vector3Int endNodePos = endNode.gridPosition;
            int Dis_X = Mathf.Abs(startNodePos.x - endNodePos.x);
            int Dis_Y = Mathf.Abs(startNodePos.y - endNodePos.y);
            if(Dis_X > Dis_Y) return Dis_Y * 14 + Dis_X * 10;
            else return Dis_X * 14 + Dis_Y * 10;
        }
        private List<TileNodeNav> RetracePath(TileNodeNav startNode, TileNodeNav endNode)
        {
            List<TileNodeNav> path = new List<TileNodeNav>();
            TileNodeNav currentNode = endNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.previousNode;
                //Debug.Log(currentNode.gridPosition);
            }
            path.Reverse();
            return path;
        }
        void Start()
        {
            Initialized();
            currentPath = GetPathFromWorldPosition(testStartPos, testEndPos);
        }
        private void OnDrawGizmos()
        {
            if(currentPath != null)
            {
                Gizmos.color = Color.red;
                for(int index = 0; index < currentPath.Count -1; index++)
                {
                    TileNodeNav currentNode = currentPath[index];
                    TileNodeNav nextNode = currentPath[index + 1];
                    if (nextNode == null) break;
                    Gizmos.DrawLine(currentNode.worldPosition, nextNode.worldPosition);
                }
                
            }
        }
        void Update()
        {

        }
    }
}

