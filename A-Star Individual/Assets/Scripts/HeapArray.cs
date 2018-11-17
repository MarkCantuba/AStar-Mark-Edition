using System.Collections.Generic;

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

        BottomTopHeapify(count); // relocate inserted item into its appropriate spot in our list
    }


    /*
     * Pop smallest item off our list, which is the root node.
     */ 
    public Node Pop() {
        if (count == 0) {
            throw new System.Exception("Cannot delete off an empty heap!");
        }

        Node topValue = nodeArray[1];

        if (count == 1) {
            count = 0;
            nodeArray.RemoveAt(1);
            return topValue;
        }

        if (count > 1)
        {
            Node replace = nodeArray[count];
            nodeArray[1] = replace;
            nodeArray.RemoveAt(count);
        }
        count--;

        TopBottomHeapify(1);

        return topValue;
    }
    
    /*
     * Re-arrange our heap from bottom to top. used when a new item is inserted
     * at the bottom of the list (index = count). This is uszed for A* too
     */ 
    public void BottomTopHeapify(int index) {
        int counter = index;    // Start at the bottom

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
     * Re-arrange our heap downwards. This is used for pop operation
     */ 
    void TopBottomHeapify(int index) {
        int leftChild = GetLeftChild(index);
        int rightChild = GetRightChild(index);

        int smallest;
        if (leftChild <= count && nodeArray[leftChild].CompareTo(nodeArray[index]) < 0) {
            smallest = leftChild;
        } else {
            smallest = index;
        } 
        if (rightChild <= count && nodeArray[rightChild].CompareTo(nodeArray[smallest]) < 0) {
            smallest = rightChild;
        } if (smallest != index) {
            Node holder = nodeArray[index];
            nodeArray[index] = nodeArray[smallest];
            nodeArray[smallest] = holder;
            TopBottomHeapify(smallest);
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
        return nodeArray.Contains(node);
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
