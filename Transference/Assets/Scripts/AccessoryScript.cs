using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Stat
{
    Attack,
    Accuraccy,
    Defense,
    Speed, 
    Luck,
    Movement,
    Atk_Dist
}
public class AccessoryScript : MonoBehaviour
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
