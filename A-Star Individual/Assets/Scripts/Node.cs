using UnityEngine;
using System;
using System.Collections.Generic;
/*
* Node class that represents a tile in our grid.
*/
public class Node : IComparable<Node> {

    public Vector2 point;   // World Position of this node

    public float gCost;  // Path cost from one node to another
    public float hCost;  // Heuristic cost, which is the distance from the current position to the goal node
    public Node parentNode; // Parent of the expanded node
    public List<Node> neighbors;

    public float FCost {
        get {
            return this.gCost + this.hCost;
        }
        set { }
    }

    public bool walkable = true;

    /*
     * Constructor method that builds our node. 
     * :param gCost -> Distance from one node to the next node.
     * :param hCost -> Heuristic Cost, which will be the distance from this node to goal node.
     * :param x,y coordinate -> (x,y) location in which this node is placed in.
     */ 
    public Node(int xCoordinate, int yCoordinate, bool isWalkable, float hCost=Mathf.Infinity, float gCost=Mathf.Infinity, Node parent=null) {
        
        this.point = new Vector2(xCoordinate, yCoordinate);
        this.walkable = isWalkable;
        this.gCost = gCost;
        this.hCost = hCost;
        this.parentNode = parent;
        
    }

    public int CompareTo(Node other) {
        return this.FCost.CompareTo(other.FCost);
    }

    public override string ToString() {
        return "{ FCost: " + this.FCost + ",Position: " + this.point + " }";

    }
}
