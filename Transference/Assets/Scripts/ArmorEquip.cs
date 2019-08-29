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


    [SerializeField]
    private int currentTurnCount = 0;

    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public ArmorScript SCRIPT
    {
        get { return equipped; }

    }
    public int DEFENSE
    {
        get { return equipped.DEFENSE; }

    }
    public int RESISTANCE
    {
        get { return equipped.RESISTANCE; }

    }
    public int SPEED
    {
        get { return equipped.SPEED; }

    }

    public List<EHitType> HITLIST
    {
        get { return equipped.HITLIST; }

    }
    public int ARMORID
    {
        get { return equipped.INDEX; }

    }
    public int TURNCOUNT
    {
        get { return equipped.TURNCOUNT; }
    }

    public int USECOUNT
    {
        get { return equipped.USECOUNT; }
    }

    public void Use()
    {
        if (equipped)
        {
            equipped.Use();
        }
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

    public bool UpdateTurnCount()
    {
        if (equipped != owner.DEFAULT_ARMOR)
        {

            currentTurnCount--;
            if (currentTurnCount <= 0)
            {
                unEquip();
                return true;
            }
        }
        return false;
    }
    public void Equip(ArmorScript armor)
    {
        base.Equip(armor);
        currentTurnCount = armor.TURNCOUNT;
        maxHealthPercent = armor.MAX_HEALTH;
        healthPercent = armor.HEALTH;
        equipped = armor;

        if (owner)
        {
            if (owner.BARRIER)
            {
                if (ARMORID < 200)
                {

                    owner.BARRIER.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Shields/")[ARMORID];
                    owner.BARRIER.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

    public void unEquip()
    {
        //ArmorScript noArmor = Common.noArmor;
        //noArmor.NAME = "none";
        //noArmor.HITLIST = Common.noHitList;
        //noArmor.DESC = "No armor equipped";

        Equip(owner.DEFAULT_ARMOR);
        if (owner)
        {
            if (owner.BARRIER)
            {
                owner.BARRIER.GetComponent<SpriteRenderer>().color = Common.trans;
            }
        }
    }
    public bool DamageArmor(float amt)
    {
        bool broken = false;
        if (USER)
        {
            if (equipped)
            {
                if (equipped != owner.DEFAULT_ARMOR)
                {

                    float trueAmount = amt / (float)USER.MAX_HEALTH;
                    trueAmount *= 100;
                    equipped.HEALTH -= trueAmount;
                    if (equipped.HEALTH <= 0)
                    {
                        //equipped.BREAKS++;
                        //if (equipped.BREAKS % 2 == 0)
                        //{
                        //    equipped.LevelUP();
                        //}
                        unEquip();
                        broken = true;
                    }
                }
            }

        }
        return broken;
    }
}
