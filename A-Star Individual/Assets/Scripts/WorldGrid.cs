using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour {

    public Node[,] gridNode;    // A 2D Array representation of our grid world
    int cellSize = 1;
    [Range(1, 1000)] public int gridX = 1;
    [Range(1, 1000)] public int gridY = 1;
    LayerMask wallMask;
    public bool drawGrid = false;
    public GameObject wall;
    LayerMask goal;

    /*
     * Add a button that generates a randomly generated map (by map i mean place random
     * walls within grid xD).
     */ 
    private void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 50, 20), "CreateNewGrid")) {
            foreach (Transform wall in GameObject.Find("Walls").transform) {
                Destroy(wall.gameObject);
            }
            gridNode = new Node[gridX, gridY];
            AddRandomWalls();
            wallMask = LayerMask.GetMask(new string[] { "wall"});           
            gridNode = new Node[gridX, gridY];
            for (int i = 0; i < gridX; i++) {
                for (int j = 0; j < gridY; j++) {
                    Vector2 gridPos = new Vector2(i, j);
                    bool wallDetected = Physics2D.OverlapCircle(GridToWorld(gridPos + GetLeftCorner(), false), cellSize / 2, wallMask);
                    gridNode[i, j] = new Node(i, j, !wallDetected, Mathf.Infinity, Mathf.Infinity);
                }
            }
        }
    }

    /*
     * Initialize our grid nodes. It creates a grid of tile sprites (also viewable in game view!) 
     * with their corresponding coordinates attached as text mesh on them!
     */
    private void Awake() {
        gridNode = new Node[gridX, gridY];
        AddRandomWalls();
        wallMask = LayerMask.GetMask(new string[] { "wall" });
        gridNode = new Node[gridX, gridY];
        for (int i = 0; i < gridX; i++) {
            for (int j = 0; j < gridY; j++) {
                Vector2 gridPos = new Vector2(i, j);
                bool wallDetected = Physics2D.OverlapCircle(GridToWorld(gridPos + GetLeftCorner(), false), cellSize / 2, wallMask);
                gridNode[i, j] = new Node(i, j, !wallDetected, Mathf.Infinity, Mathf.Infinity);
            }
        }
    }

    /*
     * Add walls to random coordinates in our grid. Good for testing!
     */ 
    void AddRandomWalls() {
        for (int i = 0; i < gridX; i++) {
            for (int j = 0; j < gridY; j++) {
                int randomNumber = Random.Range(-2, 2);
                Vector2 gridPos = new Vector2(i, j);
                goal = LayerMask.GetMask(new string[] { "goal" , "player"});
                bool wallDetected = Physics2D.OverlapCircle(GridToWorld(gridPos, false) + GetLeftCorner(), cellSize, goal);
                if (randomNumber == 1) { 
                    if (wallDetected) {
                        continue;
                    } else {
                        GameObject unwalkable = Instantiate(wall, GridToWorld(new Vector2(i, j), false) + GetLeftCorner(), Quaternion.identity);
                        unwalkable.transform.parent = GameObject.Find("Walls").transform;
                    }
                }
            }   
        }
    }



    /*
     * Get given node's neighbors. Taken from the tutorial on path finding and modified,
     * so it works with my implementation. It always consider diagonal moves.
     * Also, it only consider within bounds nodes and excludes walls!
     */ 
    public List<Node> GetNeighbors(Node node) {

        int x = (int)node.point.x;
        int y = (int)node.point.y;
        List<Node> neighbors = new List<Node>();

        bool N, E, W, S;

        N = (y < gridY - 1);
        S = (y > 0);
        E = (x < gridX - 1);
        W = (x > 0);
        if (N && gridNode[x, y + 1].walkable) { 
            neighbors.Add(gridNode[x, y + 1]);
        }
        if (S && gridNode[x, y - 1].walkable) {
            neighbors.Add(gridNode[x, y - 1]);
        }
        if (E && gridNode[x + 1, y].walkable) {
            neighbors.Add(gridNode[x + 1, y]);
        }
        if (W && gridNode[x - 1, y].walkable) {
            neighbors.Add(gridNode[x - 1, y]);
        }
        if (N && E && gridNode[x + 1, y + 1].walkable) { 
            neighbors.Add(gridNode[x + 1, y + 1]);
        }
        if (N && W && gridNode[x - 1, y + 1].walkable) {
            neighbors.Add(gridNode[x - 1, y + 1]);
        }
        if (S && E && gridNode[x + 1, y - 1].walkable) {
            neighbors.Add(gridNode[x + 1, y - 1]);
        }
        if (S && W && gridNode[x - 1, y - 1].walkable) {
            neighbors.Add(gridNode[x - 1, y - 1]);
        }
        
        return neighbors;
    }

    /*
     * Converts world Position to grid position. This is taken from the grid class 
     * from the pathfinding tutorial! Modified so it works for my grid version!
     */
    public Vector2 WorldToGrid(Vector2 worldPos) {
        int gridPosX = Mathf.FloorToInt((worldPos.x - transform.position.x) / cellSize);
        int gridPosY = Mathf.FloorToInt((worldPos.y - transform.position.y) / cellSize);

        Vector2 leftCorner = GetLeftCorner();

        int leftCornerX = Mathf.FloorToInt((leftCorner.x - transform.position.x) / cellSize);
        int leftCornerY = Mathf.FloorToInt((leftCorner.y - transform.position.y) / cellSize);
        return new Vector2(gridPosX, gridPosY) - new Vector2(leftCornerX, leftCornerY);
    }


    /*
     * Convert Grid to world position. Taken from the grid class from pathfinding tutorial, with
     * Slight modification to offSet Value!
     */
    public Vector2 GridToWorld(Vector2 gridPos, bool centered) {
        if (!centered) {
            float worldPosX = transform.position.x + gridPos.x * cellSize;
            float worldPosY = transform.position.y + gridPos.y * cellSize;
            return new Vector2(worldPosX, worldPosY);
        }

        float worldPosA = transform.position.x + gridPos.x * cellSize;
        float worldPosB = transform.position.y + gridPos.y * cellSize;
        return new Vector2(worldPosA, worldPosB) + GetLeftCorner();
    }

    /*
     * Get the starting point of our grid, which is the bottom left corner of our grid, centered
     * where WireFrameCube is drawn.
     */ 
    public Vector2 GetLeftCorner() {
        Vector2 cell = new Vector2(cellSize, cellSize); // a cell of size 1x1
        
        // Get bottom edge coordinate.
        Vector2 bottomEdge = (Vector2)transform.position + (cell + Vector2.down * gridY) / 2;
        Vector2 leftEdge = (Vector2.left * gridX) / 2;
        Vector2 leftCorner = (bottomEdge + leftEdge);
        return leftCorner;
    }

    /*
     * Check if the object is within bounds of our grid.
     * returns false if out of bounds
     */ 
    public bool IsInBounds(Vector2 position) {
        Vector2 gridPoint = WorldToGrid(position);
        return (gridPoint.x < gridX) && (gridPoint.x >= 0) && (gridPoint.y < gridY) && (gridPoint.y >= 0);
    }


    /*
     * Good for Debugging! Draw our grid
     */ 
    private void OnDrawGizmos() {
        if (drawGrid) {
            Gizmos.DrawWireCube(transform.position, new Vector2(gridX, gridY));  // Outer Grid Cell 
            Vector2 cell = new Vector2(cellSize, cellSize); // Cell Size of 1x1
            Vector2 leftCorner = GetLeftCorner();   // Left Corner of our grid (0, 0)

            // Draw grid cells inside rectangle
            for (int i = 0; i < gridX; i++) {
                for (int j = 0; j < gridY; j++) {
                    Vector2 pos = new Vector2(i, j);
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(leftCorner + pos, cell);  // Draw wire cube every cellSize increment.
                    if (gridNode != null) {
                        if (!gridNode[i, j].walkable) {
                            Gizmos.color = Color.red;
                            Gizmos.DrawCube(leftCorner + pos, cell / 2);
                        }
                    }
                }
            }
        }
    }
}
