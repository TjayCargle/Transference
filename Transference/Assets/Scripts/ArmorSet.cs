using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArmorSet : MonoBehaviour
{

    public LivingObject currentObj;
    public GridObject currentGridObj;

    [SerializeField]
    Image[] armorreacts;

    [SerializeField]
    Sprite[] armorSprites;

    [SerializeField]
    Text[] attributes;

    [SerializeField]
    Slider wardSlider;

    [SerializeField]
    Text sliderText;

    private List<EHitType> selectedHitlist;

    void Start()
    {

    }



    public void updateDetails()
    {
        if (!currentObj)
            return;
        selectedHitlist = currentObj.ARMOR.HITLIST;

        if (selectedHitlist != null)
        {
            for (int i = 0; i < selectedHitlist.Count; i++)
            {
                armorreacts[i].sprite = armorSprites[(int)selectedHitlist[i]];
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
        attributes[0].text = "Str: " + currentObj.STRENGTH;

        //val = currentObj.STATS.DEFENSE + currentObj.ARMOR.DEFENSE;
        //if (val > 0)
        //    attributes[1].text = "Def: " + currentObj.BASE_STATS.DEFENSE + " (+" + val + ")";
        //else if (val == 0)
        //    attributes[1].text = "Def: " + currentObj.BASE_STATS.DEFENSE;
        //else
        //    attributes[1].text = "Def: " + currentObj.BASE_STATS.DEFENSE + " (" + val + ")";
        attributes[1].text = "Def: " + currentObj.DEFENSE;//(currentObj.BASE_STATS.DEFENSE + currentObj.STATS.DEFENSE);

        //val = currentObj.STATS.MAGIC;
        //if (val > 0)
        //    attributes[2].text = "Mag: " + currentObj.BASE_STATS.MAGIC + " (+" + val + ")";
        //else if (val == 0)
        //    attributes[2].text = "Mag: " + currentObj.BASE_STATS.MAGIC;
        //else
        //    attributes[2].text = "Mag: " + currentObj.BASE_STATS.MAGIC + " (" + val + ")";

        attributes[2].text = "Spd: " + currentObj.SPEED;//(currentObj.BASE_STATS.SPEED + currentObj.STATS.SPEED);

        //val = currentObj.STATS.RESIESTANCE + currentObj.ARMOR.RESISTANCE;
        //if (val > 0)
        //    attributes[3].text = "Res: " + currentObj.BASE_STATS.RESIESTANCE + " (+" + val + ")";
        //else if (val == 0)
        //    attributes[3].text = "Res: " + currentObj.BASE_STATS.RESIESTANCE;
        //else
        //    attributes[3].text = "Res: " + currentObj.BASE_STATS.RESIESTANCE + " (" + val + ")";

        attributes[3].text = "Mag: " + currentObj.MAGIC;
        attributes[4].text = "Res: " + currentObj.RESIESTANCE;// (currentObj.BASE_STATS.RESIESTANCE + currentObj.STATS.RESIESTANCE);

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

        attributes[5].text = "Skl: " + currentObj.SKILL;

    }
    public void updateGridDetails()
    {
        if (!currentGridObj)
            return;
        selectedHitlist = Common.noHitList;

        if (selectedHitlist != null)
        {
            for (int i = 0; i < selectedHitlist.Count; i++)
            {
                armorreacts[i].sprite = armorSprites[(int)selectedHitlist[i]];
            }
        }


        if (wardSlider)
        {
                wardSlider.gameObject.SetActive(false);         
        }

      
        
        attributes[0].text = "Str: " + currentGridObj.BASE_STATS.STRENGTH;

        attributes[1].text = "Def: " + currentGridObj.BASE_STATS.DEFENSE;//(currentObj.BASE_STATS.DEFENSE + currentObj.STATS.DEFENSE);



        attributes[2].text = "Spd: " + currentGridObj.BASE_STATS.SPEED;//(currentObj.BASE_STATS.SPEED + currentObj.STATS.SPEED);


        attributes[3].text = "Mag: " + currentGridObj.BASE_STATS.MAGIC;
        attributes[4].text = "Res: " + currentGridObj.BASE_STATS.RESIESTANCE;// (currentObj.BASE_STATS.RESIESTANCE + currentObj.STATS.RESIESTANCE);

     

        attributes[5].text = "Skl: " + currentGridObj.BASE_STATS.SKILL;

    }
}
