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
    private ArmorScript equipped;
    [SerializeField]
    private float healthPercent;

    [SerializeField]
    private float maxHealthPercent;
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
    public float MAX_HEALTH
    {
        get
        {
            if (equipped)
            {
                return equipped.MAX_HEALTH;
            }
            return maxHealthPercent;
        }
        set
        {
            if (equipped)
            {
                equipped.MAX_HEALTH = value;
                maxHealthPercent = equipped.MAX_HEALTH;
            }
        }
    }
    public float HEALTH
    {
        get
        {
            if (equipped)
            {
                return equipped.HEALTH;
            }
            return healthPercent;
        }
        set
        {
            if (equipped)
            {
                equipped.HEALTH = value;
                healthPercent = equipped.HEALTH;
            }
        }
    }
    public void Equip(ArmorScript armor)
    {
        base.Equip(armor);
        this.DEFENSE = armor.DEFENSE;
        this.RESISTANCE = armor.RESISTANCE;
        this.SPEED = armor.SPEED;
        this.HITLIST = armor.HITLIST;
        maxHealthPercent = armor.MAX_HEALTH;
        healthPercent = armor.HEALTH;
        equipped = armor;
    }

    public void unEquip()
    {
        this.NAME = "none";
        this.DEFENSE = 0;
        this.RESISTANCE = 0;
        this.SPEED = 0;
        this.HITLIST = Common.noAmor;
        this.DESC = "No armor equipped";
        equipped = null;
    }
    public bool DamageArmor(float amt)
    {
        bool broken = false;
        if (USER)
        {
            if (equipped)
            {
                float trueAmount = amt / (float)USER.MAX_HEALTH;
                trueAmount *= 100;
                equipped.HEALTH -= trueAmount;
                if (equipped.HEALTH <= 0)
                {
                    unEquip();
                    broken = true;
                }
            }

        }
        return broken;
    }
}
