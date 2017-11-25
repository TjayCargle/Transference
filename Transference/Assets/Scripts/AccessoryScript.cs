using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Stat
{
    None,
    Attack,
    Accuraccy,
    Defense,
    Speed, 
    Luck,
    Movement,
    Atk_Dist,
    Affinity
}
public class AccessoryScript : UsableScript
{
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
	
}
