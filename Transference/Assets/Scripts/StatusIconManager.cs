using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatusIconManager : MonoBehaviour
{

    [SerializeField]
    Sprite[] statusIconImages;
    List<StatIcon> totalTstatusIcons;
    List<StatIcon> panelIcons;
    public Font usedFont;

    private StatIcon createStatusIcon()
    {
        GameObject newContainer = new GameObject();
        GameObject newImage = new GameObject();
        GameObject newText = new GameObject();

        StatIcon icon = newContainer.AddComponent<StatIcon>();
        newContainer.transform.parent = transform;

        HorizontalLayoutGroup horizontal = newContainer.AddComponent<HorizontalLayoutGroup>();
        horizontal.spacing = 32;
        horizontal.childAlignment = TextAnchor.UpperLeft;
        horizontal.childControlWidth = true;
        horizontal.childControlHeight = true;
        horizontal.childForceExpandHeight = true;
        horizontal.childForceExpandWidth = true;

        icon.myImage = newImage.AddComponent<Image>();
        newImage.transform.parent = newContainer.transform;


        icon.myText = newText.AddComponent<Text>();
        newText.transform.parent = newContainer.transform;
        icon.myText.resizeTextForBestFit = true;
        icon.myText.font = usedFont;
        return icon;
    }

    public void loadIconPanel(LivingObject living)
    {
        if (totalTstatusIcons == null)
        {
            totalTstatusIcons = new List<StatIcon>();
        }
        if (panelIcons == null)
        {
            panelIcons = new List<StatIcon>();
        }
        if(living == null)
        {
            return;
        }
        panelIcons.Clear();
        int panelAmount = 0;
        panelAmount = living.INVENTORY.DEBUFFS.Count;
        panelAmount += living.INVENTORY.BUFFS.Count;
        if (living.GetComponent<EffectScript>())
        {
            panelAmount += 1;
        }
        if (living.SSTATUS == SecondaryStatus.confusion)
        {
            panelAmount += 1;
        }
        if (living.PSTATUS == PrimaryStatus.crippled)
        {
            panelAmount += 1;
        }
        if(totalTstatusIcons.Count < panelAmount)
        {
            int times = panelAmount - totalTstatusIcons.Count;
            for (int i = 0; i < times; i++)
            {
                totalTstatusIcons.Add(createStatusIcon());
            }
        }
        for (int i = 0; i < totalTstatusIcons.Count; i++)
        {
            totalTstatusIcons[i].gameObject.SetActive(false);
        }
        int realCount = 0;
        if(living.PSTATUS == PrimaryStatus.crippled)
        {
            StatIcon statcon = totalTstatusIcons[realCount];
            statcon.myImage.sprite = statusIconImages[(int)StatusIcon.Crippled];
            statcon.myText.text = "Crippled!";
            statcon.gameObject.SetActive(true);
            panelIcons.Add(statcon);
            realCount++;
        }

        if (living.SSTATUS == SecondaryStatus.confusion)
        {
            StatIcon statcon = totalTstatusIcons[realCount];
            statcon.myImage.sprite = statusIconImages[(int)StatusIcon.Confuse];
            statcon.myText.text = "Confused!";
            statcon.gameObject.SetActive(true);
            panelIcons.Add(statcon);
            realCount++;
        }

        if (living.GetComponent<EffectScript>())
        {
            StatIcon statcon = totalTstatusIcons[realCount];

            switch (living.GetComponent<EffectScript>().EFFECT)
            {
                case SideEffect.none:
                    Debug.Log("I am shocked and amazed");
                    break;
                case SideEffect.paralyze:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.Paralyze];
                    statcon.myText.text = "Paralyzed.";
                    break;
                case SideEffect.sleep:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.Sleep];
                    statcon.myText.text = "Sleep";
                    break;
                case SideEffect.freeze:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.Freeze];
                    statcon.myText.text = "Frozen";
                    break;
                case SideEffect.burn:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.Burn];
                    statcon.myText.text = "Burned";
                    break;
                case SideEffect.poison:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.Poison];
                    statcon.myText.text = "Poisoned";
                    break;
                case SideEffect.bleed:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.Bleed];
                    statcon.myText.text = "Bleeding";
                    break;
                default:
                    Debug.Log("If you see this.... bad");
                    break;
            }
            statcon.gameObject.SetActive(true);
            panelIcons.Add(statcon);
            realCount++;
        }

        for (int i = 0; i < living.INVENTORY.BUFFS.Count; i++ )
        {
            StatIcon statcon = totalTstatusIcons[realCount];
            switch (living.INVENTORY.BUFFS[i].BUFF)
            {
                case BuffType.none:
                    break;
                case BuffType.str:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.AtkUP];
                    statcon.myText.text = "Atk +" + living.INVENTORY.BUFFS[i].BUFFVAL + "%";
                    break;
                case BuffType.mag:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.MagUp];
                    statcon.myText.text = "Mag +" + living.INVENTORY.BUFFS[i].BUFFVAL + "%";
                    break;
                case BuffType.defense:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.DefUP];
                    statcon.myText.text = "Def +" + living.INVENTORY.BUFFS[i].BUFFVAL + "%";
                    break;
                case BuffType.resistance:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.ResUp];
                    statcon.myText.text = "Res +" + living.INVENTORY.BUFFS[i].BUFFVAL + "%";


                    break;
                case BuffType.speed:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.SpdUp];
                    statcon.myText.text = "Spd +" + living.INVENTORY.BUFFS[i].BUFFVAL + "%";


                    break;
                case BuffType.skill:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.SklUp];
                    statcon.myText.text = "Skl +" + living.INVENTORY.BUFFS[i].BUFFVAL + "%";


                    break;
                case BuffType.attack:
                    break;
                default:
                    Debug.Log("YOU DIDNT DO IT U FUCKING IDIOT");
                    break;
            }
            statcon.gameObject.SetActive(true);
            panelIcons.Add(statcon);
            realCount++;
        }

        for (int i = 0; i < living.INVENTORY.DEBUFFS.Count; i++)
        {
            StatIcon statcon = totalTstatusIcons[realCount];
            switch (living.INVENTORY.DEBUFFS[i].BUFF)
            {
                case BuffType.none:
                    break;
                case BuffType.str:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.AtkDown];
                    statcon.myText.text = "Atk " + living.INVENTORY.DEBUFFS[i].BUFFVAL + "%";
                    break;
                case BuffType.mag:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.MagDown];
                    statcon.myText.text = "Mag " + living.INVENTORY.DEBUFFS[i].BUFFVAL + "%";

                    break;
                case BuffType.defense:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.DefDown];
                    statcon.myText.text = "Def " + living.INVENTORY.DEBUFFS[i].BUFFVAL + "%";


                    break;
                case BuffType.resistance:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.ResDown];
                    statcon.myText.text = "Res " + living.INVENTORY.DEBUFFS[i].BUFFVAL + "%";


                    break;
                case BuffType.speed:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.SpdDown];
                    statcon.myText.text = "Spd " + living.INVENTORY.DEBUFFS[i].BUFFVAL + "%";


                    break;
                case BuffType.skill:
                    statcon.myImage.sprite = statusIconImages[(int)StatusIcon.SklDown];
                    statcon.myText.text = "Skl " + living.INVENTORY.DEBUFFS[i].BUFFVAL + "%";


                    break;
        
                default:
                    Debug.Log("YOU DIDNT DO IT U FUCKING IDIOT");
                    break;
            }
            statcon.gameObject.SetActive(true);
            panelIcons.Add(statcon);
            realCount++;
        }




    }
}
