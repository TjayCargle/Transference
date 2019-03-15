using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DetailsScreen : MonoBehaviour
{

    public LivingObject currentObj;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text sectionText;

    [SerializeField]
    Image spriteImg;

    [SerializeField]
    Text[] skills;

    [SerializeField]
    Image[] armorreacts;

    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Sprite[] armorSprites;

    [SerializeField]
    public DetailType detail = DetailType.Command;

    [SerializeField]
    Text[] attributes;

    [SerializeField]
    DetailsTabController leftDetailsTab;

    [SerializeField]
    DetailsTabController rightDetailsTab;

    public int selectedContent;
    [SerializeField]
    Image[] selectableContent;

    [SerializeField]
    Slider wardSlider;

    [SerializeField]
    Text sliderText;
    private int lastSelectedContent;
    private GridObject lastObj;
    private DetailType lastDetail;
    private List<EHitType> selectedHitlist;
    [SerializeField]
    GameObject SkillsObj;
    [SerializeField]
    GameObject expObj;

    [SerializeField]
    Text lvLevelText;

    [SerializeField]
    Slider lvSlider;

    [SerializeField]
    Text lvsliderText;

    [SerializeField]
    Text physLevelText;

    [SerializeField]
    Slider physSlider;

    [SerializeField]
    Text physsliderText;

    [SerializeField]
    Text magLevelText;

    [SerializeField]
    Slider magSlider;

    [SerializeField]
    Text magsliderText;

    [SerializeField]
    Text skLevelText;

    [SerializeField]
    Slider skSlider;

    [SerializeField]
    Text sksliderText;
    [SerializeField]
    Text statusText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentObj)
        {
            if (spriteImg)
            {
                spriteImg.sprite = currentObj.GetComponent<SpriteRenderer>().sprite;
            }

            if (currentObj != lastObj)
            {
                updateDetails();
            }

            if (lastSelectedContent != selectedContent)
            {
                updateDetails();
            }


            if (detail != lastDetail)
            {
                updateDetails();
            }

            lastDetail = detail;
            lastObj = currentObj;
            lastSelectedContent = selectedContent;


        }
    }

    public void updateDetails()
    {
        if (!currentObj)
            return;
        selectedHitlist = currentObj.ARMOR.HITLIST;

        descriptionText.text = "";

        for (int i = 0; i < selectableContent.Length; i++)
        {
            if (i >= 8 && i <= 13)
            {
                continue;
            }
            if (selectableContent[i])
            {

                if (i == selectedContent)
                {
                    selectableContent[i].color = Color.yellow;
                }
                else
                {
                    selectableContent[i].color = Color.white;
                }
            }
        }
        if (nameText)
        {
            nameText.text = currentObj.NAME;
        }
        if(statusText)
        {
            statusText.text ="Current Status: " + currentObj.PSTATUS.ToString();
        }
        switch (selectedContent)
        {
            case 33:
                if(currentObj.PSTATUS == PrimaryStatus.normal)
                {
                    descriptionText.text = "Staus is normal. No modifaction to movement or damage output";
                }
                else if (currentObj.PSTATUS == PrimaryStatus.crippled)
                {
                    descriptionText.text = "Staus is crippled! While crippled target will do half damage, take double damage, and movement will be reduced to 1";
                }
                break;
            case 8:
                descriptionText.text = "Str = Strength. Strength determines how much damage is delt with a physical attack.";
                break;
            case 9:
                descriptionText.text = "Def = Defense. Defense reduces damage from physical attacks.";
                break;
            case 10:
                descriptionText.text = "Mag = Magic. Magic determines how much damage is delt with a magical attack.";
                break;
            case 11:
                descriptionText.text = "Res = Resistance. Resistance reduces damage from magical attacks.";
                break;
            case 12:
                descriptionText.text = "Spd = Speed. Speed affects your action count and slightly affects accurracy/evasion. Every 10 speed generates 1 action.";
                break;
            case 13:
                descriptionText.text = "Skl = Skill. Skill increases your chances of auto skills activating and ailments such as burn going off.";
                break;
            case 14:
                descriptionText.text = "Water Element. Water based moves generally hit all targets in the area.";
                break;
            case 15:
                descriptionText.text = "Fire Element. Fire based moves generally have larger targeting areas.";
                break;
            case 16:
                descriptionText.text = "Ice Element. Ice based moves generally hit distant tiles.";
                break;
            case 17:
                descriptionText.text = "Electric Element. Electric moves generally hit a random amount of times within a range.";
                break;
            case 18:
                descriptionText.text = "Slash Element. Slash based moves generally hit more than once.";
                break;
            case 19:
                descriptionText.text = "Pierce Element. Pierce based moves generally hits multiple targets in range.";
                break;
            case 20:
                descriptionText.text = "Blunt Element. Blunt based moves generally hit 1 tile away.";
                break;

            case 28:
                descriptionText.text = "Wards have strength. Once a ward's str reaches 0, it will break. This strength will charge by 20% at the begining of the phase.";
                break;

            case 29:
                descriptionText.text = "This is your overall level. This is increased by <color=yellow>attacking </color>and <color=yellow>killing enemies</color>. Leveing this up increases <color=#00ade0>all stats</color> including <color=lime>Health</color>,<color=#e400e9> SP</color>, and <color=orange>FT</color>.";
                break;
            case 30:
                descriptionText.text = "This is your Physical level. This is increased by using <color=yellow>physical skills</color>. Leveling this up randomly increases <color=#ff117a>Strength</color> or <color=orange>Defense</color>.";
                break;
            case 31:
                descriptionText.text = "This is your magical level. This is increased by using <color=yellow>magical skills</color>. Leveling this up randomly increases <color=#b400e9>Magic</color> or <color=#ce0e96>Resistance</color>.";
                break;
            case 32:
                descriptionText.text = "This is your Skill level. This is increased by using <color=yellow>basic attacks</color>. Leveling this up randomly increases <color=cyan>Speed</color> or <color=#00FF00>Skill</color>.";
                break;

        }
        if (detail != DetailType.Exp)
        {
            if (expObj)
            {
                expObj.gameObject.SetActive(false);
            }
            if (SkillsObj)
            {
                SkillsObj.gameObject.SetActive(true);
            }

            if (skills != null)
            {
                skillSlots currentSlot = null;
                switch (detail)
                {
                    case DetailType.Command:
                        currentSlot = currentObj.BATTLE_SLOTS;
                        sectionText.text = "Command Skills";
                        if (selectedContent < 3)
                        {
                            descriptionText.text = "Command Skills are usable skills that require a cost and take up 1 action when used.";
                        }
                        break;
                    case DetailType.Passive:
                        currentSlot = currentObj.PASSIVE_SLOTS;
                        sectionText.text = "Passive Skills";
                        if (selectedContent < 3)
                        {
                            descriptionText.text = "Pasive Skills are non-useable skills that are always active when equipped.";
                        }
                        break;
                    case DetailType.Auto:
                        currentSlot = currentObj.AUTO_SLOTS;
                        sectionText.text = "Auto Skills";
                        if (selectedContent < 3)
                        {
                            descriptionText.text = "Auto Skills are skills that has a chance to activate after using a basic attack while equipped.";
                        }
                        break;
                    case DetailType.Opportunity:
                        currentSlot = currentObj.OPP_SLOTS;
                        sectionText.text = "Opportunity Skills";
                        if (selectedContent < 3)
                        {
                            descriptionText.text = "Opportunity Skills grant a free action after an ally uses a specified type of move.";
                        }
                        break;
                    case DetailType.BasicAtk:
                        sectionText.text = "Basic Attacks";
                        if (selectedContent < 3)
                        {
                            descriptionText.text = "Basic Attacks when equipped replace the default Attack option and doesn't require any cost to use.";
                        }
                        break;
                    case DetailType.Armor:
                        sectionText.text = "Wards";
                        if (selectedContent < 3)
                        {
                            descriptionText.text = "Wards when equipped changes elemental affinities and affect spd, def, and res.";
                        }
                        break;
                    case DetailType.Buffs:
                        sectionText.text = "Buffs";
                        if (selectedContent < 3)
                        {
                            descriptionText.text = "Buffs are temorary stat boosts that generally last 3 turns. While you can stack buffs, you cannot stack the same buff on a character.";
                        }
                        break;
                    case DetailType.Debuffs:
                        sectionText.text = "Debuffs";
                        if (selectedContent < 3)
                        {
                            descriptionText.text = "Debuffs are temorary stat drops that generally last 3 turns. The same debuff cannot be applied more than ocne on a character.";
                        }
                        break;
                    case DetailType.Effects:
                        sectionText.text = "Ailments";
                        if (selectedContent < 3)
                        {
                            descriptionText.text = "Ailments are negative status effects that disrupt the character's turn or cause them to take damage.";
                        }
                        break;
                }
                for (int i = 0; i < skills.Length; i++)
                {
                    if (currentSlot)
                    {

                        if (currentSlot.SKILLS.Count > i)
                        {
                            skills[i].text = currentSlot.SKILLS[i].NAME;
                            if (selectableContent[selectedContent].GetComponentInChildren<Text>())
                            {
                                if (selectableContent[selectedContent].GetComponentInChildren<Text>() == skills[i])
                                {
                                    descriptionText.text = currentSlot.SKILLS[i].DESC;
                                }
                            }
                        }
                        else
                        {
                            skills[i].text = "-";
                        }
                    }

                    else
                    {
                        if (detail == DetailType.Armor)
                        {
                            if (currentObj.INVENTORY.ARMOR.Count > i)
                            {
                                skills[i].text = currentObj.INVENTORY.ARMOR[i].NAME;
                                if (selectableContent[selectedContent].GetComponentInChildren<Text>())
                                {
                                    if (selectableContent[selectedContent].GetComponentInChildren<Text>() == skills[i])
                                    {
                                        descriptionText.text = currentObj.INVENTORY.ARMOR[i].DESC;
                                        selectedHitlist = currentObj.INVENTORY.ARMOR[i].HITLIST;
                                    }
                                }
                            }
                            else
                            {
                                skills[i].text = "-";
                            }
                        }
                        else if (detail == DetailType.BasicAtk)
                        {
                            if (currentObj.INVENTORY.WEAPONS.Count > i)
                            {
                                skills[i].text = currentObj.INVENTORY.WEAPONS[i].NAME + " LV: " + currentObj.INVENTORY.WEAPONS[i].LEVEL;
                                if (selectableContent[selectedContent].GetComponentInChildren<Text>())
                                {
                                    if (selectableContent[selectedContent].GetComponentInChildren<Text>() == skills[i])
                                    {
                                        descriptionText.text = currentObj.INVENTORY.WEAPONS[i].DESC;
                                    }
                                }
                            }
                            else
                            {
                                skills[i].text = "-";
                            }
                        }
                        else if (detail == DetailType.Buffs)
                        {
                            if (currentObj.INVENTORY.BUFFS.Count > i)
                            {
                                skills[i].text = currentObj.INVENTORY.BUFFS[i].NAME;
                                if (selectableContent[selectedContent].GetComponentInChildren<Text>())
                                {
                                    if (selectableContent[selectedContent].GetComponentInChildren<Text>() == skills[i])
                                    {
                                        descriptionText.text = currentObj.INVENTORY.BUFFS[i].DESC;
                                    }
                                }
                            }
                            else
                            {
                                skills[i].text = "-";
                            }
                        }
                        else if (detail == DetailType.Debuffs)
                        {
                            if (currentObj.INVENTORY.DEBUFFS.Count > i)
                            {
                                skills[i].text = currentObj.INVENTORY.DEBUFFS[i].NAME;
                                if (selectableContent[selectedContent].GetComponentInChildren<Text>())
                                {
                                    if (selectableContent[selectedContent].GetComponentInChildren<Text>() == skills[i])
                                    {
                                        descriptionText.text = currentObj.INVENTORY.DEBUFFS[i].DESC;
                                    }
                                }
                            }
                            else
                            {
                                skills[i].text = "-";
                            }
                        }
                        else if (detail == DetailType.Effects)
                        {
                            if (currentObj.INVENTORY.EFFECTS.Count > i)
                            {
                                skills[i].text = currentObj.INVENTORY.EFFECTS[i].EFFECT.ToString();
                                if (selectableContent[selectedContent].GetComponentInChildren<Text>())
                                {
                                    if (selectableContent[selectedContent].GetComponentInChildren<Text>() == skills[i])
                                    {
                                        descriptionText.text = Common.GetSideEffectText(currentObj.INVENTORY.EFFECTS[i].EFFECT);
                                    }
                                }
                            }
                            else
                            {
                                skills[i].text = "-";
                            }
                        }
                        else
                        {
                            Debug.Log("Whats thes actuals fucks");
                        }
                    }
                }

            }
        }
        else
        {
            sectionText.text = "Levels and XP";
            if (expObj)
            {
                expObj.gameObject.SetActive(true);
            }
            if (SkillsObj)
            {
                SkillsObj.gameObject.SetActive(false);
            }
            if (selectedContent < 3)
            {
                descriptionText.text = "Levels show growth in a character and XP shows how close an area is to leveling up.";
            }
            if (lvSlider)
            {
                lvLevelText.text = "Exp LV: " + currentObj.LEVEL;
                lvSlider.gameObject.SetActive(true);
                lvSlider.maxValue = 100;
                lvSlider.value = currentObj.BASE_STATS.EXP;
                if (lvsliderText)
                {
                    float trueAmt = lvSlider.value / lvSlider.maxValue;
                    trueAmt *= 100.0f;
                    trueAmt = Mathf.Round(trueAmt);
                    lvsliderText.text = "" + trueAmt + "%";
                }
            }

            if (physSlider)
            {
                physLevelText.text = "Phys LV: " + currentObj.PHYSLEVEL;
                physSlider.gameObject.SetActive(true);
                physSlider.maxValue = 100;
                physSlider.value = currentObj.BASE_STATS.PHYSEXP;
                if (physsliderText)
                {
                    float trueAmt = physSlider.value / physSlider.maxValue;
                    trueAmt *= 100.0f;
                    trueAmt = Mathf.Round(trueAmt);
                    physsliderText.text = "" + trueAmt + "%";
                }

            }

            if (magSlider)
            {
                magLevelText.text = "Mag LV: " + currentObj.MAGLEVEL;
                magSlider.gameObject.SetActive(true);
                magSlider.maxValue = 100;
                magSlider.value = currentObj.BASE_STATS.MAGEXP;
                if (magsliderText)
                {
                    float trueAmt = magSlider.value / magSlider.maxValue;
                    trueAmt *= 100.0f;
                    trueAmt = Mathf.Round(trueAmt);
                    magsliderText.text = "" + trueAmt + "%";
                }

            }

            if (skSlider)
            {
                skLevelText.text = "SK LV: " + currentObj.SKLEVEL;
                skSlider.gameObject.SetActive(true);
                skSlider.maxValue = 100;
                skSlider.value = currentObj.BASE_STATS.SKILLEXP;
                if (sksliderText)
                {
                    float trueAmt = skSlider.value / skSlider.maxValue;
                    trueAmt *= 100.0f;
                    trueAmt = Mathf.Round(trueAmt);
                    sksliderText.text = "" + trueAmt + "%";
                }

            }
        }
        if (selectedHitlist != null)
        {
            for (int i = 0; i < selectedHitlist.Count; i++)
            {
                armorreacts[i].sprite = armorSprites[(int)selectedHitlist[i]];
                if (selectableContent[selectedContent] == armorreacts[i].transform.parent.GetComponent<Image>())
                {
                    switch ((int)selectedHitlist[i])
                    {
                        case 0:
                            descriptionText.text = "Abs = Absorb. Absorbing an element allows it to heal by the damage it would have delt.";
                            break;
                        case 1:
                            descriptionText.text = "Null = Nullify. Nullifying an element reduces the damage to 0.";
                            break;
                        case 2:
                            descriptionText.text = "Rpl = Repel. Repeling an element sends it back to the attacker.";
                            break;
                        case 3:
                            descriptionText.text = "RS = Resist. Resisting an element reduces the damage it would have delt.";
                            break;
                        case 4:
                            descriptionText.text = "Damage applied normally. Str against Def and Mag against Res.";
                            break;
                        case 5:
                            descriptionText.text = "WK = Weak. Weakening damage slightly increases the damage it would have delt.";
                            break;
                        case 6:
                            descriptionText.text = "Svg = Savage. Savage damage moderately increases damage it would have delt and reduces generated action count by 1.";
                            break;
                        case 7:
                            descriptionText.text = "Cpl = Cripple. Crippling damage moderately increases damage it would have delt and puts target in crippled state for 1 turn.";
                            break;
                        case 8:
                            descriptionText.text = "Lthl = Lethal.  Lethal damage heavyily increases damage it would have delt, reduces generated action count by 1, and puts target into crippled state for 1 turn.";
                            break;
                        default:


                            break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < currentObj.ARMOR.HITLIST.Count; i++)
            {
                armorreacts[i].sprite = armorSprites[(int)currentObj.ARMOR.HITLIST[i]];
            }
        }

        if (wardSlider)
        {
            if (currentObj.ARMOR.NAME != "none")
            {
                wardSlider.gameObject.SetActive(true);
                wardSlider.maxValue = currentObj.ARMOR.MAX_HEALTH;
                wardSlider.value = currentObj.ARMOR.HEALTH;
                if (sliderText)
                {
                    float trueAmt = wardSlider.value / wardSlider.maxValue;
                    trueAmt *= 100.0f;
                    trueAmt = Mathf.Round(trueAmt);
                    sliderText.text = "" + trueAmt + "%";
                }
            }
            else
            {
                wardSlider.gameObject.SetActive(false);
            }
        }

        int val = currentObj.STATS.STRENGTH;
        //if (val > 0)
        //    attributes[0].text = "Str: " + currentObj.BASE_STATS.STRENGTH + " (+" + val + ")";
        //else if (val == 0)
        //    attributes[0].text = "Str: " + currentObj.BASE_STATS.STRENGTH;
        //else
        //    attributes[0].text = "Str: " + currentObj.BASE_STATS.STRENGTH + " (" + val + ")";
        attributes[0].text = "Str: " + (currentObj.BASE_STATS.STRENGTH + currentObj.STATS.STRENGTH);

        //val = currentObj.STATS.DEFENSE + currentObj.ARMOR.DEFENSE;
        //if (val > 0)
        //    attributes[1].text = "Def: " + currentObj.BASE_STATS.DEFENSE + " (+" + val + ")";
        //else if (val == 0)
        //    attributes[1].text = "Def: " + currentObj.BASE_STATS.DEFENSE;
        //else
        //    attributes[1].text = "Def: " + currentObj.BASE_STATS.DEFENSE + " (" + val + ")";
        attributes[1].text = "Def: " + (currentObj.BASE_STATS.DEFENSE + currentObj.STATS.DEFENSE);

        //val = currentObj.STATS.MAGIC;
        //if (val > 0)
        //    attributes[2].text = "Mag: " + currentObj.BASE_STATS.MAGIC + " (+" + val + ")";
        //else if (val == 0)
        //    attributes[2].text = "Mag: " + currentObj.BASE_STATS.MAGIC;
        //else
        //    attributes[2].text = "Mag: " + currentObj.BASE_STATS.MAGIC + " (" + val + ")";

        attributes[2].text = "Spd: " + (currentObj.BASE_STATS.SPEED + currentObj.STATS.SPEED);

        //val = currentObj.STATS.RESIESTANCE + currentObj.ARMOR.RESISTANCE;
        //if (val > 0)
        //    attributes[3].text = "Res: " + currentObj.BASE_STATS.RESIESTANCE + " (+" + val + ")";
        //else if (val == 0)
        //    attributes[3].text = "Res: " + currentObj.BASE_STATS.RESIESTANCE;
        //else
        //    attributes[3].text = "Res: " + currentObj.BASE_STATS.RESIESTANCE + " (" + val + ")";

        attributes[3].text = "Mag: " + (currentObj.BASE_STATS.MAGIC + currentObj.STATS.MAGIC);
        attributes[4].text = "Res: " + (currentObj.BASE_STATS.RESIESTANCE + currentObj.STATS.RESIESTANCE);

        //val = currentObj.STATS.SPEED + currentObj.ARMOR.SPEED;
        //if (val > 0)
        //    attributes[4].text = "Spd: " + currentObj.BASE_STATS.SPEED + " (+" + val + ")";
        //else if (val == 0)
        //    attributes[4].text = "Spd: " + currentObj.BASE_STATS.SPEED;
        //else
        //    attributes[4].text = "Spd: " + currentObj.BASE_STATS.SPEED + " (" + val + ")";


        // val = currentObj.STATS.SKILL;
        //if (val > 0)
        //    attributes[5].text = "Skl: " + currentObj.BASE_STATS.SKILL + " (+" + val + ")";
        //else if (val == 0)
        //    attributes[5].text = "Skl: " + currentObj.BASE_STATS.SKILL;
        //else
        //    attributes[5].text = "Skl: " + currentObj.BASE_STATS.SKILL + " (" + val + ")";

        attributes[5].text = "Skl: " + (currentObj.BASE_STATS.SKILL + currentObj.STATS.SKILL);

    }
}
