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
    public int z = 0;

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

    public Camera mainCam;
    public bool moving = false;
    public float targetOrthoSize = 8.0f;
    private float editedOthoSize = 8.0f;
    private bool updateOrtho = false;

    public Slider enemyhealthSlider;
    public Slider enemymansSlider;
    public Slider enemyfatigueSlider;
    public Text enemyhealthText;
    public Text enemymanaText;
    public Text enemyfatigueText;
    public ArmorSet enemyarmorSet;
    public Text enemyactionText;
    public Image enemyfaceImage;
    public Text enemyinfoText;
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
            mainCam = GetComponent<Camera>();
        }
    }

    void FixedUpdate()
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
        if (updateOrtho == true)
        {
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, editedOthoSize, smoothSpd * Time.fixedDeltaTime);
            if (mainCam.orthographicSize == editedOthoSize)
            {
                updateOrtho = false;
            }
        }
    }
    private float hoverTime = 0.0f;
    public void UpdatePosition()
    {
        if (currentTile)
        {
            Vector3 tilePos = currentTile.transform.position;
            Vector3 camPos = transform.position;
            tilePos.x += x;
            tilePos.y += y;
            tilePos.z += z;
            //Vector3 targetLocation = tilePos - camPos;
            if (Vector3.Distance(transform.position, tilePos) > 0.5f)
            {
                hoverTime += 2 * Time.deltaTime;

                if (manager.GetState() != State.SceneRunning && manager.prevState != State.PlayerMove && manager.GetState() != State.PlayerMove)
                {
                    if (hoverTime >= 3)
                    {

                        moving = true;
                        Vector3 smooth = Vector3.Lerp(transform.position, tilePos, smoothSpd * Time.fixedDeltaTime);
                        transform.position = smooth;
                    }
                }
                else
                {
                    moving = true;
                    Vector3 smooth = Vector3.Lerp(transform.position, tilePos, smoothSpd * Time.fixedDeltaTime);
                    transform.position = smooth;
                }
            }
            else
            {
                hoverTime = 0.0f;
                moving = false;
            }

        }
        else if (infoObject)
        {
            Vector3 tilePos = infoObject.transform.position;
            Vector3 camPos = transform.position;
            tilePos.y += y;
            tilePos.z += z;
            //Vector3 targetLocation = tilePos - camPos;
            if (Vector3.Distance(transform.position, tilePos) > 0.5f)
            {
                moving = true;
                Vector3 smooth = Vector3.Lerp(transform.position, tilePos, smoothSpd * Time.fixedDeltaTime);
                transform.position = smooth;
            }
            else
            {
                moving = false;
            }
        }
    }

    public void SetCameraPosDefault()
    {

        if (mainCam)
        {
            editedOthoSize = targetOrthoSize;
            updateOrtho = true;
        }
    }

    public void SetCameraPosSlightZoom()
    {

        y = 10;
        if (mainCam)
        {
            editedOthoSize = targetOrthoSize - 0.5f;
            updateOrtho = true;
        }
    }

    public void SetCameraPosZoom()
    {

        y = 8;
        if (mainCam)
        {
            editedOthoSize = targetOrthoSize - 1.0f;
            updateOrtho = true;
        }
    }

    public void SetCameraPosOffsetZoom()
    {

        y = 6;
        if (mainCam)
        {
            editedOthoSize = targetOrthoSize + 1.0f;
            updateOrtho = true;
        }
    }

    public void SetCameraPosFar()
    {

        y = 13;
        if (mainCam)
        {
            editedOthoSize = targetOrthoSize + 3.0f;
            updateOrtho = true;
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

                                if (infoObject.GetComponent<LivingObject>())
                                {
                                    LivingObject liver = null;
                                    if (infoObject.FACTION == Faction.ally)
                                    {
                                        if (infoObject.FACTION == Faction.ally)
                                        {
                                            liver = infoObject.GetComponent<LivingObject>();
                                        }
                                        if (manager.player.current)
                                        {

                                            liver = manager.player.current;
                                        }
                                        if (manager.liveEnemies.Count > 0)
                                            actionText.text = "AP next turn: " + (3 + liver.GENERATED);
                                        infoText.text = liver.FullName + " LV: " + liver.LEVEL.ToString();
                                        if (faceImage)
                                        {
                                            faceImage.sprite = liver.FACE;
                                            if (faceImage.sprite == null)
                                            {
                                                faceImage.color = Common.trans;
                                            }
                                            else
                                            {
                                                faceImage.color = Color.white;
                                            }
                                        }

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
                                        if (healthSlider)
                                        {
                                            healthSlider.value = (float)(liver.HEALTH) / (float)liver.MAX_HEALTH;

                                            healthText.text = (liver.HEALTH).ToString() + "/" + liver.MAX_HEALTH.ToString();



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
                                    }
                                    else if (infoObject.FACTION != Faction.ally)
                                    {
                                        liver = infoObject.GetComponent<LivingObject>();
                                        enemyinfoText.text = liver.FullName + " LV: " + liver.LEVEL.ToString();
                                        if (enemyactionText != null)
                                        {
                                            if (manager.GetState() == State.EnemyTurn)
                                                enemyactionText.text = "AP next turn: " + (3 + liver.GENERATED);
                                            else
                                                enemyactionText.text = "AP next turn: " + (liver.ACTIONS);
                                        }

                                        if (enemyfaceImage)
                                        {
                                            enemyfaceImage.sprite = liver.FACE;
                                            if (enemyfaceImage.sprite == null)
                                            {
                                                enemyfaceImage.color = Common.trans;
                                            }
                                            else
                                            {
                                                enemyfaceImage.color = Color.white;
                                            }
                                        }

                                        if (enemyarmorSet)
                                        {
                                            enemyarmorSet.currentObj = liver;
                                            enemyarmorSet.selectedArmor = liver.ARMOR.SCRIPT;


                                            enemyarmorSet.updateDetails();
                                        }
                                        if (enemyhealthSlider)
                                        {
                                            enemyhealthSlider.value = (float)(liver.HEALTH - potentialDamage) / (float)liver.MAX_HEALTH;
                                            if (attackingCheck == false)
                                            {
                                                enemyhealthText.text = (liver.HEALTH).ToString() + "/" + liver.MAX_HEALTH.ToString();

                                            }
                                            else
                                            {
                                                enemyhealthText.text = liver.HEALTH + " - " + potentialDamage;
                                            }
                                        }
                                        if (enemymansSlider)
                                        {
                                            if (liver.MAX_MANA > 0)
                                                enemymansSlider.value = (float)liver.MANA / (float)liver.MAX_MANA;
                                            else
                                                enemymansSlider.value = 0;
                                            enemymanaText.text = liver.MANA.ToString() + "/" + liver.MAX_MANA.ToString();
                                        }
                                        if (fatigueSlider)
                                        {
                                            if (liver.MAX_FATIGUE > 0)
                                                enemyfatigueSlider.value = (float)liver.FATIGUE / (float)liver.MAX_FATIGUE;
                                            else
                                                enemyfatigueSlider.value = 0;
                                            enemyfatigueText.text = liver.FATIGUE.ToString() + "/" + liver.MAX_FATIGUE.ToString();
                                        }
                                    }

                                    if (manager.iconManager)
                                        manager.iconManager.loadIconPanel(liver);


                                    // " - " + liver.GetClassType();//
                                    if (!actionText.IsActive())
                                    {
                                        //  actionText.transform.parent.gameObject.SetActive(true);
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
                                    if (enemyfaceImage)
                                    {
                                        enemyfaceImage.sprite = infoObject.FACE;
                                        if (enemyfaceImage.sprite == null)
                                        {
                                            enemyfaceImage.color = Common.trans;
                                        }
                                        else
                                        {
                                            enemyfaceImage.color = Color.white;
                                        }
                                    }
                                    if (enemyhealthSlider)
                                    {
                                        enemyinfoText.text = infoObject.FullName;
                                        if (enemyinfoText.text == "Passive Coffin" || enemyinfoText.text == "PassiveCoffin")
                                        {
                                            enemyinfoText.text = "Combo Coffin";
                                        }
                                        else
                                        {
                                            enemyinfoText.text = infoObject.FullName;
                                        }
                                        enemyhealthText.text = infoObject.STATS.HEALTH.ToString() + "/" + infoObject.BASE_STATS.MAX_HEALTH.ToString();

                                        if (attackingCheck == false)
                                        {
                                            enemyhealthSlider.value = (float)infoObject.STATS.HEALTH / (float)infoObject.BASE_STATS.MAX_HEALTH;
                                            enemyhealthText.text = (infoObject.STATS.HEALTH).ToString() + "/" + infoObject.BASE_STATS.MAX_HEALTH.ToString();

                                        }
                                        else
                                        {
                                            enemyhealthSlider.value = (float)(infoObject.STATS.HEALTH - potentialDamage) / (float)infoObject.BASE_STATS.MAX_HEALTH;
                                            enemyhealthText.text = infoObject.STATS.HEALTH + " - " + potentialDamage;
                                        }
                                    }
                                    if (enemymansSlider)
                                    {
                                        enemymansSlider.value = 0;
                                        enemymanaText.text = "0/0";
                                    }
                                    if (enemyfatigueSlider)
                                    {
                                        enemyfatigueSlider.value = 0;
                                        enemyfatigueText.text = "0/0";
                                    }
                                    // + " LV:" + infoObject.BASE_STATS.LEVEL.ToString();


                                    if (enemyarmorSet)
                                    {
                                        enemyarmorSet.currentGridObj = infoObject;
                                        enemyarmorSet.updateGridDetails();
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

                                    actionText.text = "" + updateTile.MAP;
                                    infoText.text = "";
                                }
                                break;
                            case TileType.shop:
                                {

                                    actionText.text = "Shop Tile ";
                                    infoText.text = "";
                                }
                                break;
                            case TileType.help:
                                {
                                    actionText.text = "Tip";
                                    string[] extraParse = updateTile.EXTRA.Split(';');
                                    if (extraParse.Length > 1)
                                    {
                                        actionText.text += ": " + extraParse[1];
                                    }


                                    infoText.text = "";

                                }
                                break;
                            case TileType.tevent:
                                {
                                    actionText.text = "Event Tile ";
                                    infoText.text = "";
                                }
                                break;
                            case TileType.knockback:
                                {
                                    actionText.text = "Knockback Tile ";
                                    infoText.text = "Push target 1 tile";
                                }
                                break;
                            case TileType.pullin:
                                {
                                    actionText.text = "Pullback Tile ";
                                    infoText.text = "Pull target 1 tile";
                                }
                                break;
                            case TileType.swap:
                                {
                                    actionText.text = "Swap Tile ";
                                    infoText.text = "Swaps target and attacker";
                                }
                                break;
                            case TileType.reposition:
                                {
                                    actionText.text = "Reposition Tile ";
                                    infoText.text = "Attacker jumps to opposite side of target";
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

