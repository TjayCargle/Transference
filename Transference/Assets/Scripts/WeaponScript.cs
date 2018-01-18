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
    }
    public EType ATTACK_TYPE
    {

        get
        {
            EType returnType = EType.magical;
            switch (myAfinity)
            {
                case Element.Water:
                    returnType = EType.magical;
                    break;
                case Element.Fire:
                    returnType = EType.magical;
                    break;
                case Element.Ice:
                    returnType = EType.magical;
                    break;
                case Element.Electric:
                    returnType = EType.magical;
                    break;
                case Element.Slash:
                    returnType = EType.physical;
                    break;
                case Element.Pierce:
                    returnType = EType.physical;
                    break;
                case Element.Blunt:
                    returnType = EType.physical;
                    break;
                case Element.Neutral:
                    returnType = EType.magical;
                    break;

            }
            return returnType;

        }
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
  
}
