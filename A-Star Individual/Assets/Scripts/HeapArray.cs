using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A Class that will carry our node heap array, to reduce search time of nodes
 * containing lowest fCost from O(n) to O(logn). I didn't make my heap a generic 
 * type for simpler implementation!
 */
public class NodeHeapArray {

    List<Node> nodeArray;
    int count;

    public NodeHeapArray() {
        nodeArray = new List<Node>();
        count = 0;
    }

    public void Push(Node node) {
       
    }

    public Node Pop() {
        return new Node(1,1, false);
    }

    public bool IsHeap() {
        return false;
    }

    public void Heapify() {
        
    }

    /*
     * Get the Left child of item in position 'index'
     */ 
    public int GetLeftChild(int index) {
        // Left child will be odd indexes. +1 ensures that we can start at index 0
        return index * 2 + 1;   
    }

    /*
     * Get the right child of item in position 'index'
     */ 
    public int GetRightChild(int index) {
        // Right child will be assigned to even indexes.
        return index * 2 + 2;  
    }

    public int GetParent(int index) {
        return index / 2;
    }

    public override string ToString() {
        string stringVersion = "";
        foreach (Node node in nodeArray) {
            stringVersion += node.ToString() + " | ";
        }
        return stringVersion;
    }

}
