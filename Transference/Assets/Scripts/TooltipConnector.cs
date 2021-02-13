using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipConnector : MonoBehaviour
{
    public SimpleTooltip myTooltip;
    public UsableScript myUsable;

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
        if(myTooltip != null)
        {
            if(myUsable != null)
            {
                if(myUsable.GetType() == typeof(ComboSkill))
                {
                    ComboSkill combo = myUsable as ComboSkill;

                    myTooltip.infoLeft = "Combo: " + combo.NAME + " \n " +  combo.FIRST + "," +  combo.SECOND + "," + combo.THIRD + ". \n" + "Grant + 1 action on next turn when above combination is used";

                }
                else if (myUsable.GetType() == typeof(ItemScript))
                {
                    ItemScript item = myUsable as ItemScript;

                    myTooltip.infoLeft = "Item: " + item.NAME + " \n " + item.DESC;

                }
                else if (myUsable.GetType() == typeof(ArmorScript))
                {
                    ArmorScript armor = myUsable as ArmorScript;

                    myTooltip.infoLeft = "Armor: " + armor.NAME + " \n " + armor.DESC;

                }
                else if (myUsable.GetType() == typeof(CommandSkill))
                {
                    CommandSkill cmd = myUsable as CommandSkill;

                    if (cmd.SUBTYPE == SubSkillType.Ailment)
                    {

                         myTooltip.infoLeft = "Ailment: " + cmd.NAME + "\n"  + Common.GetSideEffectText(cmd.EFFECT);

                    }
                    else if(cmd.SUBTYPE == SubSkillType.Buff )
                    {
                        myTooltip.infoLeft = "Buff: " + cmd.NAME + "\n" + cmd.DESC;
                    }
                    else if (cmd.SUBTYPE == SubSkillType.Debuff)
                    {
                        myTooltip.infoLeft = "DeBuff: " + cmd.NAME + "\n" + cmd.DESC;
                    }


                }

            }
       

        }
    }
}
