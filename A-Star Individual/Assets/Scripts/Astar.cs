using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {

    WorldGrid grid;
    private LinkedList<Node> finalPath = new LinkedList<Node>();
    public float bias;  // Bias Value to improve our heuristic. In this case, it seems that the higher the bias value is, the less
                        // nodes are checked!
    


    void Start () {
        grid = GameObject.Find("Grid").GetComponent<WorldGrid>();
    }

    NodeHeapArray openSet = new NodeHeapArray();
    List<Node> closedSet = new List<Node>();

    void AStar(Vector2 startPoint, Vector2 endPoint) {

        Vector2 playerGridPos = grid.WorldToGrid(startPoint);
        Vector2 endPointGridPos = grid.WorldToGrid(endPoint);

        Node playerNode = grid.gridNode[(int)playerGridPos.x, (int)playerGridPos.y];

        playerNode.gCost = 0;
        playerNode.hCost = HeuristicCost(playerGridPos, endPointGridPos, bias);

        closedSet = new List<Node>();
        openSet = new NodeHeapArray();

        openSet.Push(playerNode);
        
        while (openSet.Count() > 0) {
            Node current = openSet.Pop();
            print(current.gCost);
            closedSet.Add(current);

            if (current.point == endPointGridPos) {
                Node potato = current;
                while (potato.parentNode != null) {
                    finalPath.AddLast(potato);
                    potato = potato.parentNode;
                }

                return;
            }

            //print(openSet + " <<< ");

            foreach (Node node in grid.GetNeighbors(current)) {
                if (!ClosedContains(node)) {
                    float tempGScore = current.gCost + HeuristicCost(current.point, node.point);
                    if (!openSet.Contains(node)) {
                        openSet.Push(node);
                    } else if (tempGScore >= current.gCost) {
                        continue;
                    }
                    if (node.parentNode == null)
                        node.parentNode = current;
                    node.gCost = node.parentNode.gCost + HeuristicCost(current.point, node.point);
                    node.hCost = HeuristicCost(node.point, endPointGridPos, bias);

                    
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            AStar(transform.position, GameObject.Find("GoalTile").transform.position);

	}

    float HeuristicCost(Vector2 nodePoint, Vector2 goalPoint, float biasValue=1) {
        return Mathf.RoundToInt((Vector2.Distance(nodePoint, goalPoint))) * biasValue;
    }

    public bool ClosedContains(Node node) {
        foreach (Node node1 in closedSet) {
            if (node1.point == node.point) {
                return true;
            }
        }
        return false;
    }

    public void OnDrawGizmos() {
        if (openSet.Count() > 0) {
            foreach (Node node in openSet.nodeArray) {
                if (node == null) continue;
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(grid.GridToWorld(node.point, true), 0.2f);
            }
        }

        if (closedSet.Count > 0) {
            foreach (Node node in closedSet) {
                if (node == null) continue;
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(grid.GridToWorld(node.point, true), 0.2f);
            }
        }

        if (finalPath!= null) {
            foreach (Node node in finalPath) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(grid.GridToWorld(node.point, true), 0.2f);
            }
        }
    }



}

