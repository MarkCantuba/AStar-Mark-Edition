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
    }

   void AStar(Vector2 startPoint, Vector2 endPoint) {

        Vector2 playerGridPos = grid.WorldToGrid(startPoint);
        Node startNode = grid.gridNode[(int) playerGridPos.x, (int) playerGridPos.y];

        Vector2 goalGridPos = grid.WorldToGrid(endPoint);
        Node endNode = grid.gridNode[(int) goalGridPos.x, (int) goalGridPos.y];

        return;
    }

    // Update is called once per frame
    void Update () {
        //AStar(transform.position, GameObject.Find("GoalTile").transform.position);

	}

    float HeuristicCost(Vector2 nodePoint, Vector2 goalPoint) {
        return Vector2.Distance(nodePoint, goalPoint);
    }
    
}

