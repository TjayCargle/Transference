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
    public Image faceImage;
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


    public Image[] resists;
    public Image[] elements;
    public Image[] weaknesses;
    Color transparent;
    Color opaque;
    public float smoothSpd = 0.5f;
    public ArmorSet armorSet;
    public AudioClip[] musicClips;
    AudioSource audio;
    bool isSetup = false;

    int soundTrack = 1;
    void Start()
    {
        Setup();
    }
   public void Setup()
    {
        if (!isSetup)
        {
            isSetup = true;

            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.Rotate(new Vector3(90, 0, 0));
            manager = GameObject.FindObjectOfType<ManagerScript>();
            audio = GetComponent<AudioSource>();
            SetCameraPosDefault();
            transparent = new Color(0, 0, 0, 0);
            opaque = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            if (manager)
            {
                manager.Setup();
            }
        }
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

    public void SetCameraPosDefault()
    {
        x = 0;
        y = 6;
        z = 0;
    }

    public void SetCameraPosSlightZoom()
    {
        x = 0;
        y = 5;
        z = 0;
    }

    public void SetCameraPosZoom()
    {
        x = 0;
        y = 4;
        z = 0;
    }

    public void SetCameraPosFar()
    {
        x = 0;
        y = 8;
        z = -1;
    }

    public void PlaySoundTrack1()
    {
        if (audio)
        {
            if (soundTrack != 1)
            {
                soundTrack = 1;
                audio.clip = musicClips[0];
                audio.Play();
            }
        }
    }


    public void PlaySoundTrack2()
    {
        if (audio)
        {
            if (soundTrack != 2)
            {
                soundTrack = 2;
                audio.clip = musicClips[1];
                audio.Play();
            }
        }
    }

    public void PlaySoundTrack3()
    {
        if (audio)
        {
            if (soundTrack != 3)
            {
                soundTrack = 3;
                audio.clip = musicClips[2];
                audio.Play();
            }
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
                                    // actionText.gameObject.SetActive(true);
                                }
                                if (faceImage)
                                {
                                    faceImage.sprite = infoObject.FACE;
                                    if (faceImage.sprite == null)
                                    {
                                        faceImage.color = Common.trans;
                                    }
                                    else
                                    {
                                        faceImage.color = Color.white;
                                    }
                                }
                                if (infoObject.GetComponent<LivingObject>())
                                {

                                    LivingObject liver = infoObject.GetComponent<LivingObject>();
                                    manager.iconManager.loadIconPanel(liver);

                                    if (armorSet)
                                    {
                                        armorSet.currentObj = liver;
                                        if(manager)
                                        {
                                            if(manager.currentState != State.PlayerEquipping)
                                            {
                                                armorSet.selectedArmor = null;
                                            }
                                        }
                                        armorSet.updateDetails();
                                    }

                                    infoText.text = infoObject.FullName + " LV:" + infoObject.GetComponent<BaseStats>().LEVEL.ToString();
                                    if (!actionText.IsActive())
                                    {
                                        //  actionText.transform.parent.gameObject.SetActive(true);
                                    }
                                    actionText.text = "Actions: " + liver.ACTIONS;
                                    if (liver.ACTIONS == 1)
                                    {
                                        actionText.color = Color.red;
                                        actionText.fontStyle = FontStyle.BoldAndItalic;
                                    }
                                    else
                                    {
                                        actionText.color = Color.white;
                                        actionText.fontStyle = FontStyle.Normal;
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
                                                        newText = manager.invManager.selectedMenuItem.refItem.DESC;

                                                        txt.text = newText;
                                                        txt.resizeTextForBestFit = true;

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
                                else if (infoObject.GetComponent<BaseStats>())
                                {

                                    if (healthSlider)
                                    {
                                        infoText.text = infoObject.FullName;
                                        healthText.text = infoObject.BASE_STATS.HEALTH.ToString() + "/" + infoObject.BASE_STATS.MAX_HEALTH.ToString();

                                        if (attackingCheck == false)
                                        {
                                            healthSlider.value = (float)infoObject.BASE_STATS.HEALTH / (float)infoObject.BASE_STATS.MAX_HEALTH;
                                            healthText.text = (infoObject.BASE_STATS.HEALTH).ToString() + "/" + infoObject.BASE_STATS.MAX_HEALTH.ToString();

                                        }
                                        else
                                        {
                                            healthSlider.value = (float)(infoObject.BASE_STATS.HEALTH - potentialDamage) / (float)infoObject.BASE_STATS.MAX_HEALTH;
                                            healthText.text = infoObject.BASE_STATS.HEALTH + " - " + potentialDamage;
                                        }
                                    }

                                    infoText.text = infoObject.FullName + " LV:" + infoObject.GetComponent<BaseStats>().LEVEL.ToString();


                                    if (armorSet)
                                    {
                                        armorSet.currentGridObj = infoObject;
                                        armorSet.updateGridDetails();
                                    }
                                    if (infoObject.FACTION == Faction.eventObj)
                                    {
                                        EventDetails eve = Common.GetEventText(infoObject.BASE_STATS.SPEED, null);
                                        actionText.text = eve.eventText;
                                    }
                                    else
                                    {
                                        actionText.text = "";

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
                    else if (currentTile.TTYPE == TileType.door)
                    {

                        infoText.text = "Door to " + currentTile.MAP;
                        actionText.text = "";
                    }
                    else if (currentTile.TTYPE == TileType.shop)
                    {

                        infoText.text = "Shop Tile ";
                        actionText.text = "";
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

