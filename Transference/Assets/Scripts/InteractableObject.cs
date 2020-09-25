using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : GridObject
{
    public Interaction reaction = Interaction.none;
    public RangeType range = RangeType.any;
    public bool interactedWith = false;
    private CommandSkill fakeSKill;
    public bool onlyOnce = true;
    public int targetTileIndex = 0;
    public bool useTargetIndex = false;
    public override void Setup()
    {
        base.Setup();
        fakeSKill = Common.CommonDebuffDef;
        if (fakeSKill != null)
        {
            fakeSKill.RTYPE = range;
        }
    }
    public void Interact(LivingObject invokingObject)
    {
        if (myManager != null && interactedWith == false)
        {

            switch (reaction)
            {
                case Interaction.none:
                    break;
                case Interaction.dropChandelier:
                    break;
                case Interaction.drink:
                    break;
                case Interaction.strUp:
                    break;
                case Interaction.defUp:
                    break;
                case Interaction.spdUp:
                    break;
                case Interaction.slashDmg:
                    {

                        fakeSKill.SKILLTYPE = SkillType.Command;
                        fakeSKill.SUBTYPE = SubSkillType.Skill;
                        fakeSKill.ELEMENT = Element.Slash;
                        if (useTargetIndex == false)
                            myManager.SetTargetList(myManager.GetSkillAttackableTilesOneList(currentTile, fakeSKill));
                        else
                            myManager.SetTargetList(myManager.GetSkillAttackableTilesOneList(myManager.tileMap[targetTileIndex], fakeSKill));
                        bool check = myManager.AttackTargets(invokingObject, fakeSKill);
                        if (check == true)
                        {
                            if (onlyOnce == true)
                                interactedWith = true;

                        }
                    }
                    break;
                case Interaction.pierceDmg:
                    break;
                case Interaction.bluntDmg:
                    break;
                default:
                    break;
            }
        }
    }
}