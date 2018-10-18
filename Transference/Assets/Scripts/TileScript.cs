using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {
    public Color myColor;
    public bool isOccupied;
    public bool canBeOccupied = true;
    MeshRenderer myRender;

	// Use this for initialization
	public void Setup () {
		if(GetComponent<MeshRenderer>())
        {
            myRender = GetComponent<MeshRenderer>();
        }
        myColor = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		if(myRender)
        {
            myRender.material.color = myColor;
        }
	}
}
