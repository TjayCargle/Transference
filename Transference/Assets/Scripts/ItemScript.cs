using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : UsableScript
{
    [SerializeField]
    private int targetRange;
    [SerializeField]
    private ItemType iType;
    [SerializeField]
    private TargetType tType;
    [SerializeField]
    private float trueValue;
    [SerializeField]
    private SideEffect effect;
    [SerializeField]
    private Element dmgElement;

    [SerializeField]
    private ModifiedStat modStat;

    public int RANGE
    {
        get { return targetRange; }
        set { targetRange = value; }
    }
    
    public ItemType ITYPE
    {
        get { return iType; }
        set { iType = value; }
    }
    public TargetType TTYPE
    {
        get { return tType; }
        set { tType = value; }
    }

    public float VALUE
    {
        get { return trueValue; }
        set { trueValue = value; }
    }

    public Element ELEMENT
    {
        get { return dmgElement; }
        set { dmgElement = value; }
    }
    public SideEffect EFFECT
    {
        get { return effect; }
        set { effect = value; }
    }
    public ModifiedStat STAT
    {
        get { return modStat; }
        set { modStat = value; }
    }
    public bool useItem(LivingObject target)
    {
        bool usedEffect = false;
        switch (ITYPE)
        {
            case ItemType.healthPotion:
        
                int amoint = (int)(target.MAX_HEALTH * trueValue);
                usedEffect = target.ChangeHealth( amoint);

                break;
            case ItemType.manaPotion:
                usedEffect = target.ChangeHealth((int)(target.MAX_HEALTH * trueValue));
                break;
            case ItemType.fatiguePotion:
                usedEffect = target.ChangeHealth((int)(target.MAX_HEALTH * trueValue));
                break;
            case ItemType.cure:
                {
                    switch (effect)
                    {
                        case SideEffect.slow:
                            if(target.SSTATUS == SecondaryStatus.slow)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.rage:
                            if (target.SSTATUS == SecondaryStatus.rage)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.charm:
                            if (target.SSTATUS == SecondaryStatus.charm)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.seal:
                            if (target.SSTATUS == SecondaryStatus.seal)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.poison:
                            if (target.ESTATUS == StatusEffect.poisoned)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.confusion:
                            if (target.SSTATUS == SecondaryStatus.confusion)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.paralyze:
                            if (target.ESTATUS == StatusEffect.paralyzed)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.sleep:
                            if (target.ESTATUS == StatusEffect.sleep)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.freeze:
                            if (target.ESTATUS == StatusEffect.frozen)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.burn:
                            if (target.ESTATUS == StatusEffect.burned)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                    }
                }
                break;
            case ItemType.buff:
                break;
            case ItemType.dmg:
                break;
        }
        return usedEffect;
    }
}
