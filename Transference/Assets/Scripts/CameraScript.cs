using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public TileScript currentTile;
    public Canvas infoCanvas;
    public Text infoText;
    public Slider healthSlider;
    public Slider mansSlider;
    public Slider fatigueSlider;
    public Text healthText;
    public Text manaText;
    public Text fatigueText;
    public GridObject infoObject;
    public Text actionText;
    public CanvasRenderer test;
    public Canvas DescriptionCanvas;
    ManagerScript manager;
    public bool showActions = true;
    public int x = 0;
    public int y = -5;
    public int z = 7;

    public int potentialDamage = 0;
    public bool attackingCheck = false;

    public Sprite[] resSprites;
    public Sprite[] weakSprites;
    public Sprite[] attrSprites;

    public Image[] resists;
    public Image[] elements;
    public Image[] weaknesses;
    Color transparent;
    Color opaque;
    public float smoothSpd = 0.5f;
    public ArmorSet armorSet;
    void Start()
    {
        transform.Rotate(new Vector3(35, 5, 0));
        manager = GameObject.FindObjectOfType<ManagerScript>();
        if (manager)
        {
            manager.Setup();
        }
        transparent = new Color(0, 0, 0, 0);
        opaque = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    void FixedUpdate()
    {

        if (currentTile)
        {
            Vector3 tilePos = currentTile.transform.position;
            Vector3 camPos = transform.position;
            tilePos.y += y;
            tilePos.z += z;
            Vector3 targetLocation = tilePos - camPos;
            Vector3 smooth = Vector3.Lerp(transform.position, tilePos, smoothSpd);
            transform.position = smooth;

        }
    }

    public void UpdateCamera()
    {
        if (currentTile)
        {
            if (infoCanvas)
            {


                if (infoText)
                {

                    if (currentTile.isOccupied)
                    {

                        // infoCanvas.gameObject.SetActive(true);
                        if (infoObject != null)
                        {

                            if (!infoObject.GetComponent<TempObject>())
                            {

                                //if (!infoCanvas.gameObject.activeInHierarchy)
                                //{
                                //    infoCanvas.gameObject.SetActive(true);
                                //}

                                if (actionText.transform.parent.gameObject.activeInHierarchy)
                                {
                                    actionText.gameObject.SetActive(true);
                                }

                                if (infoObject.GetComponent<LivingObject>())
                                {

                                    LivingObject liver = infoObject.GetComponent<LivingObject>();

                                    if (armorSet)
                                    {
                                        armorSet.currentObj = liver;
                                        armorSet.updateDetails();
                                    }

                                    infoText.text = infoObject.FullName + " \n LV:" + infoObject.GetComponent<BaseStats>().LEVEL.ToString();
                                    if (!actionText.IsActive())
                                    {
                                        //  actionText.transform.parent.gameObject.SetActive(true);
                                    }
                                    actionText.text = "Actions: " + liver.ACTIONS;
                                    if (showActions)
                                    {


                                    }
                                    else
                                    {
                                        if (actionText)
                                        {
                                            if (actionText.IsActive())
                                            {
                                                //actionText.transform.parent.gameObject.SetActive(false);
                                            }
                                        }

                                    }
                                    if (healthSlider)
                                    {
                                        healthSlider.value = (float)(liver.HEALTH - potentialDamage) / (float)liver.MAX_HEALTH;
                                        if (attackingCheck == false)
                                        {
                                            healthText.text = (liver.HEALTH).ToString() + "/" + liver.MAX_HEALTH.ToString();

                                        }
                                        else
                                        {
                                            healthText.text = liver.HEALTH + " - " + potentialDamage;
                                        }
                                    }
                                    if (mansSlider)
                                    {
                                        mansSlider.value = (float)liver.MANA / (float)liver.MAX_MANA;
                                        manaText.text = liver.MANA.ToString() + "/" + liver.MAX_MANA.ToString();
                                    }
                                    if (fatigueSlider)
                                    {
                                        fatigueSlider.value = (float)liver.FATIGUE / (float)liver.MAX_FATIGUE;
                                        fatigueText.text = liver.FATIGUE.ToString() + "/" + liver.MAX_FATIGUE.ToString();
                                    }

                                    if (manager.currentState == State.PlayerEquipping)
                                    {
                                        if (DescriptionCanvas)
                                        {


                                            if (manager.invManager.selectedMenuItem)
                                            {
                                                if (manager.invManager.selectedMenuItem.refItem)
                                                {
                                                    //   DescriptionCanvas.gameObject.SetActive(true);
                                                    Text txt = DescriptionCanvas.GetComponentInChildren<Text>();
                                                    string newText = "";
                                                    if (txt)
                                                    {

                                                        switch (manager.invManager.selectedMenuItem.refItem.TYPE)
                                                        {
                                                            case 0:
                                                                switch (manager.descriptionState)
                                                                {

                                                                    case descState.stats:
                                                                        {
                                                                            newText = manager.invManager.selectedMenuItem.refItem.DESC;

                                                                        }
                                                                        break;
                                                                    case descState.skills:
                                                                        {

                                                                            WeaponScript wep = (manager.invManager.selectedMenuItem.refItem as WeaponScript);
                                                                            newText = "Accuracy: ";
                                                                            newText += wep.ACCURACY.ToString();
                                                                            newText += "\n Element: " + wep.AFINITY.ToString();
                                                                            newText += "\n Type: " + wep.ATTACK_TYPE.ToString();
                                                                        }
                                                                        break;

                                                                    case descState.equipped:
                                                                        {

                                                                            WeaponScript wep = (manager.invManager.selectedMenuItem.refItem as WeaponScript);
                                                                            newText = "Uses: ";
                                                                            newText += wep.USECOUNT.ToString();
                                                                            newText += "\n Level: " + wep.LEVEL.ToString();
                                                                        }
                                                                        break;

                                                                }
                                                                txt.text = newText;
                                                                txt.resizeTextForBestFit = true;
                                                        break;

                                                        case 4:
                                                            switch (manager.descriptionState)
                                                        {

                                                            case descState.stats:
                                                                {
                                                                    newText = manager.invManager.selectedMenuItem.refItem.DESC;

                                                                }
                                                                break;
                                                            case descState.skills:
                                                                {

                                                                    CommandSkill skil = (manager.invManager.selectedMenuItem.refItem as CommandSkill);
                                                                    newText = "Accuracy: ";
                                                                    newText += skil.ACCURACY.ToString();
                                                                    newText += "\n Base Damage: " + ((int)skil.DAMAGE);
                                                                    newText += "\n Side effect: " + skil.EFFECT;

                                                                }
                                                                break;

                                                            case descState.equipped:
                                                                {
                                                                    CommandSkill skil = (manager.invManager.selectedMenuItem.refItem as CommandSkill);

                                                                    newText += "Learn upgraded in skill " + skil.NEXTCOUNT + " uses";

                                                                }
                                                                break;

                                                        }
                                                        txt.text = newText;
                                                        txt.resizeTextForBestFit = true;
                                                        break;
                                                        }
                                                    }
                                                }
                                            }


                                        }
                                        else
                                        {
                                            //     DescriptionCanvas.gameObject.SetActive(false);
                                        }
                                    }
                                    else
                                    {
                                        //        DescriptionCanvas.gameObject.SetActive(false);
                                    }



                                }
                                else if (infoObject.GetComponent<StatScript>())
                                {
                                    if (healthSlider)
                                    {
                                        infoText.text = infoObject.FullName;
                                        healthSlider.value = infoObject.GetComponent<StatScript>().HEALTH / infoObject.GetComponent<StatScript>().MAX_HEALTH;
                                        healthText.text = infoObject.GetComponent<StatScript>().HEALTH.ToString() + "/" + infoObject.GetComponent<StatScript>().MAX_HEALTH.ToString();
                                    }

                                }
                            }
                            else
                            {
                                //if (actionText.gameObject.activeInHierarchy)
                                //{

                                //    actionText.gameObject.SetActive(false);
                                //}
                                //if (infoCanvas.gameObject.activeInHierarchy)
                                //{
                                //    infoCanvas.gameObject.SetActive(false);
                                //}
                                //if (DescriptionCanvas.gameObject.activeInHierarchy)
                                //{
                                //    DescriptionCanvas.gameObject.SetActive(false);
                                //}
                            }

                        }
                        else if (infoObject == null)
                        {
                            if (infoCanvas.gameObject.activeInHierarchy)
                            {

                                infoObject = null;
                                infoText.text = "";
                                //  infoCanvas.gameObject.SetActive(false);
                            }

                        }
                    }
                    else
                    {
                        //if (actionText.gameObject.activeInHierarchy)
                        //{

                        //    actionText.gameObject.SetActive(false);
                        //}
                        if (infoCanvas.gameObject.activeInHierarchy)
                        {
                            //  infoCanvas.gameObject.SetActive(false);
                        }
                        if (DescriptionCanvas.gameObject.activeInHierarchy)
                        {
                            //     DescriptionCanvas.gameObject.SetActive(false);
                        }
                    }



                }

            }







        }
        else
        {
            if (actionText.gameObject.activeInHierarchy)
            {
                // actionText.gameObject.SetActive(false);
            }
            if (infoCanvas.gameObject.activeInHierarchy)
            {
                //    infoCanvas.gameObject.SetActive(false);
            }
        }
    }
}

