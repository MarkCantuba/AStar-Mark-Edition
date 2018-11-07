using System.Collections;
using UnityEngine;

public class Node : IComparer {

    public Vector2 point;

    public int gCost;
    public int hCost;

    public int FCost {
        get {
            return this.gCost + this.hCost;
        }
    }

    public bool walkable = true;

    /*
     * Constructor method that builds our node. 
     * :param gCost -> Distance from one node to the next node.
     * :param hCost -> Heuristic Cost, which will be the distance from this node to goal node.
     * :param x,y coordinate -> (x,y) location in which this node is placed in.
     */ 
    public Node(int xCoordinate, int yCoordinate, int gCost, int hCost, bool isWalkable) {
        this.point = new Vector2(xCoordinate, yCoordinate);
        this.gCost = gCost;
        this.hCost = hCost;
        this.walkable = isWalkable;
    }

    /*
     * Comparator for this node object, for ordering nodes in accordance to their fCost!
     * 1 if x > y
     * -1 if x < y
     * 0 if x == y
     */ 
    public int Compare(object x, object y) {
        Node node1 = (Node) x;
        Node node2 = (Node) y;

        if (node1.FCost > node2.FCost) {
            return 1;
        }

        else if (node1.FCost < node2.FCost) {
            return -1;
        }
        
        else {
            return 0;
        }
    }
}
