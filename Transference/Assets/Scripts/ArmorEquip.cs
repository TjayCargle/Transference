using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class ArmorEquip : Equipable
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

    public List<EHitType> HITLIST
    {
        get { return hitList; }
        set { hitList = value; }
    }

    public void Equip(ArmorScript armor)
    {
        base.Equip(armor);
        this.DEFENSE = armor.DEFENSE;
        this.RESISTANCE = armor.RESISTANCE;
        this.SPEED = armor.SPEED;
        this.HITLIST = armor.HITLIST;
    }
}
