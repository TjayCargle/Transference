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

    //[SerializeField]
    //private int myHealth;

    //[SerializeField]
    //private int myMaxHealth;

    [SerializeField]
    private float healthPercent = 100.0f;

    [SerializeField]
    private float maxHealthPercent = 100.0f;
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
    public float MAX_HEALTH
    {
        get { return maxHealthPercent; }
        set
        {
            maxHealthPercent = value;
            if (maxHealthPercent > 100)
            {
                maxHealthPercent = 100;
            }
        }
    }
    public float HEALTH
    {
        get { return healthPercent; }
        set
        {
            healthPercent = value;
            if (healthPercent > maxHealthPercent)
            {
                healthPercent = maxHealthPercent;
            }

            if (healthPercent < 0)
            {
                healthPercent = 0;
            }
        }
    }

}
