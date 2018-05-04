using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorScript : UsableScript
{
    [SerializeField]
    private int myDefense;
    [SerializeField]
    private int myRes;
    [SerializeField]
    private int mySpeed;

    [SerializeField]
    private List<EHitType> hitList;

    private LivingObject owner;


    public List<EHitType> HITLIST
    {
        get { return hitList; }
        set { hitList = value; }
    }

    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public int DEFENSE
    {
        get { return myDefense; }
        set { myDefense = value; }
    }
    public int RESISTANCE
    {
        get { return myRes; }
        set { myRes = value; }
    }
    public int SPEED
    {
        get { return mySpeed; }
        set { mySpeed = value; }
    }
  
}
