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
    public int x = 0;
    public int y = -5;
    public int z = 7;

    public GameObject elementPanel;

    public Sprite[] resSprites;
    public Sprite[] weakSprites;
    public Sprite[] attrSprites;

    public Image[] resists;
    public Image[] elements;
    public Image[] weaknesses;
    Color transparent;
    Color opaque;
    public float smoothSpd = 0.5f;
    // Use this for initialization
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
                                            elementPanel.SetActive(false);
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
                                                case descState.mag_affinities:
                                                    //newText = "Armor Afinities: \n";
                                                    if (elementPanel)
                                                    {
                                                        if (!elementPanel.gameObject.activeInHierarchy)
                                                        {
                                                            elementPanel.SetActive(true);

                                                            Element el = Element.Water;
                                                            EHitType hitType = EHitType.normal;

                                                            elements[0].sprite = attrSprites[(int)Element.Water];
                                                            elements[1].sprite = attrSprites[(int)Element.Fire];
                                                            elements[2].sprite = attrSprites[(int)Element.Ice];
                                                            elements[3].sprite = attrSprites[(int)Element.Electric];
                                                            elements[3].color = opaque;

                                                            if (liver.ARMOR.HITLIST[(int)Element.Water] < EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Water];
                                                                resists[0].sprite = resSprites[truetype];
                                                                resists[0].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                resists[0].color = transparent;
                                                            }

                                                            if (liver.ARMOR.HITLIST[(int)Element.Fire] < EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Fire];
                                                                resists[1].sprite = resSprites[truetype];
                                                                resists[1].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                resists[1].color = transparent;

                                                            }


                                                            if (liver.ARMOR.HITLIST[(int)Element.Ice] < EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Ice];
                                                                resists[2].sprite = resSprites[truetype];
                                                                resists[2].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                resists[2].color = transparent;
                                                            }


                                                            if (liver.ARMOR.HITLIST[(int)Element.Electric] < EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Electric];
                                                                resists[3].sprite = resSprites[truetype];
                                                                resists[3].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                resists[3].color = transparent;
                                                            }


                                                            if (liver.ARMOR.HITLIST[(int)Element.Water] > EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Water];
                                                                truetype -= 5;
                                                                weaknesses[0].sprite = weakSprites[truetype];
                                                                weaknesses[0].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                weaknesses[0].color = transparent;
                                                            }

                                                            if (liver.ARMOR.HITLIST[(int)Element.Fire] > EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Fire];
                                                                truetype -= 5;
                                                                weaknesses[1].sprite = weakSprites[truetype];
                                                                weaknesses[1].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                weaknesses[1].color = transparent;

                                                            }


                                                            if (liver.ARMOR.HITLIST[(int)Element.Ice] > EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Ice];
                                                                truetype -= 5;
                                                                weaknesses[2].sprite = weakSprites[truetype];
                                                                weaknesses[2].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                weaknesses[2].color = transparent;
                                                            }


                                                            if (liver.ARMOR.HITLIST[(int)Element.Electric] > EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Electric];
                                                                truetype -= 5;
                                                                weaknesses[3].sprite = weakSprites[truetype];
                                                                weaknesses[3].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                weaknesses[3].color = transparent;
                                                            }

                                                        }
                                                    }


                                                    break;

                                                case descState.phys_affinities:
                                                    //newText = "Armor Afinities: \n";
                                                    if (elementPanel)
                                                    {
                                                        if (!elementPanel.gameObject.activeInHierarchy)
                                                        {
                                                            elementPanel.SetActive(true);

                                                            Element el = Element.Water;
                                                            EHitType hitType = EHitType.normal;

                                                            elements[0].sprite = attrSprites[(int)Element.Slash];
                                                            elements[1].sprite = attrSprites[(int)Element.Pierce];
                                                            elements[2].sprite = attrSprites[(int)Element.Blunt];
                                                            elements[3].color = transparent;
                                                            resists[3].color = transparent;
                                                            weaknesses[3].color = transparent;

                                                            if (liver.ARMOR.HITLIST[(int)Element.Slash] < EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Slash];
                                                                resists[0].sprite = resSprites[truetype];
                                                                resists[0].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                resists[0].color = transparent;
                                                            }

                                                            if (liver.ARMOR.HITLIST[(int)Element.Pierce] < EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Pierce];
                                                                resists[1].sprite = resSprites[truetype];
                                                                resists[1].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                resists[1].color = transparent;

                                                            }


                                                            if (liver.ARMOR.HITLIST[(int)Element.Blunt] < EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Blunt];
                                                                resists[2].sprite = resSprites[truetype];
                                                                resists[2].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                resists[2].color = transparent;
                                                            }


                                                            if (liver.ARMOR.HITLIST[(int)Element.Slash] > EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Slash];
                                                                truetype -= 5;
                                                                weaknesses[0].sprite = weakSprites[truetype];
                                                                weaknesses[0].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                weaknesses[0].color = transparent;
                                                            }

                                                            if (liver.ARMOR.HITLIST[(int)Element.Pierce] > EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Pierce];
                                                                truetype -= 5;
                                                                weaknesses[1].sprite = weakSprites[truetype];
                                                                weaknesses[1].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                weaknesses[1].color = transparent;

                                                            }


                                                            if (liver.ARMOR.HITLIST[(int)Element.Blunt] > EHitType.normal)
                                                            {
                                                                int truetype = (int)liver.ARMOR.HITLIST[(int)Element.Blunt];
                                                                truetype -= 5;
                                                                weaknesses[2].sprite = weakSprites[truetype];
                                                                weaknesses[2].color = opaque;
                                                            }
                                                            else
                                                            {
                                                                weaknesses[2].color = transparent;
                                                            }



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
            tilePos.y += y;
            tilePos.z += z;
            Vector3 targetLocation = tilePos - camPos;  //Vector3.Distance(tilePos, camPos);// Mathf.Abs(tilePos.sqrMagnitude - camPos.sqrMagnitude);
            Vector3 smooth = Vector3.Lerp(transform.position, tilePos, smoothSpd);
           // distance = targetLocation.sqrMagnitude;
            transform.position = smooth;

            //distance = Mathf.Sqrt(distance);
            // if (distance > 0.2f)
            {
                //Vector3 directionVector = (currentTile.transform.position - new Vector3(x, y, z)) - transform.position;
                //transform.position = currentTile.transform.position
            }
        }

    }
}

