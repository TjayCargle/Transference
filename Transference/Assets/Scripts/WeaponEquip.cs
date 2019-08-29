using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : Equipable
{

    [SerializeField]
    private int myAttack;
    [SerializeField]
    private int myAccurracy;
    [SerializeField]
    private int myCritChance;
    [SerializeField]
    private int myStartkDist;
    [SerializeField]
    private int myAttackRange;
    [SerializeField]
    private EType eType;
    [SerializeField]
    private Element myAfinity = Element.Slash;
    [SerializeField]
    private int useCount;
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private int weaponid = 1;
    [SerializeField]
    private WeaponScript equipped;
    private GridObject owner;

    public WeaponScript EQUIPPED
    { get { return equipped; } }

    public GridObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public Element ELEMENT
    {
        get { return equipped.ELEMENT; }
      
    }
    public EType ATTACK_TYPE
    {

        get { return equipped.ATTACK_TYPE; }
      
    }

    public RangeType RTYPE
    {
        get { return equipped.ATKRANGE; }
    }

    public int ATTACK
    {
        get { return (int)equipped.ATTACK + LEVEL; }
       
    }
    public int ACCURACY
    {
        get { return equipped.ACCURACY; }
       
    }
    public int CRIT
    {
        get { return equipped.CRIT; }
      
    }
    public int DIST
    {
        get { return equipped.DIST; }
       
    }
    public int Range
    {
        get { return equipped.Range; }
    }
    public int USECOUNT
    {
        get { return equipped.USECOUNT; }
     
    }

    public int BOOSTVAL
    {
        get { return equipped.BOOSTVAL; }

    }

    public int LEVEL
    {
        get
        {
            if (equipped)
            {
                return equipped.LEVEL;
            }
            return level;
        }
      
    }

    public int WEPID
    {
        get { return equipped.INDEX; }

    }
    public void Equip(WeaponScript weapon)
    {
        base.Equip(weapon);       
       
        equipped = weapon;
    }
    public void unEquip()
    {
        //WeaponScript weapon = Common.noWeapon;
        //weapon.INDEX = -1;
        //weapon.name = "none";
        //weapon.DESC = "No weapon equipped";
        //Equip(weapon);

    }
    public void Use()
    {
        equipped.Use();

    }
}
