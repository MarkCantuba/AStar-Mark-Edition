using System.Collections;
using UnityEngine;

/*
 * Node class that represents a tile in our grid.
 */ 
public class Node : System.IComparable<Node> {

    public Vector2 point;   // World Position of this node

    public float gCost;  
    public float hCost;
    public Node parentNode;

    public float FCost {
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
    public Node(int xCoordinate, int yCoordinate, bool isWalkable, float gCost=Mathf.Infinity, float hCost=Mathf.Infinity, Node parent=null) {
        
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
