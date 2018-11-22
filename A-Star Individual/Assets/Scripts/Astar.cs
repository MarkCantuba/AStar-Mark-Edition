using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {

    WorldGrid grid;     // Reference to our grid object.
    public bool visualizer = true;
    private LinkedList<Node> finalPath = new LinkedList<Node>();
    public float bias;  // Bias Value to improve our heuristic. In this case, it seems that the higher the bias value is, the less
                        // nodes are checked!
               
    
    void Start () {
        grid = GameObject.Find("Grid").GetComponent<WorldGrid>();
    }

    List<Node> closedSet = new List<Node>();
    NodeHeapArray openSet = new NodeHeapArray();


    /*
     * A* algorithm. Customized to use a heap!
     */ 
    IEnumerator AStar(Vector2 startPoint, Vector2 endPoint) {

        // A dictionary containing all the explored nodes so far. I used a dict so search time is constant!
        Dictionary<Node, bool> explored = new Dictionary<Node, bool>();
        // Heap array for our open set.
        openSet = new NodeHeapArray();
        if (visualizer)
        {
            closedSet = new List<Node>();
        }

        // Reinatialize all nodes in our grid done
        foreach (Node node in grid.gridNode) {
            node.gCost = Mathf.Infinity;
            node.hCost = Mathf.Infinity;
            node.parentNode = null;
            explored[node] = false;
        }

        // Player's grid position
        Vector2 playerGridPos = grid.WorldToGrid(startPoint);
        // Goal's grid Position
        Vector2 endPointGridPos = grid.WorldToGrid(endPoint);

        // Node corresponding to the player's grid position
        Node playerNode = grid.gridNode[(int)playerGridPos.x, (int)playerGridPos.y];

        playerNode.gCost = 0;
        playerNode.hCost = HeuristicCost(playerGridPos, endPointGridPos);
        openSet.Push(playerNode);
        
        while (openSet.Count() > 0) {
            yield return new WaitForEndOfFrame();
            Node current = openSet.Pop();
            explored[current] = true;
            if (visualizer)
            {
                closedSet.Add(current);
            }

            // check if we found our goal!
            if (current.point == endPointGridPos) {
                finalPath.Clear();  // Clear previous final path
                int iterationCount = 10000;     // Ensures that we don't get stuck in an infinite cycle!
                Node goal = grid.gridNode[(int)endPointGridPos.x, (int)endPointGridPos.y];
                while (goal.parentNode != null && iterationCount > 0) {
                    finalPath.AddLast(goal);
                    goal = goal.parentNode;
                    iterationCount--;
                }
                break;
            }

            // Check all neighbors
            foreach (Node node in grid.GetNeighbors(current)) {
                // New to be gCost. 
                // Addding a bias value to the gCost significantly reduces the number
                // of Nodes explored during search time!
                float tentativeGScore = current.gCost + HeuristicCost(node.point, current.point, bias);     
                // If the node has already been explored, or the gCost is higher htan the current node's gCost
                // There's no point checking this node!
                if (explored[node] || tentativeGScore >= node.gCost) {
                    continue;
                }
                if (!openSet.Contains(node)) {
                    openSet.Push(node);
                }
                // Update this node's information!
                int nodeIndex = openSet.nodeArray.IndexOf(node);
                node.parentNode = current;
                node.gCost = tentativeGScore;
                node.hCost = HeuristicCost(node.point, endPointGridPos);
                openSet.BottomTopHeapify(nodeIndex);    // re-heapify our open set!                
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(AStar(transform.position, GameObject.Find("GoalTile").transform.position));

    }

    // Heuristic cost which calculates the distance between a node and another node!
    float HeuristicCost(Vector2 nodePoint, Vector2 goalPoint, float biasValue = 1) {
        return Vector2.Distance(grid.GridToWorld(nodePoint, false), grid.GridToWorld(goalPoint, false)) * biasValue;
    }

    public void OnDrawGizmos() {

        if (visualizer)
        {
            if (closedSet.Count > 0)
            {
                foreach (Node node in closedSet)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(grid.GridToWorld(node.point, true), 0.2f);
                }
            }
            if (openSet.Count() > 0)
            {
                foreach (Node node in openSet.nodeArray)
                {
                    if (node != null)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(grid.GridToWorld(node.point, true), 0.2f);
                    }
                }
            }
        }

        if (finalPath != null) {
            foreach (Node node in finalPath) {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(grid.GridToWorld(node.point, true), 0.2f);
            }
        }
    }

}

