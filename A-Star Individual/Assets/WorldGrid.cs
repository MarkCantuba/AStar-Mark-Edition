using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour {

    public Node[,] gridNode;
    int cellSize = 1;
    [Range(1, 1000)] public int gridX = 1;
    [Range(1, 1000)] public int gridY = 1;

    private void Start() {
        gridNode = new Node[gridX, gridY];
        for (int i = 0; i < gridX; i++) {
            for (int j = 0; j < gridY; j++) {
                gridNode[i, j] = new Node(i, j, 0, 0);
            }
        }
    }

    private void OnDrawGizmos() {

        Gizmos.DrawWireCube(transform.position, new Vector2(gridX, gridY));
        Vector2 cell = new Vector2(cellSize, cellSize);
        Vector2 bottomEdge = (Vector2) transform.position + (cell + Vector2.down * gridY) / 2;
        Vector2 leftEdge = (Vector2.left * gridX) / 2;
        Vector2 leftCorner = (bottomEdge + leftEdge);

        for (int i = 0; i < gridX; i++) {
            for (int j = 0; j < gridY; j++) {
                Vector2 pos = new Vector2(i, j);
                Gizmos.DrawWireCube(leftCorner + pos, cell);
            }
        }
    }
}
