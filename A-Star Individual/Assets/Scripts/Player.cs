using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    WorldGrid grid;
	// Use this for initialization
	void Start () {
        grid = GameObject.Find("Grid").GetComponent<WorldGrid>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
