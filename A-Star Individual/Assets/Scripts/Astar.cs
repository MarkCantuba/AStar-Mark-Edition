using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {

    WorldGrid grid;
    NodeHeapArray myNodeArray;
    // Use this for initialization
    void Start () {
        grid = GameObject.Find("Grid").GetComponent<WorldGrid>();
        myNodeArray = new NodeHeapArray();
        print(myNodeArray);

        Node node1 = new Node(1, 1, false, 56, 100);
        Node node2 = new Node(1, 1, false, 57, 100);
        Node node3 = new Node(1, 1, false, 54, 100);
        Node node4 = new Node(1, 1, false, 51, 100);
        Node node5 = new Node(1, 1, false, 40, 100);
        Node node6 = new Node(1, 1, false, 55, 100);
        Node node7 = new Node(1, 1, false, 55, 100);

        myNodeArray.Push(node1);
        
        myNodeArray.Push(node2);
        myNodeArray.Push(node3);
        myNodeArray.Push(node4);
        myNodeArray.Push(node5);
        myNodeArray.Push(node6);
        myNodeArray.Push(node7);
        print(myNodeArray);
        print(myNodeArray.Pop());
        print(myNodeArray);
        print(myNodeArray.Pop());
        print(myNodeArray);
        print(myNodeArray.Pop());
        print(myNodeArray);
    }

    void AStar(Vector2 startPoint, Vector2 endPoint) {

        Vector2 playerGridPos = grid.WorldToGrid(startPoint);
        Node startNode = grid.gridNode[(int)playerGridPos.x, (int)playerGridPos.y];
        startNode.FCost = HeuristicCost(startPoint, endPoint);
        startNode.gCost = 0;
        startNode.neighbors = grid.GetNeighbors(startNode);
        print(startNode + "<<<<<");
        foreach (Node node in startNode.neighbors)
        {
            print(node);
        }

        Vector2 goalGridPos = grid.WorldToGrid(endPoint);
        Node endNode = grid.gridNode[(int)goalGridPos.x, (int)goalGridPos.y];
        endNode.FCost = 0;

        NodeHeapArray openSet = new NodeHeapArray();
        List<Node> closedSet = new List<Node>();

        openSet.Push(startNode);



        return;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) AStar(transform.position, GameObject.Find("GoalTile").transform.position);

	}

    float HeuristicCost(Vector2 nodePoint, Vector2 goalPoint, int biasValue=1) {
        return Vector2.Distance(nodePoint, goalPoint) * biasValue;
    }
    
}

