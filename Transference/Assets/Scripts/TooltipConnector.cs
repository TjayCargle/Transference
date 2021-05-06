using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipConnector : MonoBehaviour
{
    public SimpleTooltip myTooltip;
    public UsableScript myUsable;
    public EHitType myHitType = EHitType.normal;
    public string defaultString = "";

    private void Start()
    {
        if(myTooltip == null)
        {
            myTooltip = GetComponent<SimpleTooltip>();
        }
    }
    private void OnDisable()
    {
        if (myTooltip != null)
        {
            myTooltip.HideTooltip();
        }
    }
    public void UpdateTooltip()
    {
        if (myTooltip == null)
        {
            myTooltip = GetComponent<SimpleTooltip>();
        }
        if (myTooltip != null)
        {
            if(myUsable != null)
            {
                if(myUsable.GetType() == typeof(ComboSkill))
                {
                    ComboSkill combo = myUsable as ComboSkill;

                    myTooltip.infoLeft = "*Combo: @" + combo.NAME + " \n @" +  combo.FIRST + "," +  combo.SECOND + "," + combo.THIRD + ". \n" + "Grant + "+combo.GAIN+" action on next turn when above combination is used";

                }
                else if (myUsable.GetType() == typeof(ItemScript))
                {
                    ItemScript item = myUsable as ItemScript;

                    myTooltip.infoLeft = "*Item: @" + item.NAME + " \n @" + item.DESC;

                }
                else if (myUsable.GetType() == typeof(ArmorScript))
                {
                    ArmorScript armor = myUsable as ArmorScript;

                    myTooltip.infoLeft = "*Armor: @" + armor.NAME + " \n @" + armor.DESC;

                }
                else if (myUsable.GetType() == typeof(CommandSkill))
                {
                    CommandSkill cmd = myUsable as CommandSkill;

                    if (cmd.SUBTYPE == SubSkillType.Ailment)
                    {

                         myTooltip.infoLeft = "*Ailment: @" + cmd.NAME + "\n @"  + Common.GetSideEffectText(cmd.EFFECT);

                    }
                    else if(cmd.SUBTYPE == SubSkillType.Buff )
                    {
                        myTooltip.infoLeft = "*Buff: @" + cmd.NAME + "\n @" + cmd.DESC;
                    }
                    else if (cmd.SUBTYPE == SubSkillType.Debuff)
                    {
                        myTooltip.infoLeft = "*DeBuff: @" + cmd.NAME + "\n @" + cmd.DESC;
                    }


                }

            }
            else if( myHitType != EHitType.normal)
            {
                switch (myHitType)
                {
                    case EHitType.absorbs:
                        myTooltip.infoLeft = "*Resistance: $Absorb \n @" + "Damage from this element heals the target instead.";
                        break;
                    case EHitType.nulls:
                        myTooltip.infoLeft = "*Resistance: $Null \n @" + "Damage from this element is always zero.";

                        break;
                    case EHitType.reflects:
                        myTooltip.infoLeft = "*Resistance: $Reflect \n @" + "Damage from this element is sent back to the attacker.";
                        break;
                    case EHitType.resists:
                        myTooltip.infoLeft = "*Resistance: $Resist \n @" + "Damage from this element is reduced by 50%.";

                        break;
                    case EHitType.normal:
                        myTooltip.infoLeft = "*Damage: @Normal \n " + "Damage from this element is calculated normally.";

                        break;
                    case EHitType.weak:
                        myTooltip.infoLeft = "*Weakness: ~Weak \n @" + "Damage from this element is increased by 50%. \n Hitting a weakness decreases the target's actions by 1.";

                        break;
                    case EHitType.savage:
                        myTooltip.infoLeft = "*Weakness: ~Savage \n @" + "Damage from this element is doubled and the target loses an additional action next turn. \n Hitting a weakness decreases the target's actions by 1.";

                        break;
                    case EHitType.cripples:
                        myTooltip.infoLeft = "*Weakness: ~Crippling \n @" + "Damage from this element is doubled and the user is inflicted with the crippled status. \n Hitting a weakness decreases the target's actions by 1. \n A crippled unit takes double damage, deals half damage, and can only move 1 tile at a time.";

                        break;
                    case EHitType.lethal:
                        myTooltip.infoLeft = "*Weakness: ~Lethal \n @" + "Damage from this element is tripled, the target is left in a crippled state, and the target loses an additional action next turn. \n Hitting a weakness decreases the target's actions by 1. \n A crippled unit takes double damage, deals half damage, and can only move 1 tile at a time."; 

                        break;
                }


            }
            else
            {
                myTooltip.infoLeft = defaultString;
            }
        }
    }
}