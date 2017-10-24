using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    private int myAttack;
    [SerializeField]
    private int myAccurracy;
    [SerializeField]
    private int myLuck;
    [SerializeField]
    private int myAttackDist;
    [SerializeField]
    private string myName;
    private LivingObject owner;
    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public int ATTACK
    {
        get { return myAttack; }
        set { myAttack = value; }
    }
    public int ACCURACY
    {
        get { return myAccurracy; }
        set { myAccurracy = value; }
    }
    public int LUCK
    {
        get { return myLuck; }
        set { myLuck = value; }
    }
    public int DIST
    {
        get { return myAttackDist ; }
        set { myAttackDist = value; }
    }
    public string NAME
    {
        get { return myName; }
        set { myName = value; }
    }
}
