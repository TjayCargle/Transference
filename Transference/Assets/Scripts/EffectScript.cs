using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : ScriptableObject
{
    [SerializeField]
    private SideEffect effect;

    [SerializeField]
    private int turnsActive;

    public SideEffect EFFECT
    {
        get { return effect; }
        set { effect = value; }
    }

    public int TURNS
    {
        get { return turnsActive; }
        set { turnsActive = value; }
    }

    private LivingObject owner;

    public void ApplyReaction(ManagerScript manager, LivingObject living)
    {
        owner = living;
        if (!living)
        {
            Debug.Log("no target for ailment :( ");
            PoolManager.GetManager().ReleaseEffect(this);
        }

        // int chance = Random.Range(-8, 3);
        // Debug.Log("Status Effect Chance = " + chance);
        switch (effect)
        {
            case SideEffect.confusion:
                {
                    if (living.SSTATUS != SecondaryStatus.confusion)
                    {
                        manager.CreateDmgTextEvent("Normal", Color.magenta, living);
                        living.INVENTORY.EFFECTS.Remove(this);
                        living.updateAilmentIcons();
                        PoolManager.GetManager().ReleaseEffect(this);
                    }
                    else
                    {
                        manager.CreateDmgTextEvent("Confused", Color.magenta, living);

                    }
                }
                break;
            case SideEffect.paralyze:
                //  Debug.Log(living.FullName + " is paralyzed");
                if (manager.log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                    manager.log.Log(coloroption + living.NAME + "</color> is <color=#9B870C>paralyzed</color>");
                }
                if (TURNS > 0)
                {
                    TURNS--;
                    //Debug.Log("Player is stunned");

                    int dmg = (int)(living.MAX_HEALTH * 0.1);
                    manager.DamageGridObject(living, dmg);
                    manager.CreateDmgTextEvent("-1 Action", Color.yellow, living);
                    manager.CreateDmgTextEvent("- " + dmg.ToString() + "<sprite=2> ", Color.yellow, living);
                    manager.CreateDmgTextEvent("Paralysis", Color.yellow, living);

                    living.ACTIONS--;
                    // manager.NextTurn("effectScript");
                    manager.CreateTextEvent(this, "" + living.FullName + " is stunned", "stun effect", manager.CheckText, manager.TextStart);
                    if (manager.log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";

                        manager.log.Log(coloroption + living.NAME + "</color> was stunned. " + dmg.ToString() + " health");
                        manager.log.Log(coloroption + living.NAME + "</color> lost " + dmg.ToString() + " health and 1 action from being <color=#9B870C>paralyzed</color>");
                    }
                    living.updateAilmentIcons();
                    return;
                }
                manager.CreateTextEvent(this, "" + living.FullName + " is no longer paralyzed", "no longer paralyzed effect", manager.CheckText, manager.TextStart);
                if (manager.log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                    manager.log.Log(coloroption + living.NAME + "</color> is no longer <color=#9B870C>paralyzed</color>");
                }
                living.INVENTORY.EFFECTS.Remove(this);
                living.updateAilmentIcons();
                PoolManager.GetManager().ReleaseEffect(this);
                break;
            case SideEffect.sleep:
                // Debug.Log(living.FullName + " is sleep");
                if (TURNS > 0)
                {
                    TURNS--;
                    //   Debug.Log(living.FullName + " is sleeping");
                    int dmg = -(int)(living.MAX_HEALTH * 0.1);
                    manager.DamageGridObject(living, dmg);
                    manager.CreateDmgTextEvent("+ " + (-1 * dmg).ToString() + "<sprite=2> ", Color.blue, living);
                    manager.CreateDmgTextEvent("Sleep", Color.blue, living);
                    living.GENERATED += living.ACTIONS;
                    living.ACTIONS = 0;
                    //manager.NextTurn("effectScript");
                    manager.CreateTextEvent(this, "" + living.FullName + " is sleeping", "sleep effect", manager.CheckText, manager.TextStart);
                    return;
                }
                manager.CreateDmgTextEvent("Woke Up", Color.blue, living);
                manager.CreateTextEvent(this, "" + living.FullName + " woke up", "no longer sleep effect", manager.CheckText, manager.TextStart);
                living.INVENTORY.EFFECTS.Remove(this);
                living.updateAilmentIcons();
                PoolManager.GetManager().ReleaseEffect(this);
                break;
            case SideEffect.freeze:
                //Debug.Log(living.FullName + " is frozen");
                if (TURNS > 0)
                {
                    TURNS--;
                    manager.CreateDmgTextEvent("Frozen", Color.cyan, living);
                    living.ACTIONS = 0;
                    //  manager.NextTurn("effectScript");
                    manager.CreateTextEvent(this, "" + living.FullName + " is frozen", "frozen effect", manager.CheckText, manager.TextStart);
                    return;
                }
                manager.CreateDmgTextEvent("Thawed", Color.cyan, living);
                manager.CreateTextEvent(this, "" + living.FullName + " thawed out", "no longer frozen effect", manager.CheckText, manager.TextStart);
                living.INVENTORY.EFFECTS.Remove(this);
                living.updateAilmentIcons();
                PoolManager.GetManager().ReleaseEffect(this);
                break;
            case SideEffect.burn:
                {

                    if (TURNS > 0)
                    {
                        TURNS--;
                        int dmg = (int)(living.MAX_HEALTH * 0.2);
                        living.ACTIONS++;
                        manager.DamageGridObject(living, dmg);
                        manager.CreateDmgTextEvent("- " + dmg.ToString() + "<sprite=2> ", Color.red, living);
                        manager.CreateDmgTextEvent("Burn", Color.red, living);
                        manager.CreateTextEvent(this, "" + living.FullName + " took damage from their burn", "burned effect", manager.CheckText, manager.TextStart);
                        return;
                    }

                    manager.CreateDmgTextEvent("Burn Healed", Color.red, living);
                    manager.CreateTextEvent(this, "" + living.FullName + " is no longer burned", "no longer burned effect", manager.CheckText, manager.TextStart);
                    living.INVENTORY.EFFECTS.Remove(this);
                    PoolManager.GetManager().ReleaseEffect(this);

                    living.updateAilmentIcons();
                }
                break;
            case SideEffect.poison:
                {

                    if (TURNS > 0)
                    {
                        TURNS--;
                        if (manager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                            manager.log.Log(coloroption + living.NAME + "</color> is <color=#8A2BE2>poisoned</color>");
                        }

                        int dmg = (int)(living.MAX_HEALTH * 0.1);
                        manager.DamageGridObject(living, dmg);
                        manager.CreateDmgTextEvent("-" + dmg.ToString() + "<sprite=2> ", Color.magenta, living);
                        manager.CreateDmgTextEvent("Poison", Color.magenta, living);

                        if (manager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                            manager.log.Log(coloroption + living.NAME + "</color> lost " + dmg.ToString() + " health and had STRENGTH debuffed from being <color=#8A2BE2>poisoned</color>");
                        }

                        else if (!living.INVENTORY.DEBUFFS.Contains(Common.CommonDebuffStr))
                        {
                            CommandSkill strDebuff = Common.CommonDebuffStr;
                            strDebuff.EFFECT = SideEffect.debuff;
                            strDebuff.BUFF = BuffType.Str;
                            strDebuff.BUFFVAL = -10f;
                            strDebuff.ELEMENT = Element.Buff;
                            strDebuff.SUBTYPE = SubSkillType.Debuff;
                            strDebuff.OWNER = living;
                            strDebuff.NAME = "Str Poison";
                            living.INVENTORY.DEBUFFS.Add(strDebuff);


                            DebuffScript buff = living.gameObject.AddComponent<DebuffScript>();
                            buff.SKILL = strDebuff;
                            buff.BUFF = strDebuff.BUFF;
                            buff.COUNT = 2;

                            living.INVENTORY.TDEBUFFS.Add(buff);

                            living.UpdateBuffsAndDebuffs();

                        }
                        else if (living.GetComponent<DebuffScript>())
                        {
                            DebuffScript[] debuffs = living.GetComponents<DebuffScript>();
                            for (int i = 0; i < debuffs.Length; i++)
                            {
                                if (debuffs[i].SKILL == Common.CommonDebuffStr)
                                {
                                    debuffs[i].COUNT++;
                                }
                            }
                        }
                        return;
                    }
                    manager.CreateDmgTextEvent("Poison Healed", Color.magenta, living);

                    manager.CreateTextEvent(this, living.FullName + " is no longer poisoned", "auto atk", manager.CheckText, manager.TextStart);
                    if (manager.log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                        manager.log.Log(coloroption + living.NAME + "</color> is no longer <color=#8A2BE2>poisoned</color>");
                    }
                    living.INVENTORY.EFFECTS.Remove(this);
                    PoolManager.GetManager().ReleaseEffect(this);

                    living.updateAilmentIcons();
                }
                break;

            case SideEffect.slow:
                //living.MOVE_DIST = (int)(living.MOVE_DIST * 0.5f);
                break;
            case SideEffect.bleed:
                {
                    if (TURNS > 0)
                    {
                        TURNS--;
                        if (manager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                            manager.log.Log(coloroption + living.NAME + "</color> is <color=#FF2BE2>bleeding</color>");
                        }
                        Debug.Log(living.FullName + " is bleeding");

                        int dmg = (int)(living.MAX_HEALTH * 0.1);
                        manager.DamageGridObject(living, dmg);
                        if (manager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                            manager.log.Log(coloroption + living.NAME + "</color> lost " + dmg.ToString() + " health and had SPEED debuffed from being <color=#FF2BE2>bleeding</color>");
                        }

                        if (!living.INVENTORY.DEBUFFS.Contains(Common.CommonDebuffSpd))
                        {
                            Common.CommonDebuffStr.EFFECT = SideEffect.debuff;
                            Common.CommonDebuffStr.BUFF = BuffType.Spd;
                            Common.CommonDebuffStr.BUFFVAL = -10f;
                            Common.CommonDebuffStr.ELEMENT = Element.Buff;
                            Common.CommonDebuffStr.SUBTYPE = SubSkillType.Debuff;

                            living.INVENTORY.DEBUFFS.Add(Common.CommonDebuffStr);
                            DebuffScript buff = living.gameObject.AddComponent<DebuffScript>();
                            buff.SKILL = Common.CommonDebuffStr;
                            buff.BUFF = Common.CommonDebuffStr.BUFF;
                            buff.COUNT = 1;

                            living.INVENTORY.TDEBUFFS.Add(buff);
                            living.UpdateBuffsAndDebuffs();

                        }

                        return;
                    }

                    Debug.Log(living.FullName + " is no longer bleeding");
                    if (manager.log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                        manager.log.Log(coloroption + living.NAME + "</color> is no longer <color=#FF2BE2>bleeding</color>");
                    }
                    living.INVENTORY.EFFECTS.Remove(this);
                    PoolManager.GetManager().ReleaseEffect(this);

                    living.updateAilmentIcons();
                    break;

                }
                break;
            default:
                break;
        }

    }

}