using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    [SerializeField]
    SideEffect effect;

    public SideEffect EFFECT
    {
        get { return effect; }
        set { effect = value; }
    }

    public void ApplyReaction(ManagerScript manager, LivingObject living)
    {
        int chance = Random.Range(-5, 5);
        Debug.Log("Status Effect Chance = " + chance);
        switch (effect)
        {
            case SideEffect.paralyze:
                Debug.Log(living.FullName + " is paralyzed");
                if (manager.log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                    manager.log.Log(coloroption + living.NAME + "</color> is <color=#9B870C>paralyzed</color>");
                }
                if (chance <= 0)
                {
                    //Debug.Log("Player is stunned");
                    int dmg = (int)(living.HEALTH * 0.1);
                    manager.DamageGridObject(living, dmg);
                    manager.CreateDmgTextEvent(dmg.ToString(), Color.yellow, living);
                    living.ACTIONS--;
                    // manager.NextTurn("effectScript");
                    manager.CreateTextEvent(this, "" + living.FullName + " is stunned", "stun effect", manager.CheckText, manager.TextStart);
                    if (manager.log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";

                        manager.log.Log(coloroption + living.NAME + "</color> was stunned. " + dmg.ToString() + " health");
                        manager.log.Log(coloroption + living.NAME + "</color> lost " + dmg.ToString() + " health and 1 action from being <color=#9B870C>paralyzed</color>");
                    }
                    return;
                }
                manager.CreateTextEvent(this, "" + living.FullName + " is no longer paralyzed", "no longer paralyzed effect", manager.CheckText, manager.TextStart);
                if (manager.log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                    manager.log.Log(coloroption + living.NAME + "</color> is no longer <color=#9B870C>paralyzed</color>");
                }
                Destroy(this);
                break;
            case SideEffect.sleep:
                Debug.Log(living.FullName + " is sleep");
                if (chance <= 0)
                {
                    //   Debug.Log(living.FullName + " is sleeping");
                    int dmg = -(int)(living.HEALTH * 0.1);
                    manager.DamageGridObject(living, dmg);
                    manager.CreateDmgTextEvent(dmg.ToString(), Color.blue, living);
                    living.ACTIONS = 0;
                    manager.NextTurn("effectScript");
                    manager.CreateTextEvent(this, "" + living.FullName + " is sleeping", "sleep effect", manager.CheckText, manager.TextStart);
                    return;
                }
                manager.CreateTextEvent(this, "" + living.FullName + " woke up", "no longer sleep effect", manager.CheckText, manager.TextStart);
                Destroy(this);
                break;
            case SideEffect.freeze:
                Debug.Log(living.FullName + " is frozen");
                if (chance <= 0)
                {
                    Debug.Log(living.FullName + " is frozen solid");
                    manager.CreateDmgTextEvent("Frozen", Color.cyan, living);
                    living.ACTIONS = 0;
                    manager.NextTurn("effectScript");
                    manager.CreateTextEvent(this, "" + living.FullName + " is frozen", "frozen effect", manager.CheckText, manager.TextStart);
                    return;
                }
                manager.CreateTextEvent(this, "" + living.FullName + " thawed out", "no longer frozen effect", manager.CheckText, manager.TextStart);
                Destroy(this);
                break;
            case SideEffect.burn:
                {

                    Debug.Log(living.FullName + " is burned");
                    int dmg = (int)(living.HEALTH * 0.2);
                    living.ACTIONS++;
                    manager.DamageGridObject(living, dmg);
                    manager.CreateDmgTextEvent(dmg.ToString(), Color.red, living);
                    manager.CreateTextEvent(this, "" + living.FullName + " took damage from their burn", "burned effect", manager.CheckText, manager.TextStart);

                    if (chance > 0)
                    {
                        manager.CreateTextEvent(this, "" + living.FullName + " is no longer burned", "no longer burned effect", manager.CheckText, manager.TextStart);
                        Destroy(this);
                    }
                }
                break;
            case SideEffect.poison:
                {

                    if (manager.log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                        manager.log.Log(coloroption + living.NAME + "</color> is <color=#8A2BE2>poisoned</color>");
                    }
                    Debug.Log(living.FullName + " is poisoned");

                    int dmg = (int)(living.HEALTH * 0.1);
                    manager.DamageGridObject(living, dmg);
                    if (manager.log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                        manager.log.Log(coloroption + living.NAME + "</color> lost " + dmg.ToString() + " health and had STRENGTH debuffed from being <color=#8A2BE2>poisoned</color>");
                    }

                    if (!living.INVENTORY.DEBUFFS.Contains(Common.CommonDebuffStr))
                    {
                        Common.CommonDebuffStr.EFFECT = SideEffect.debuff;
                        Common.CommonDebuffStr.BUFF = BuffType.Str;
                        Common.CommonDebuffStr.BUFFVAL = -10f;
                        Common.CommonDebuffStr.ELEMENT = Element.Buff;
                        Common.CommonDebuffStr.SUBTYPE = SubSkillType.Debuff;

                        living.INVENTORY.DEBUFFS.Add(Common.CommonDebuffStr);
                        DebuffScript buff = living.gameObject.AddComponent<DebuffScript>();
                        buff.SKILL = Common.CommonDebuffStr;
                        buff.BUFF = Common.CommonDebuffStr.BUFF;
                        buff.COUNT = 1;
                        living.ApplyPassives();

                    }
                    if (chance > 0)
                    {
                        Debug.Log(living.FullName + " is no longer poisoned");
                        if (manager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                            manager.log.Log(coloroption + living.NAME + "</color> is no longer <color=#8A2BE2>poisoned</color>");
                        }
                        Destroy(this);
                    }
                    break;

                }
            case SideEffect.slow:
                //living.MOVE_DIST = (int)(living.MOVE_DIST * 0.5f);
                break;
            case SideEffect.bleed:
                {

                    if (manager.log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                        manager.log.Log(coloroption + living.NAME + "</color> is <color=#FF2BE2>bleeding</color>");
                    }
                    Debug.Log(living.FullName + " is bleeding");

                    int dmg = (int)(living.HEALTH * 0.1);
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
                        living.ApplyPassives();

                    }
                    if (chance > 0)
                    {
                        Debug.Log(living.FullName + " is no longer bleeding");
                        if (manager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(living.FACTION)) + ">";
                            manager.log.Log(coloroption + living.NAME + "</color> is no longer <color=#FF2BE2>bleeding</color>");
                        }
                        Destroy(this);
                    }
                    break;

                }
                break;
            default:
                break;
        }
    }
}