using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
     protected ManagerScript myManager;
    public TileScript currentTile;
    State currentState;
    public string FullName;
    private bool hasMoved = false;
    protected int MoveDist = 0;
    protected int MinAttackDist = 0;
    protected int MaxAttackDist = 0;
    public bool isSetup = false;


    public bool HASMOVED
    {
        get { return hasMoved; }
        set { hasMoved = value; }
    }

    public virtual int MOVE_DIST
    {
        get { return MoveDist; }
        set { MoveDist = value; }
    }
    public virtual int MIN_ATK_DIST
    {
        get { return MinAttackDist; }
        set { MinAttackDist = value; }
    }
    public virtual int MAX_ATK_DIST
    {
        get { return MaxAttackDist; }
        set { MaxAttackDist = value; }
    }

    // Use this for initialization

    protected virtual void Setup()
    {
        if (GameObject.FindObjectOfType<ManagerScript>())
        {
            myManager = GameObject.FindObjectOfType<ManagerScript>();
        }
        if (myManager)
        {
            if (!myManager.isSetup)
            {
                myManager.Setup();
            }

        }
        isSetup = true;
    }
    protected void Start ()
    {
        if(!isSetup)
        {
            Setup();
        }

	}
	
	// Update is called once per frame
	protected void Update () {
		if(!myManager)
        {
            return;
        }
        currentState = myManager.currentState;
	}
    

}
