using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour {

    public Node[,] gridNode;
    int cellSize = 1;
    [Range(1, 1000)] public int gridX = 1;
    [Range(1, 1000)] public int gridY = 1;
    public LayerMask wallMask;

    /*
     * Initialize our grid nodes. This handles 
     */ 
    private void Awake() {
        wallMask = LayerMask.GetMask(new string[] { "wall" });
        gridNode = new Node[gridX, gridY];
        for (int i = 0; i < gridX; i++) {
            for (int j = 0; j < gridY; j++) {
                Vector2 gridPos = new Vector2(i, j);
                bool wallDetected = Physics2D.OverlapCircle(GridToWorld(gridPos + getLeftCorner(), true), cellSize/2, wallMask);
                gridNode[i, j] = new Node(i, j, 0, 0, !wallDetected);
            }
        }
    }


    /*
     * Converts world Position to grid position. This is taken from the grid class 
     * from the pathfinding tutorial!
     */
    public Vector2 WorldToGrid(Vector2 worldPos) {
        int gridPosX = Mathf.FloorToInt((worldPos.x - transform.position.x) / cellSize);
        int gridPosY = Mathf.FloorToInt((worldPos.y - transform.position.y) / cellSize);
        return new Vector2(gridPosX, gridPosY);
    }


    /*
     * Convert Grid to world position. Taken from the grid class from pathfinding tutorial, with
     * Slight modification to offSet Value!
     */
    public Vector2 GridToWorld(Vector2 gridPos, bool centered) {
        float offSetVal = centered ? 0 : cellSize / 2.0f;
        float worldPosX = transform.position.x + gridPos.x * cellSize + offSetVal;
        float worldPosY = transform.position.y + gridPos.y * cellSize + offSetVal;
        return new Vector2(worldPosX, worldPosY);
    }

    Vector2 getLeftCorner() {
        Vector2 cell = new Vector2(cellSize, cellSize);
        Vector2 bottomEdge = (Vector2)transform.position + (cell + Vector2.down * gridY) / 2;
        Vector2 leftEdge = (Vector2.left * gridX) / 2;
        Vector2 leftCorner = (bottomEdge + leftEdge);
        return leftCorner;
    }

    private void OnDrawGizmos() {

        Gizmos.DrawWireCube(transform.position, new Vector2(gridX, gridY));  // Outer Grid Cell 
        Vector2 cell = new Vector2(cellSize, cellSize); // Cell Size of 1x1
        Vector2 leftCorner = getLeftCorner();   // Left Corner of our grid (0, 0)

        // Draw grid cells inside rectangle
        for (int i = 0; i < gridX; i++) {
            for (int j = 0; j < gridY; j++) {
                Vector2 pos = new Vector2(i, j);   
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(leftCorner + pos, cell);  // Draw wire cube every cellSize increment.
            }
        }
        if (gridNode != null) {
            foreach (Node node in gridNode) {
                Vector2 pos = new Vector2(node.point.x, node.point.y);
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(leftCorner + pos, cell);


                Gizmos.color = node.walkable ? Color.green : Color.red;
                Gizmos.DrawCube(leftCorner + pos, cell / 2);
            }
        }
    }
}
