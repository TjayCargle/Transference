using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected ManagerScript myManager;
    public TileScript currentTile;
    public string FullName;
    [SerializeField]
    public Faction myFaction;
    protected int MoveDist = 0;
    public bool isSetup = false;

    public virtual string NAME
    {
        get { return FullName; }
        set { FullName = value; }
    }

    public virtual Faction FACTION
    {
        get { return myFaction; }
        set { myFaction = value; }
    }

    public virtual int MOVE_DIST
    {
        get { return MoveDist; }
        set { MoveDist = value; }
    }

    // Use this for initialization

    public virtual void Setup()
    {
        if (!isSetup)
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
        }
        isSetup = true;
    }
    protected void Start()
    {
        if (!isSetup)
        {
            Setup();
        }

    }

    public virtual void Die()
    {

    }

}
