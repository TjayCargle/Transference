using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public TileScript currentTile;
    public TileScript selectedTile;
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
    public AudioClip previousClip;
    bool isSetup = false;
    int soundTrack = 1;

    private int fadeMode = 0;
    private float currentVolume = 0.0f;
    private AudioClip nextClip;

    public TextObj confirmButton;
    public TextObj cancelButton;


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
            previousClip = musicClips[0];
        }
    }

    void LateUpdate()
    {
        if (manager)
        {
            if (manager.options)
            {
                if (manager.options.fixedCamera == true)
                {
                    UpdatePosition();
                }
            }
            else
            {
                UpdatePosition();
            }
        }
        else
        {
            UpdatePosition();
        }
    }

    public void UpdatePosition()
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
        else if (infoObject)
        {
            Vector3 tilePos = infoObject.transform.position;
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
        y = 12;
        z = 0;
    }

    public void SetCameraPosSlightZoom()
    {
        x = 0;
        y = 10;
        z = 0;
    }

    public void SetCameraPosZoom()
    {
        x = 0;
        y = 8;
        z = 0;
    }

    public void SetCameraPosFar()
    {
        x = 0;
        y = 13;
        z = -1;
    }

    public void PlaySoundTrack1()
    {
        if (audio)
        {
            if (soundTrack != 1)
            {
                previousClip = musicClips[soundTrack - 1];
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
                previousClip = musicClips[soundTrack - 1];
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
                previousClip = musicClips[soundTrack - 1];
                soundTrack = 3;
                audio.clip = musicClips[2];

                audio.Play();
            }
        }
    }

    public void PlaySoundTrack(int num)
    {
        if (audio)
        {
            if (num <= musicClips.Length)
            {
                if (soundTrack != num)
                {
                    currentVolume = audio.volume;
                    if (currentVolume <= 0)
                    {
                        previousClip = musicClips[soundTrack - 1];
                        soundTrack = num;
                        audio.clip = musicClips[num - 1];
                        audio.Play();
                    }
                    else
                    {
                        soundTrack = num;
                        fadeMode = -1;
                        nextClip = musicClips[num - 1];
                    }
                }
            }
        }
    }

    public void PlayPreviousSoundTrack()
    {
        if (audio)
        {
            if (previousClip != null)
            {
                currentVolume = audio.volume;
                if (currentVolume <= 0)
                {
                    audio.clip = previousClip;
                    for (int i = 0; i < musicClips.Length; i++)
                    {
                        if (previousClip == musicClips[i])
                        {
                            soundTrack = i + 1;
                            break;
                        }
                    }
                    audio.Play();
                }
                else
                {
                    for (int i = 0; i < musicClips.Length; i++)
                    {
                        if (previousClip == musicClips[i])
                        {
                            soundTrack = i + 1;
                            break;
                        }
                    }
                    fadeMode = -1;
                    nextClip = previousClip;
                }
            }
        }
    }
    private void Update()
    {
        if (fadeMode != 0)
        {
            //fade out
            if (fadeMode == -1)
            {
                if (audio.volume > 0)
                {
                    audio.volume = Mathf.Lerp(audio.volume, 0.0f, 0.1f);
                    if (audio.volume - 0.1f <= 0.1f)
                    {
                        audio.volume = 0.0f;

                        fadeMode = 1;
                        audio.clip = nextClip;
                        audio.Play();
                    }
                }
                else
                {

                    fadeMode = 1;
                    audio.clip = nextClip;
                    audio.Play();
                }
            }
            else
            {
                //fade in
                if (audio.volume < currentVolume)
                {
                    audio.volume = Mathf.Lerp(audio.volume, currentVolume, 0.1f);

                    if (audio.volume + 0.1f >= currentVolume)
                    {

                        audio.volume = currentVolume;
                        fadeMode = 0;
                    }
                }
                else
                {
                    fadeMode = 0;



                }
            }
        }
    }
    public void UpdateCamera()
    {
        TileScript updateTile = selectedTile;
        if (updateTile == null)
        {
            updateTile = currentTile;
        }
        if (updateTile)
        {
            if (infoCanvas)
            {


                if (infoText)
                {

                    if (updateTile.isOccupied)
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
                                        if (manager)
                                        {
                                            if (manager.currentState != State.PlayerEquipping)
                                            {
                                                armorSet.selectedArmor = null;
                                            }
                                        }
                                        armorSet.updateDetails();
                                    }

                                    infoText.text = liver.FullName + " - " + liver.GetClassType();//LEVEL.ToString();
                                    if (!actionText.IsActive())
                                    {
                                        //  actionText.transform.parent.gameObject.SetActive(true);
                                    }
                                    if (manager.liveEnemies.Count > 0)
                                        actionText.text = "Actions: " + liver.ACTIONS;
                                    else
                                        actionText.text = "Actions: unlimited";

                                    if (manager.liveEnemies.Count > 0)
                                    {
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
                                    }
                                    else
                                    {
                                        actionText.color = Color.blue;
                                        actionText.fontStyle = FontStyle.BoldAndItalic;
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
                                        if (liver.MAX_MANA > 0)
                                            mansSlider.value = (float)liver.MANA / (float)liver.MAX_MANA;
                                        else
                                            mansSlider.value = 0;
                                        manaText.text = liver.MANA.ToString() + "/" + liver.MAX_MANA.ToString();
                                    }
                                    if (fatigueSlider)
                                    {
                                        if (liver.MAX_FATIGUE > 0)
                                            fatigueSlider.value = (float)liver.FATIGUE / (float)liver.MAX_FATIGUE;
                                        else
                                            fatigueSlider.value = 0;
                                        fatigueText.text = liver.FATIGUE.ToString() + "/" + liver.MAX_FATIGUE.ToString();
                                    }

                                    if (manager.GetState() == State.PlayerEquipping)
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
                                        healthText.text = infoObject.STATS.HEALTH.ToString() + "/" + infoObject.BASE_STATS.MAX_HEALTH.ToString();

                                        if (attackingCheck == false)
                                        {
                                            healthSlider.value = (float)infoObject.STATS.HEALTH / (float)infoObject.BASE_STATS.MAX_HEALTH;
                                            healthText.text = (infoObject.STATS.HEALTH).ToString() + "/" + infoObject.BASE_STATS.MAX_HEALTH.ToString();

                                        }
                                        else
                                        {
                                            healthSlider.value = (float)(infoObject.STATS.HEALTH - potentialDamage) / (float)infoObject.BASE_STATS.MAX_HEALTH;
                                            healthText.text = infoObject.STATS.HEALTH + " - " + potentialDamage;
                                        }
                                    }
                                    if (mansSlider)
                                    {
                                        mansSlider.value = 0;
                                        manaText.text = "0/0";
                                    }
                                    if (fatigueSlider)
                                    {
                                        fatigueSlider.value = 0;
                                        fatigueText.text = "0/0";
                                    }
                                    infoText.text = infoObject.FullName + " LV:" + infoObject.GetComponent<BaseStats>().LEVEL.ToString();


                                    if (armorSet)
                                    {
                                        armorSet.currentGridObj = infoObject;
                                        armorSet.updateGridDetails();
                                    }
                                    if (infoObject.FACTION == Faction.eventObj)
                                    {
                                        EventDetails eve = Common.GetEventText(infoObject.BASE_STATS.DEX, null);
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
                    else
                    {
                        switch (updateTile.TTYPE)
                        {
                            case TileType.regular:
                                break;
                            case TileType.door:
                                {

                                    infoText.text = "Door to " + updateTile.MAP;
                                    actionText.text = "";
                                }
                                break;
                            case TileType.shop:
                                {

                                    infoText.text = "Shop Tile ";
                                    actionText.text = "";
                                }
                                break;
                            case TileType.help:
                                {
                                    infoText.text = "Tip";
                                    string[] extraParse = updateTile.EXTRA.Split(';');
                                    if (extraParse.Length > 1)
                                    {
                                        infoText.text += ": " + extraParse[1];
                                    }


                                    actionText.text = "";

                                }
                                break;
                            case TileType.tevent:
                                {
                                    infoText.text = "Event Tile ";
                                    actionText.text = "";
                                }
                                break;
                            case TileType.knockback:
                                {
                                    infoText.text = "Knockback Tile ";
                                    actionText.text = "Push target 1 tile";
                                }
                                break;
                            case TileType.pullin:
                                {
                                    infoText.text = "Pullback Tile ";
                                    actionText.text = "Pull target 1 tile";
                                }
                                break;
                            case TileType.swap:
                                {
                                    infoText.text = "Swap Tile ";
                                    actionText.text = "Swaps target and attacker";
                                }
                                break;
                            case TileType.reposition:
                                {
                                    infoText.text = "Reposition Tile ";
                                    actionText.text = "Attacker jumps to opposite side of target";
                                }
                                break;
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
        updateConfirmCancel();
    }

    public void updateConfirmCancel()
    {
        if (selectedTile)
        {
            if (confirmButton && cancelButton)
            {
                if (infoObject)
                {
                    confirmButton.transform.parent.gameObject.SetActive(true);
                    cancelButton.transform.parent.gameObject.SetActive(true);
                    switch (manager.currentState)
                    {
                        case State.PlayerInput:
                            {
                                if (manager.currentMenuitem)
                                {

                                    MenuItemType menuItem = (MenuItemType)manager.currentMenuitem.itemType;
                                    confirmButton.textmeshpro.text = menuItem.ToString();
                                    cancelButton.textmeshpro.text = "Cancel";
                                   
                                }
                            }
                            break;
                        case State.PlayerMove:
                            break;
                        case State.PlayerAttacking:
                            break;
                        case State.PlayerEquippingMenu:
                            break;
                        case State.PlayerEquipping:
                            break;
                        case State.playerUsingSkills:
                            break;
                        case State.PlayerEquippingSkills:
                            break;
                        case State.PlayerSkillsMenu:
                            break;

                        case State.FreeCamera:
                            {
                                if (selectedTile.isOccupied)
                                {
                                    if (infoObject.currentTile == selectedTile)
                                    {
                                        if (infoObject.FACTION == Faction.ally)
                                        {
                                            confirmButton.textmeshpro.text = "Commands";
                                            cancelButton.textmeshpro.text = "Status";
                                        }
                                        else if (infoObject.FACTION == Faction.enemy || infoObject.FACTION == Faction.hazard || infoObject.FACTION == Faction.fairy)
                                        {
                                            confirmButton.textmeshpro.text = "Mark";
                                            cancelButton.textmeshpro.text = "Status";
                                        }
                                        else
                                        {
                                            confirmButton.textmeshpro.text = "Pause";
                                            cancelButton.textmeshpro.text = "Status";
                                        }
                                    }
                                }
                                else
                                {
                                    confirmButton.textmeshpro.text = "Pause";
                                    cancelButton.textmeshpro.text = "Options";
                                }
                            }
                            break;
                        case State.PlayerSelectItem:
                            break;
                        case State.PlayerOppSelecting:
                            break;
                        case State.PlayerOppOptions:
                            break;

                        case State.ChangeOptions:
                            break;

                        case State.PlayerUsingItems:
                            break;

                        case State.PlayerAct:
                            break;
                        case State.PlayerDead:
                            break;
                        default:
                            {
                                confirmButton.transform.parent.gameObject.SetActive(false);
                                cancelButton.transform.parent.gameObject.SetActive(false);
                            }
                            break;
                    }
                }
                else
                {
                    confirmButton.textmeshpro.text = "Pause";
                    cancelButton.textmeshpro.text = "Options";
                }
            }

        }
    }
}

