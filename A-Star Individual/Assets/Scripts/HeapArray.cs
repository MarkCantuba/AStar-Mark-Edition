using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A Class that will carry our node heap array, to reduce search time of nodes
 * containing lowest fCost from O(n) to O(logn). I didn't make my heap a generic 
 * type for simpler implementation! Min Heap representation!
 */
public class NodeHeapArray {

    public List<Node> nodeArray;
    int count;

    /*
     * Initialize our heap array. Starting index will be 1 instead of 0.
     */ 
    public NodeHeapArray() {
        nodeArray = new List<Node>();
        nodeArray.Insert(0, null);  // Ensures we start at index 1
        count = 0;
    }

    /*
     * Insert an item to heap array. Since it's a min heap, the root value
     * will always contain node with the lowest fCost (see node for compare method.
     */ 
    public void Push(Node node) {
        nodeArray.Add(node);

        count++;

        // Base case. If there is only one item in our heap, there is no need to heapify list 
        if (count == 1) {  
            return;
        }

        BottomTopHeapify(); // relocate inserted item into its appropriate spot in our list
    }


    /*
     * Pop smallest item off our list, which is the root node.
     */ 
    public Node Pop() {
        if (count == 0) {
            throw new System.Exception("Cannot delete off an empty heap!");
        }

        Node topValue = nodeArray[1];


        if (count > 1)
        {
            Node replace = nodeArray[count];
            nodeArray[1] = replace;
            nodeArray.RemoveAt(count);
        }
        count--;

        TopBottomHeapify();

        return topValue;
    }
    
    /*
     * Re-arrange our heap from bottom to top. used when a new item is inserted
     * at the bottom of the list (index = count).
     */ 
    void BottomTopHeapify() {
        int counter = count;    // Start at the bottom

        // While the node stored in counterr is not on the right place, keep switching nodes
        // until it is on the right spot!
        while (counter > 1 && nodeArray[counter].CompareTo(nodeArray[GetParent(counter)]) < 0)
        {

            Node tempNode = nodeArray[GetParent(counter)];
            nodeArray[GetParent(counter)] = nodeArray[counter];
            nodeArray[counter] = tempNode;
            counter = GetParent(counter);
        }
    }

    /*
     * Re-arrange our heap to preserve it's heapness. This is used when 
     * an item is inserted at index 1, or and item is deleted.
     */ 
    void TopBottomHeapify()
    {
        // If there is only 1 or no item in our heap, then there is no the rearrange our heap.
        if (count == 1 || count == 0)
        {
            return;
        }

        // Start at index 1 instead of 0.
        int currentNode = 1;

        // While we are within bounds of the current array heap count
        while (GetLeftChild(currentNode) <= count)
        {
            int child = GetLeftChild(currentNode);

            
            if (child + 1 <= count)
            {    
                // If the child is greater than the item beside it.
                if (nodeArray[child].CompareTo(nodeArray[child+1]) > 0)
                {
                    child++;    // Move to the next child
                }
            }

            // Otherwise, if the current node is greater than the child node
            // We have to switch the 2 nodes around to preserve it's heap property
            if (nodeArray[currentNode].CompareTo(nodeArray[child]) > 0)
            {
                Node tempNode = nodeArray[currentNode];
                nodeArray[currentNode] = nodeArray[child];
                nodeArray[child] = tempNode;
                currentNode = child;
            }
            else
            {
                return;
            }
        }
    }
    

    /*
     * Get the number of nodes currently stored in our array.
     */ 
    public int Count()
    {
        return this.count;
    }

    /*
     * Get the Left child of item in position 'index'
     */ 
    public int GetLeftChild(int index) {
        // Left child will be odd indexes. +1 ensures that we can start at index 0
        return index * 2;   
    }

    /*
     * Get the right child of item in position 'index'
     */ 
    public int GetRightChild(int index) {
        // Right child will be assigned to even indexes.
        return index * 2 + 1;  
    }

    /*
     * Get the parent node index of the node in index.
     */ 
    public int GetParent(int index) {
        return (index) / 2;
    }

    /*
     * Check if item contains a certain node.
     */ 
    public bool Contains(Node node) {
        if (this.Count() == 0) {
            return false;
        }
        for (int i = 1; i <= count; i++) {
            if (nodeArray[i].point == node.point) {
                return true;
            }
        }
        return false;
    }

    /*
     * To string representation of array heap.
     */ 
    public override string ToString() {
        string stringVersion = "";
        for (int i = 1; i <= count; i++)
        {
            stringVersion += nodeArray[i].ToString() + "\n";
        }
        return stringVersion;
    }

}
