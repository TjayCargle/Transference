﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : Equipable {

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
        get { return myStartkDist; }
        set { myStartkDist = value; }
    }
    public int Range
    {
        get { return myAttackRange; }
        set { myAttackRange = value; }
    }
    public void Equip(WeaponScript weapon)
    {
        base.Equip(weapon);
        this.ATTACK = weapon.ATTACK;
        this.ACCURACY = weapon.ACCURACY;
        this.AFINITY = weapon.AFINITY;
        this.ATTACK_TYPE = weapon.ATTACK_TYPE;
        this.DIST = weapon.DIST;
        this.Range = weapon.Range;
    }

}
