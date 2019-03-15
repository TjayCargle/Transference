using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : UsableScript
{
    [SerializeField]
    private int myAttack;
    [SerializeField]
    private int myAccurracy;
    [SerializeField]
    private int myLuck;
    [SerializeField]
    private int myStartkDist;
    [SerializeField]
    private int myAttackRange;
    [SerializeField]
    private EType eType;
    [SerializeField]
    private int useCount;


    private Element myAfinity = Element.Slash;

    private LivingObject owner;

    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public Element AFINITY
    {
        get { return myAfinity; }
        set { myAfinity = value; }
    }
    public EType ATTACK_TYPE
    {

        get { return eType; }
        set { eType = value; }
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
        get { return myStartkDist ; }
        set { myStartkDist = value; }
    }
    public int Range
    {
        get { return myAttackRange; }
        set { myAttackRange = value; }
    }

    public int USECOUNT
    {
        get { return useCount; }
        set { useCount = value; }
    }


  
}
