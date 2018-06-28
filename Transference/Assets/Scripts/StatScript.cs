using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatScript : MonoBehaviour {
    [SerializeField]
    private int myLevel = 1;
    [SerializeField]
    private int myMaxHealth = 100;
    [SerializeField]
    private int myHealth;
    [SerializeField]
    private int myMaxMana = 100;
    [SerializeField]
    private int myMana;
    [SerializeField]
    private int myMaxFatigue = 100;
    [SerializeField]
    private int myFatigue;
    [SerializeField]
    private int myBaseStr;
    [SerializeField]
    private int myBaseMag;
    [SerializeField]
    private int myBaseDefense;
    [SerializeField]
    private int myBaseResistance;
    [SerializeField]
    private int myBaseSpeed;
    [SerializeField]
    private int myBaseLuck;
    [SerializeField]
    private int myMoveDist = 0;
    [SerializeField]
    private int myMaxAtkDist = 0;
    [SerializeField]
    private int myMinAtkDist = 0;
    [SerializeField]
    List<string> appliedPassives = new List<string>();
    public int type = 0;
    private LivingObject owner;

    public List<string> MODS
    {
        get { return appliedPassives; }
        set { appliedPassives = value; }
    }

    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public int MOVE_DIST
    {
        get { return myMoveDist; }
        set { myMoveDist = value; }
    }
    public int Max_Atk_DIST
    {
        get { return  myMaxAtkDist; }
        set { myMoveDist = value; }
    }
    public int Min_Atk_DIST
    {
        get { return myMinAtkDist; }
        set { myMoveDist = value; }
    }
    public int STRENGTH
    {
        get { return  myBaseStr ; }
        set { myBaseStr = value; }
    }
    public int MAGIC
    {
        get { return myBaseMag; }
        set { myBaseMag = value; }
    }
    public int DEFENSE
    {
        get { return  myBaseDefense + owner.ARMOR.DEFENSE; }
        set { myBaseDefense = value; }
    }
    public int RESIESTANCE
    {
        get { return  myBaseResistance + owner.ARMOR.RESISTANCE; }
        set { myBaseResistance = value; }
    }
    public int SPEED
    {
        get { return  myBaseSpeed + owner.ARMOR.SPEED; }
        set { myBaseSpeed = value; }
    }
    public int LUCK
    {
        get { return myBaseLuck + owner.WEAPON.LUCK; }
        set { myBaseLuck = value; }
    }
    public int MAX_HEALTH
    {
        get { return myMaxHealth; }
        set { myMaxHealth = value; }
    }
    public int HEALTH
    {
        get { return myHealth; }
        set { myHealth = value; }
    }
    public int MAX_MANA
    {
        get { return myMaxMana; }
        set { myMaxMana = value; }
    }
    public int MANA
    {
        get { return myMana; }
        set { myMana = value; }
    }
    public int MAX_FATIGUE
    {
        get { return myMaxFatigue; }
        set { myMaxFatigue = value; }
    }
    public int FATIGUE
    {
        get { return myFatigue; }
        set { myFatigue = value; }
    }
    public int LEVEL
    {
        get { return myLevel; }
        set { myLevel = value; }
    }
    public void Reset(bool hard = false)
    {

        Max_Atk_DIST = 0;
        Min_Atk_DIST = 0;
        HEALTH = 0;
        MANA = 0;
        STRENGTH = 0;
        MAGIC = 0;
        DEFENSE = 0;
        RESIESTANCE = 0;
        SPEED = 0;
        LUCK = 0;
        FATIGUE = 0;
        if (hard == true)
        {
            LEVEL = 0;
            MAX_HEALTH = 0;
            MAX_MANA = 0;
            MAX_FATIGUE = 0;

        }
      
    }
}
