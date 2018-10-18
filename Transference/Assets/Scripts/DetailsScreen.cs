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
            if (i == selectedContent)
            {
                selectableContent[i].color = Color.yellow;
            }
            else
            {
                selectableContent[i].color = Color.white;
            }
        }
        if (nameText)
        {
            nameText.text = currentObj.NAME;
        }



        if (skills != null)
        {
            skillSlots currentSlot = null;
            switch (detail)
            {
                case DetailType.Command:
                    currentSlot = currentObj.BATTLE_SLOTS;
                    sectionText.text = "Command Skills";
                    if(selectedContent < 3)
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
                    else
                    {
                        Debug.Log("Whats thes actuals fucks");
                    }
                }
            }

        }
        if (selectedHitlist != null)
        {
            for (int i = 0; i < selectedHitlist.Count; i++)
            {
                armorreacts[i].sprite = armorSprites[(int)selectedHitlist[i]];
                if(selectableContent[selectedContent] == armorreacts[i])
                {
                    switch((int)selectedHitlist[i])
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

        if(wardSlider)
        {
            if(currentObj.ARMOR.NAME != "none")
            {
                wardSlider.gameObject.SetActive(true);
                wardSlider.maxValue = currentObj.ARMOR.MAX_HEALTH;
                wardSlider.value = currentObj.ARMOR.HEALTH;
                if(sliderText)
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
        if (val > 0)
            attributes[0].text = "Str: " + currentObj.BASE_STATS.STRENGTH + " (+" + val + ")";
        else if (val == 0)
            attributes[0].text = "Str: " + currentObj.BASE_STATS.STRENGTH;
        else
            attributes[0].text = "Str: " + currentObj.BASE_STATS.STRENGTH + " (" + val+ ")";


        val = currentObj.STATS.DEFENSE + currentObj.ARMOR.DEFENSE;
        if (val > 0)
            attributes[1].text = "Def: " + currentObj.BASE_STATS.DEFENSE + " (+" + val + ")";
        else if (val == 0)
            attributes[1].text = "Def: " + currentObj.BASE_STATS.DEFENSE;
        else
            attributes[1].text = "Def: " + currentObj.BASE_STATS.DEFENSE + " (" + val + ")";


        val = currentObj.STATS.SPEED + currentObj.ARMOR.SPEED;
        if (val > 0)
            attributes[2].text = "Spd: " + currentObj.BASE_STATS.SPEED + " (+" + val + ")";
        else if (val == 0)
            attributes[2].text = "Spd: " + currentObj.BASE_STATS.SPEED;
        else
            attributes[2].text = "Spd: " + currentObj.BASE_STATS.SPEED + " (" + val + ")";

        val = currentObj.STATS.MAGIC;
        if (val > 0)
            attributes[3].text = "Mag: " + currentObj.BASE_STATS.MAGIC + " (+" + val + ")";
        else if (val == 0)
            attributes[3].text = "Mag: " + currentObj.BASE_STATS.MAGIC;
        else
            attributes[3].text = "Mag: " + currentObj.BASE_STATS.MAGIC + " (" + val + ")";

        val = currentObj.STATS.RESIESTANCE + currentObj.ARMOR.RESISTANCE;
        if (val > 0)
            attributes[4].text = "Res: " + currentObj.BASE_STATS.RESIESTANCE + " (+" + val + ")";
        else if (val == 0)
            attributes[4].text = "Res: " + currentObj.BASE_STATS.RESIESTANCE;
        else
            attributes[4].text = "Res: " + currentObj.BASE_STATS.RESIESTANCE + " (" + val + ")";

        val = currentObj.STATS.SKILL;
        if (val > 0)
            attributes[5].text = "Skl: " + currentObj.BASE_STATS.SKILL + " (+" + val + ")";
        else if (val == 0)
            attributes[5].text = "Skl: " + currentObj.BASE_STATS.SKILL;
        else
            attributes[5].text = "Skl: " + currentObj.BASE_STATS.SKILL + " (" + val + ")";


    }
}
