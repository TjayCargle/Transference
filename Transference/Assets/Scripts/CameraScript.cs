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
    float distance = 0;
    ManagerScript manager;
    public bool showActions = true;
    // Use this for initialization
    void Start()
    {
        transform.Rotate(new Vector3(35, 5, 0));
        manager = GameObject.FindObjectOfType<ManagerScript>();
        if (manager)
        {
            manager.Setup();
        }
    }

    // Update is called once per frame
    void Update()
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
                            // if (infoCanvas.gameObject.activeInHierarchy == false)
                            //{

                            infoCanvas.gameObject.SetActive(true);
                            infoObject.gameObject.SetActive(true);
                            infoText.text = infoObject.FullName;
                            if (infoObject.GetComponent<LivingObject>())
                            {
                                LivingObject liver = infoObject.GetComponent<LivingObject>();
                                if (showActions)
                                {
                                    if (actionText)
                                    {
                                        if (!actionText.IsActive())
                                        {
                                            actionText.transform.parent.gameObject.SetActive(true);
                                        }
                                        actionText.text = "Actions: " + liver.ACTIONS;
                                    }
                                    infoText.text = infoText.text + " \n LV:" + infoObject.GetComponent<StatScript>().LEVEL.ToString();
                                }
                                else
                                {
                                    if (actionText)
                                    {
                                        if (actionText.IsActive())
                                        {
                                            actionText.transform.parent.gameObject.SetActive(false);
                                        }
                                    }

                                }
                                if (healthSlider)
                                {
                                    healthSlider.value = (float)liver.HEALTH / (float)liver.MAX_HEALTH;
                                    healthText.text = liver.HEALTH.ToString() + "/" + liver.MAX_HEALTH.ToString();
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


                                if (manager.currentState == State.FreeCamera)
                                {
                                    if (DescriptionCanvas)
                                    {

                                        if (manager.descriptionState != descState.none)
                                        {
                                            DescriptionCanvas.gameObject.SetActive(true);
                                            Text txt = DescriptionCanvas.GetComponentInChildren<Text>();
                                            string newText = "";
                                            switch (manager.descriptionState)
                                            {
                                                case descState.stats:
                                                    newText = "Stats \n";
                                                    newText += "Str: " + liver.STRENGTH + "  \t Def: " + liver.DEFENSE + "\n";
                                                    newText += "Mag: " + liver.MAGIC + " \t Res: " + liver.RESIESTANCE + "\n";
                                                    newText += "Spd: " + liver.SPEED + " \t LUK: " + liver.LUCK + "\n";
                                                    break;
                                                case descState.skills:
                                                    newText = "Skills: \n";
                                                    for (int i = 0; i < liver.BATTLE_SLOTS.SKILLS.Count; i++)
                                                    {
                                                        newText += liver.BATTLE_SLOTS.SKILLS[i].NAME + "\n";
                                                    }
                                                    break;
                                                case descState.equipped:
                                                    newText += "Weapon: " + liver.WEAPON.NAME + "\n";
                                                    newText += "Armor: " + liver.ARMOR.NAME;
                                                    break;
                                                case descState.affinities:
                                                    newText = "Armor Afinities: \n";
                                                    Element el = Element.Water;

                                                    for (int i = 0; i < liver.ARMOR.HITLIST.Count; i++)
                                                    {
                                                        el = (Element)i;
                                                        newText += el.ToString() + ": " + liver.ARMOR.HITLIST[i].ToString() + " \t ";
                                                        if (i % 2 != 0)
                                                        {
                                                            newText += "\n";
                                                        }
                                                    }
                                                    break;
                                                case descState.status:
                                                    newText = "Status: \n";
                                                    newText += "Primary: " + liver.PSTATUS + "\n";
                                                    newText += "Secondary: " + liver.SSTATUS + "\n";
                                                    newText += "Ailments: " + liver.ESTATUS;
                                                    break;
                                            }
                                            txt.text = newText;
                                            txt.resizeTextForBestFit = true;
                                        }
                                        else
                                        {
                                            DescriptionCanvas.gameObject.SetActive(false);
                                        }
                                    }

                                }

                            }
                            else if (infoObject.GetComponent<StatScript>())
                            {
                                if (healthSlider)
                                {
                                    healthSlider.value = infoObject.GetComponent<StatScript>().HEALTH / infoObject.GetComponent<StatScript>().MAX_HEALTH;
                                    healthText.text = infoObject.GetComponent<StatScript>().HEALTH.ToString() + "/" + infoObject.GetComponent<StatScript>().MAX_HEALTH.ToString();
                                }

                            }
                            //}

                        }
                        else if (infoObject == null)
                        {
                            if (infoCanvas.gameObject.activeInHierarchy)
                            {

                                infoObject = null;
                                infoText.text = "";
                                infoCanvas.gameObject.SetActive(false);
                            }

                        }
                    }

                }

            }



        }

        if (currentTile)
        {

            Vector3 tilePos = currentTile.transform.position;
            Vector3 camPos = transform.position;
            tilePos.y = 0.0f;
            camPos.y = 0.0f;
            distance = Vector3.Distance(tilePos, camPos);// Mathf.Abs(tilePos.sqrMagnitude - camPos.sqrMagnitude);
            //distance = Mathf.Sqrt(distance);
            if (distance > 0.2f)
            {
                Vector3 directionVector = (currentTile.transform.position - new Vector3(0, -5, 7)) - transform.position;
                transform.Translate(directionVector * Time.deltaTime);
            }
        }

    }
}

