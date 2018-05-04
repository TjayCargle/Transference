using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryEquip : Equipable {

    [SerializeField]
    private Stat modifier;
    [SerializeField]
    private int addedValue;
    private LivingObject owner;
    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public Stat STAT
    {
        get { return modifier; }
        set { modifier = value; }
    }
    public int VALUE
    {
        get { return addedValue; }
        set { addedValue = value; }
    }

    public void Equip(AccessoryScript accessory)
    {
        base.Equip(accessory);
        this.STAT = accessory.STAT;
        this.VALUE = accessory.VALUE;

    }
}
