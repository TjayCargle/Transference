using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventoryMangager : MonoBehaviour
{

    public GameObject menuItem;
    Dictionary<string, GameObject> tempObjects = new Dictionary<string, GameObject>();
    //  public LivingObject testLiveObject;
    public GameObject currentContent;
    public ScrollRect currentRect;
    public int currentIndex = 0;
    public int prevIndex = 0;
    public MenuItem selectedMenuItem;
    private LivingObject lastObject;
    private MenuManager menuManager;
    private ManagerScript manager;
    public GameObject extraContent;
    public ScrollRect extraRect;
    public int menuSide = -1;
    public MenuItem[] itemSlots;
    public MenuItem[] extraSlots;
    // UsableScript genericMove;
    UsableScript genericAtk;
    public int lastIndex;
    public List<UsableScript> currentList = null;
    public List<UsableScript> extraList = null;
    public int slotIndex;
    //[SerializeField]
    //  private Sprite[] imgTypes;
    [SerializeField]
    private Sprite[] attributeImages;
    [SerializeField]
    private Sprite[] dmgTriImages;
    public bool isSetup = false;
    public int attrOffset = 120;
    public int otherOffset = 100;

    [SerializeField]
    public GameObject trueExtra;

    public TextMeshProUGUI[] newdescs;
    public Image elemImg;
    public Image dmgImg;
    public Sprite[] dmgSprites;
    [SerializeField]
    private List<LivingObject> attackbleObjects = new List<LivingObject>();

    public Sprite[] ELEMENTS
    {
        get { return attributeImages; }
    }
    public Sprite[] DMGTYPES
    {
        get { return dmgTriImages; }
    }

    public void Setup()
    {
        if (!isSetup)
        {
            menuManager = GameObject.FindObjectOfType<MenuManager>();
            manager = GetComponent<ManagerScript>();
            // imgTypes = Resources.LoadAll<Sprite>("Buttons2/");
            //   genericMove = ScriptableObject.CreateInstance<UsableScript>();
            genericAtk = ScriptableObject.CreateInstance<UsableScript>();
            //genericWait = ScriptableObject.CreateInstance<UsableScript>();
            //   genericMove.NAME = "MOVE";
            genericAtk.NAME = "ATTACK";
            //genericWait.name = "WAIT";
            //  genericMove.DESC = "Allows unit to move.";
            genericAtk.DESC = "Use Strike to attack enemy";
        }
        isSetup = true;
    }
    private int GetActiveChildNumber(Transform someTransform, int idealNumber)
    {
        int num = -1;

        for (int i = 0; i < someTransform.childCount; i++)
        {
            Transform child = someTransform.GetChild(i);
            if (child.gameObject.activeInHierarchy == true)
            {
                num++;
                if (num == idealNumber)
                {
                    num = i;
                    break;
                }
            }
        }
        return num;
    }
    private void Start()
    {
        Setup();
    }


    public void SetNumAndSelect(int numToCheck)
    {
        if (currentContent)
        {
            currentIndex = currentIndex = GetActiveChildNumber(currentContent.transform, numToCheck);
            selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
            selectedMenuItem.ApplyAction(manager.player.current);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentRect)
        {
            switch (manager.currentState)
            {
                case State.PlayerInput:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();
                    }


                    break;
                case State.PlayerAllocate:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();
                    }


                    break;
                case State.PlayerEquippingMenu:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();
                    }
                    break;
                case State.PlayerEquippingSkills:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();



                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();


                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();

                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();

                    }
                    if (extraContent)
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            if (menuSide == -1)
                            {
                                EquipSkill();
                            }
                            else
                            {
                                UnequipSkill();
                            }
                        }
                        if (extraRect)
                        {
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                menuManager.switchToEquiped();
                            }
                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                menuManager.switchToExtra();
                            }
                        }
                    }
                    break;
                case State.PlayerEquipping:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();
                        manager.myCamera.UpdateCamera();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();
                        manager.myCamera.UpdateCamera();

                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();
                    }
                    if (extraContent)
                    {
                        if (extraRect)
                        {
                            if (Input.GetKeyDown(KeyCode.Return))
                            {

                            }
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                menuManager.switchToEquiped();
                                // UpdateColors(itemSlots);
                                //UpdateColors(extraSlots);
                            }
                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                menuManager.switchToExtra();

                            }
                        }
                    }
                    break;
                case State.PlayerSkillsMenu:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();

                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();
                    }
                    break;

                case State.PlayerOppOptions:
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            IncreaseScroll();

                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            DecreaseScroll();
                        }
                        if (Input.GetAxis("Mouse ScrollWheel") > 0)
                        {
                            IncreaseScroll();
                        }

                        if (Input.GetAxis("Mouse ScrollWheel") < 0)
                        {
                            DecreaseScroll();
                        }
                    }
                    break;

                case State.PlayerSelectItem:
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            IncreaseScroll();


                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            DecreaseScroll();

                        }
                        if (Input.GetAxis("Mouse ScrollWheel") > 0)
                        {
                            IncreaseScroll();
                        }

                        if (Input.GetAxis("Mouse ScrollWheel") < 0)
                        {
                            DecreaseScroll();
                        }
                    }
                    break;
                case State.playerUsingSkills:
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            IncreaseScroll();


                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            DecreaseScroll();

                        }
                        if (Input.GetAxis("Mouse ScrollWheel") > 0)
                        {
                            IncreaseScroll();
                        }

                        if (Input.GetAxis("Mouse ScrollWheel") < 0)
                        {
                            DecreaseScroll();
                        }
                    }
                    break;
                case State.PlayerUsingItems:
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            IncreaseScroll();


                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            DecreaseScroll();

                        }
                        if (Input.GetAxis("Mouse ScrollWheel") > 0)
                        {
                            IncreaseScroll();
                        }

                        if (Input.GetAxis("Mouse ScrollWheel") < 0)
                        {
                            DecreaseScroll();
                        }
                    }
                    break;

                case State.EventRunning:
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            IncreaseScroll();

                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            DecreaseScroll();
                        }
                        if (Input.GetAxis("Mouse ScrollWheel") > 0)
                        {
                            IncreaseScroll();
                        }

                        if (Input.GetAxis("Mouse ScrollWheel") < 0)
                        {
                            DecreaseScroll();
                        }


                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            if (selectedMenuItem)
                            {
                                selectedMenuItem.ApplyAction(lastObject);
                            }
                        }

                    }
                    break;
            }
            manager.currentMenuitem = selectedMenuItem;
        }

    }
    public void IncreaseScroll()
    {
        if (currentRect)
        {
            if (currentContent)
            {

                Vector2 newPos = currentRect.normalizedPosition;
                newPos.y += 0.01f;

                if (currentContent.transform.childCount > 0)
                {
                    if (selectedMenuItem)
                    {
                        Text selectedText = selectedMenuItem.GetComponentInChildren<Text>();
                        TextMeshProUGUI proText = selectedMenuItem.GetComponentInChildren<TextMeshProUGUI>();
                        if (selectedText)
                        {
                            SetNormal(selectedText);
                        }
                        if (proText)
                        {
                            SetNormal(proText);
                        }
                    }
                    currentIndex--;

                    if (extraContent.activeInHierarchy)
                    {
                        if (menuSide == -1)
                            if (currentIndex == 0 && slotIndex > 5)
                            {
                                ShiftDown();
                                currentIndex++;
                            }

                        if (currentIndex < 0)
                        {
                            currentIndex = Mathf.Min(5, currentList.Count - 1);
                            ShowEnd();
                            if (menuSide == -1)
                                slotIndex = currentList.Count - 1;
                            else
                                slotIndex = extraList.Count - 1;
                        }
                    }
                    else if (currentIndex < 0)
                    {
                        currentIndex = currentList.Count - 1;
                    }

                    if (currentIndex < 0)
                    {
                        currentIndex = currentContent.transform.childCount - 1;
                    }

                    if (currentIndex < currentContent.transform.childCount)
                    {
                        if (currentContent.transform.GetChild(currentIndex))
                        {

                            MenuItem hoverItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                            SetSelectedAndUpdateMovement(hoverItem);
                            if (selectedMenuItem)
                            {
                                if (selectedMenuItem.gameObject.activeInHierarchy)
                                {

                                    Text selectedText = selectedMenuItem.GetComponentInChildren<Text>();
                                    TextMeshProUGUI proText = selectedMenuItem.GetComponentInChildren<TextMeshProUGUI>();
                                    if (selectedText)
                                    {
                                        SetSelected(selectedText);
                                    }
                                    if (proText)
                                    {
                                        SetSelected(proText);
                                    }
                                }
                                else
                                {
                                    IncreaseScroll();
                                }

                            }
                        }

                    }
                    float index = currentIndex / (float)currentContent.transform.childCount;
                    newPos.y = 1.0f - index;

                }
                if (newPos.y > 1)
                    newPos.y = 1;
                // currentRect.normalizedPosition = newPos;

                if (menuManager)
                {
                    if (menuManager.DESC)
                    {


                        if (selectedMenuItem)
                        {
                            if (selectedMenuItem.refItem)
                            {
                                menuManager.DESC.text = selectedMenuItem.refItem.DESC;
                                if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                                {
                                    CommandSkill cmd = selectedMenuItem.refItem as CommandSkill;
                                    manager.ShowSkillAttackbleTiles(manager.player.current, cmd);

                                    UpdateDescriptions(cmd);
                                }
                                if (selectedMenuItem.refItem.GetType() == typeof(WeaponScript))
                                {
                                    WeaponScript wep = selectedMenuItem.refItem as WeaponScript;

                                    manager.ShowWeaponAttackbleTiles(manager.player.current, wep);
                                    newdescs[0].transform.parent.parent.gameObject.SetActive(true);
                                    UpdateDescriptions(wep);
                                }
                                if (selectedMenuItem.refItem.GetType() == typeof(OppSkill))
                                {
                                    OppSkill opp = selectedMenuItem.refItem as OppSkill;

                                    UpdateDescriptions(opp);
                                }
                                if (selectedMenuItem.refItem.GetType() == typeof(ItemScript))
                                {
                                    manager.ShowItemAttackbleTiles(manager.player.current, (selectedMenuItem.refItem as ItemScript));
                                }


                            }
                        }
                    }
                }
            }
        }

    }
    public void DecreaseScroll()
    {
        if (currentRect)
        {
            if (currentContent)
            {
                if (selectedMenuItem)
                {
                    Text selectedText = selectedMenuItem.GetComponentInChildren<Text>();
                    TextMeshProUGUI proText = selectedMenuItem.GetComponentInChildren<TextMeshProUGUI>();
                    if (selectedText)
                    {
                        SetNormal(selectedText);
                    }
                    if (proText)
                    {
                        SetNormal(proText);
                    }
                }
                Vector2 newPos = currentRect.normalizedPosition;
                newPos.y -= 0.01f;

                if (currentContent.transform.childCount > 0)
                {
                    if (selectedMenuItem)
                    {
                        Text selectedText = selectedMenuItem.GetComponentInChildren<Text>();
                        TextMeshProUGUI proText = selectedMenuItem.GetComponentInChildren<TextMeshProUGUI>();
                        if (selectedText)
                        {
                            SetNormal(selectedText);
                        }
                        if (proText)
                        {
                            SetNormal(proText);
                        }

                    }
                    currentIndex++;
                    if (extraContent.activeInHierarchy)
                    {
                        if (menuSide == -1)
                            if (currentIndex == 5 && slotIndex < currentList.Count - 1)
                            {
                                ShiftUp();
                                currentIndex--;
                            }

                        if (currentIndex > 5)
                        {
                            currentIndex = 0;
                            ShowBegining();
                            slotIndex = 5;
                        }
                        if (currentIndex >= currentList.Count)
                        {
                            currentIndex = 0;
                            ShowBegining();
                            slotIndex = 5;
                        }
                    }
                    else if (currentList.Count > 0)
                    {
                        if (currentIndex > currentList.Count - 1)
                        {
                            currentIndex = 0;

                        }

                    }
                    if (currentIndex > currentContent.transform.childCount - 1)
                    {
                        currentIndex = 0;
                    }

                    MenuItem hoverItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                    SetSelectedAndUpdateMovement(hoverItem);

                    if (selectedMenuItem)
                    {
                        if (selectedMenuItem.gameObject.activeInHierarchy)
                        {

                            Text selectedText = selectedMenuItem.GetComponentInChildren<Text>();
                            TextMeshProUGUI proText = selectedMenuItem.GetComponentInChildren<TextMeshProUGUI>();
                            if (selectedText)
                            {
                                SetSelected(selectedText);
                            }
                            if (proText)
                            {
                                SetSelected(proText);
                            }
                        }
                        else
                        {
                            DecreaseScroll();
                        }
                    }

                    float index = currentIndex / (float)(currentContent.transform.childCount - 1);
                    newPos.y = 1.0f - index;

                }
                if (newPos.y < 0)
                    newPos.y = 0;
                // currentRect.normalizedPosition = newPos;

                if (menuManager)
                {
                    if (menuManager.DESC)
                    {


                        if (selectedMenuItem)
                        {
                            if (selectedMenuItem.refItem)
                            {
                                menuManager.DESC.text = selectedMenuItem.refItem.DESC;
                                if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                                {
                                    CommandSkill cmd = selectedMenuItem.refItem as CommandSkill;
                                    manager.ShowSkillAttackbleTiles(manager.player.current, cmd);

                                    UpdateDescriptions(cmd);
                                }

                                if (selectedMenuItem.refItem.GetType() == typeof(WeaponScript))
                                {
                                    WeaponScript wep = selectedMenuItem.refItem as WeaponScript;

                                    manager.ShowWeaponAttackbleTiles(manager.player.current, wep);
                                    newdescs[0].transform.parent.parent.gameObject.SetActive(true);
                                    UpdateDescriptions(wep);
                                }
                                if (selectedMenuItem.refItem.GetType() == typeof(OppSkill))
                                {
                                    OppSkill opp = selectedMenuItem.refItem as OppSkill;

                                    UpdateDescriptions(opp);
                                }

                                if (selectedMenuItem.refItem.GetType() == typeof(ItemScript))
                                {
                                    manager.ShowItemAttackbleTiles(manager.player.current, (selectedMenuItem.refItem as ItemScript));
                                }
                            }
                        }
                    }
                }
            }
        }


    }
    private void SetNormal(Text text)
    {
        text.color = Color.white;
        selectedMenuItem.GetComponent<Image>().color = Color.black;
        Vector2 pos = text.GetComponent<RectTransform>().localPosition;
        pos.x = 0;
        text.GetComponent<RectTransform>().localPosition = pos;
    }
    private void SetNormal(TextMeshProUGUI text)
    {
        text.color = Color.white;
        selectedMenuItem.GetComponent<Image>().color = Color.black;
        Vector2 pos = text.GetComponent<RectTransform>().localPosition;
        pos.x = 0;
        text.GetComponent<RectTransform>().localPosition = pos;
    }
    private void SetSelected(Text text)
    {
        text.color = Color.yellow;
        selectedMenuItem.GetComponent<Image>().color = Color.yellow;
        Vector2 pos = text.GetComponent<RectTransform>().localPosition;
        pos.x = 15;
        text.GetComponent<RectTransform>().localPosition = pos;
    }
    private void SetSelected(TextMeshProUGUI text)
    {
        text.color = Color.yellow;
        selectedMenuItem.GetComponent<Image>().color = Color.yellow;
        Vector2 pos = text.GetComponent<RectTransform>().localPosition;
        pos.x = 15;
        text.GetComponent<RectTransform>().localPosition = pos;
    }
    public void HoverSelect(MenuItem hoveritem, GameObject content)
    {
        if (currentRect)
        {
            if (currentContent)
            {
                if (selectedMenuItem)
                {

                    SupportText support = selectedMenuItem.GetComponentInChildren<SupportText>();
                    Text selectedText = selectedMenuItem.GetComponentInChildren<Text>();
                    TextMeshProUGUI proText = selectedMenuItem.GetComponentInChildren<TextMeshProUGUI>();
                    Image attr = null;
                    if (support)
                    {
                        if (support.isSetup & support.isVisible)
                        {
                            if (support.freemove)
                            {
                                support.freemove.upDown = false;
                                support.freemove.running = false;
                            }
                        }
                    }
                    if (selectedMenuItem.GetComponentsInChildren<Image>().Length > 2)
                    {

                        attr = selectedMenuItem.GetComponentsInChildren<Image>()[2];
                    }
                    Vector2 pos;
                    selectedMenuItem.GetComponent<Image>().color = Color.black;
                    if (selectedText)
                    {
                        selectedText.color = Color.white;
                        pos = selectedText.GetComponent<RectTransform>().localPosition;
                        pos.x = 0;
                        selectedText.GetComponent<RectTransform>().localPosition = pos;
                    }
                    if (proText)
                    {
                        proText.color = Color.white;
                        pos = proText.GetComponent<RectTransform>().localPosition;
                        pos.x = 0;
                        proText.GetComponent<RectTransform>().localPosition = pos;
                    }
                    if (attr)
                    {
                        pos = attr.GetComponent<RectTransform>().localPosition;
                        pos.x = -128;
                        attr.GetComponent<RectTransform>().localPosition = pos;
                    }

                    SetSelectedAndUpdateMovement(hoveritem);

                    selectedText = selectedMenuItem.GetComponentInChildren<Text>();
                    proText = selectedMenuItem.GetComponentInChildren<TextMeshProUGUI>();
                    attr = null;
                    support = selectedMenuItem.GetComponentInChildren<SupportText>();

                    if (support)
                    {
                        if (support.isSetup & support.isVisible)
                        {
                            if (support.freemove)
                            {
                                support.freemove.upDown = false;
                                support.freemove.running = true;
                            }
                        }
                    }

                    if (selectedMenuItem.GetComponentsInChildren<Image>().Length > 2)
                    {
                        attr = selectedMenuItem.GetComponentsInChildren<Image>()[2];
                    }
                    currentContent = content;
                    currentIndex = currentContent.transform.GetSiblingIndex();
                    if (trueExtra)
                    {

                        if (currentContent == trueExtra)
                        {

                            menuSide = 1;
                        }
                        else
                        {
                            menuSide = -1;
                        }
                    }


                    selectedMenuItem.GetComponent<Image>().color = Color.yellow;

                    if (selectedText)
                    {
                        selectedText.color = Color.yellow;
                        pos = selectedText.GetComponent<RectTransform>().localPosition;
                        pos.x = 15;
                        selectedText.GetComponent<RectTransform>().localPosition = pos;
                    }
                    if (proText)
                    {
                        proText.color = Color.yellow;
                        pos = proText.GetComponent<RectTransform>().localPosition;
                        pos.x = 15;
                        proText.GetComponent<RectTransform>().localPosition = pos;
                    }
                    if (attr)
                    {
                        pos = attr.GetComponent<RectTransform>().localPosition;
                        pos.x = -113;
                        attr.GetComponent<RectTransform>().localPosition = pos;
                    }
                }

                if (menuManager)
                {
                    if (menuManager.DESC)
                    {


                        if (selectedMenuItem)
                        {
                            if (selectedMenuItem.refItem)
                            {

                                menuManager.DESC.text = selectedMenuItem.refItem.DESC;
                                if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                                {
                                    CommandSkill cmd = selectedMenuItem.refItem as CommandSkill;
                                    manager.ShowSkillAttackbleTiles(manager.player.current, cmd);
                                    newdescs[0].transform.parent.parent.gameObject.SetActive(true);
                                    UpdateDescriptions(cmd);

                                }
                                else if (selectedMenuItem.refItem.GetType() == typeof(WeaponScript))
                                {
                                    WeaponScript wep = selectedMenuItem.refItem as WeaponScript;
                                    manager.ShowWeaponAttackbleTiles(manager.player.current, (selectedMenuItem.refItem as WeaponScript));
                                    newdescs[0].transform.parent.parent.gameObject.SetActive(true);
                                    UpdateDescriptions(wep);
                                }
                                else if (selectedMenuItem.refItem.GetType() == typeof(OppSkill))
                                {
                                    OppSkill opp = selectedMenuItem.refItem as OppSkill;
                                    newdescs[0].transform.parent.parent.gameObject.SetActive(true);
                                    UpdateDescriptions(opp);
                                }
                                else
                                {
                                    newdescs[0].transform.parent.parent.gameObject.SetActive(false);
                                }

                                if (selectedMenuItem.refItem.GetType() == typeof(ArmorScript))
                                {
                                    manager.myCamera.armorSet.selectedArmor = selectedMenuItem.refItem as ArmorScript;
                                    manager.myCamera.armorSet.updateDetails();
                                }

                                if (selectedMenuItem.refItem.GetType() == typeof(ItemScript))
                                {
                                    manager.ShowItemAttackbleTiles(manager.player.current, (selectedMenuItem.refItem as ItemScript));
                                }
                            }
                            else
                            {
                                newdescs[0].transform.parent.parent.gameObject.SetActive(false);
                                manager.menuManager.DESC.text = "";
                                manager.myCamera.armorSet.selectedArmor = null;
                                manager.myCamera.armorSet.updateDetails();
                            }

                        }
                    }
                }
                manager.currentMenuitem = selectedMenuItem;
            }
            else
            {
                Debug.Log("no content!");
            }
        }
        else
        {
            Debug.Log("no rekt!");
        }
    }
    public void TurnOffNewDesc()
    {
        newdescs[0].transform.parent.parent.gameObject.SetActive(false);
    }
    private void SetSelectedAndUpdateMovement(MenuItem hoveritem)
    {
        if (!hoveritem)
        {
            return;
        }
        selectedMenuItem = hoveritem;

        return;
        if (selectedMenuItem)
        {

            if (selectedMenuItem.GetComponent<FreeMovement>())
            {
                selectedMenuItem.GetComponent<FreeMovement>().running = false;
                selectedMenuItem.transform.position = selectedMenuItem.GetComponent<FreeMovement>().truePosition;
            }
        }


        if (!selectedMenuItem.GetComponent<FreeMovement>())
        {
            selectedMenuItem.gameObject.AddComponent<FreeMovement>();
            selectedMenuItem.GetComponent<FreeMovement>().truePosition = selectedMenuItem.transform.position;

        }

        if (selectedMenuItem.GetComponent<FreeMovement>())
        {
            selectedMenuItem.GetComponent<FreeMovement>().running = true;
        }
    }
    private void UpdateDescriptions(CommandSkill cmd)
    {
        if (newdescs.Length == 6)
        {
            if (cmd.ETYPE == EType.physical)
            {
                newdescs[0].text = "<sprite=0> \n Physical";
            }
            else
            {
                newdescs[0].text = "<sprite=1> \n Magical";
            }
            int indxex = (int)cmd.ELEMENT;
            elemImg.sprite = attributeImages[indxex];

            newdescs[1].text = cmd.ELEMENT.ToString();
            newdescs[2].text = cmd.ACCURACY.ToString() + "% \n Accuracy";

            if (cmd.ELEMENT <= Element.Force)
            {
                dmgImg.color = Color.white;
                dmgImg.sprite = dmgSprites[Common.GetDmgIndex(cmd.DAMAGE) - 1];
                newdescs[3].text = "" + cmd.DAMAGE.ToString() + " damage";
            }
            else
            {
                dmgImg.color = Common.trans;
                switch (cmd.ELEMENT)
                {

                    case Element.Buff:
                        {
                            if (cmd.SUBTYPE == SubSkillType.Debuff)
                            {
                                newdescs[3].text = cmd.BUFF.ToString() + " -" + cmd.BUFFVAL + "%";
                            }
                            else
                            {
                                newdescs[3].text = cmd.BUFF.ToString() + " +" + cmd.BUFFVAL + "%";
                            }
                        }
                        break;
                    case Element.Support:
                        newdescs[3].text = "";
                        break;
                    case Element.Ailment:
                        newdescs[3].text = "causes " + cmd.EFFECT.ToString();
                        break;
                    default:
                        newdescs[3].text = "";
                        break;
                }

            }
            if (cmd.MAX_HIT > 1)
            {
                newdescs[4].text = "Hits " + cmd.MIN_HIT + " - " + cmd.MAX_HIT + " times.";
            }
            else if (cmd.HITS > 1)
            {
                newdescs[4].text = "Hits " + cmd.HITS.ToString() + " times.";
            }
            else
            {
                newdescs[4].text = "";
            }

            if (cmd.EFFECT != SideEffect.none)
            {
                switch (cmd.EFFECT)
                {

                    case SideEffect.heal:
                        break;
                    case SideEffect.barrier:
                        break;
                    case SideEffect.knockback:
                        {
                            newdescs[4].text += " Pushes target 1 tile";
                        }
                        break;
                    case SideEffect.pullin:
                        {
                            newdescs[4].text += " Pulls target 1 tile";
                        }
                        break;
                    case SideEffect.pushforward:
                        {
                            newdescs[4].text += " Push self and target 1 tile";
                        }
                        break;
                    case SideEffect.pullback:
                        {
                            newdescs[4].text += " Pull self and target 1 tile";
                        }
                        break;
                    case SideEffect.jumpback:
                        {
                            newdescs[4].text += " Pull self away 1 tile";
                        }
                        break;
                    case SideEffect.reposition:
                        {
                            newdescs[4].text += " Jumps over target";
                        }
                        break;
                    case SideEffect.swap:
                        {
                            newdescs[4].text = "Swap locations with target.";
                        }
                        break;
                    default:
                        {
                            if (cmd.SUBTYPE == SubSkillType.Ailment)
                            {
                                //newdescs[4].text += "" + cmd.ACCURACY + "% chance of " + cmd.EFFECT.ToString() + ".";

                            }
                            else
                            {
                                newdescs[4].text += "" + ((cmd.OWNER.MAGIC * 0.5f) + cmd.LEVEL).ToString() + "% chance of " + cmd.EFFECT.ToString() + ".";

                            }

                        }
                        break;
                }

            }
            if (cmd.SPECIAL_EVENTS.Count > 0)
            {
                for (int i = 0; i < cmd.SPECIAL_EVENTS.Count; i++)
                {
                    SkillEventContainer sec = cmd.SPECIAL_EVENTS[i];
                    newdescs[4].text += Common.GetSkillEventText(sec.theEvent, sec.theReaction);

                    if (i + 1 < cmd.SPECIAL_EVENTS.Count)
                    {
                        newdescs[4].text += "\n";
                    }
                }
            }
            //else
            //{
            //    newdescs[5].text = " ";
            //}
        }
    }
    private void UpdateDescriptions(WeaponScript cmd)
    {
        if (newdescs.Length == 6)
        {
            newdescs[0].text = "<sprite=4> \n Natural";
            int indxex = (int)cmd.ELEMENT;
            elemImg.sprite = attributeImages[indxex];

            newdescs[1].text = cmd.ELEMENT.ToString();
            newdescs[2].text = cmd.ACCURACY.ToString() + "% \n Accuracy";

            if (cmd.ELEMENT <= Element.Force)
            {
                dmgImg.color = Color.white;
                dmgImg.sprite = dmgSprites[Common.GetDmgIndex(cmd.ATTACK) - 1];
                newdescs[3].text = "" + cmd.ATTACK.ToString() + " damage";
            }
            else
            {
                dmgImg.color = Common.trans;
                switch (cmd.ELEMENT)
                {

                    default:
                        newdescs[3].text = "";
                        break;
                }

            }

            newdescs[4].text = " May trigger an Auto Skill.";

            // if(cmd.EFFECT != SideEffect.none)
            {
                newdescs[5].text = "";// + cmd.BOOST + " + " + cmd.BOOSTVAL + " when equipped";
            }
        }
    }

    private void UpdateDescriptions(OppSkill opp)
    {
        if (newdescs.Length == 6)
        {
            if (opp.SUBTYPE == SubSkillType.Skill)
            {
                newdescs[0].text = "<sprite=0> \n Physical";
            }
            else
            {
                newdescs[0].text = "<sprite=1> \n Magical";
            }
            int indxex = (int)opp.REACTION;
            elemImg.sprite = attributeImages[indxex];

            newdescs[1].text = opp.REACTION.ToString();
            newdescs[2].text = "100% \n Accuracy";

            if (opp.REACTION <= Element.Force)
            {
                dmgImg.color = Color.white;
                dmgImg.sprite = dmgSprites[Common.GetDmgIndex(opp.DAMAGE)];
                newdescs[3].text = "" + opp.DAMAGE.ToString() + " damage";
            }
            else
            {
                dmgImg.color = Common.trans;
                switch (opp.DAMAGE)
                {

                    default:
                        newdescs[3].text = "";
                        break;
                }

            }

            newdescs[4].text = "Hits \n 1";

            // if(cmd.EFFECT != SideEffect.none)
            {
                newdescs[5].text = "";// + cmd.BOOST + " + " + cmd.BOOSTVAL + " when equipped";
            }
        }
    }
    private void ShiftUp()
    {
        if (currentList.Count > 0)
        {
            //   Debug.Log("shift up");
            for (int i = 0; i < 5; i++)
            {
                itemSlots[i].refItem = itemSlots[i + 1].refItem;
                // itemSlots[i].GetComponentInChildren<Text>().text = itemSlots[i + 1].refItem.NAME;
                if (itemSlots[i].GetComponentInChildren<Text>())
                    itemSlots[i].GetComponentInChildren<Text>().text = itemSlots[i + 1].refItem.NAME;
                else if (itemSlots[i].GetComponentInChildren<TextMeshProUGUI>())
                    itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = itemSlots[i + 1].refItem.NAME;

            }
            slotIndex++;

            itemSlots[5].refItem = currentList[slotIndex];
            //   itemSlots[5].GetComponentInChildren<Text>().text = currentList[slotIndex].NAME;
            if (itemSlots[5].GetComponentInChildren<Text>())
                itemSlots[5].GetComponentInChildren<Text>().text = itemSlots[slotIndex].refItem.NAME;
            else if (itemSlots[5].GetComponentInChildren<TextMeshProUGUI>())
                itemSlots[5].GetComponentInChildren<TextMeshProUGUI>().text = currentList[slotIndex].NAME;
            // UpdateColors(itemSlots);
            // UpdateColors(extraSlots);
        }
    }
    private void ShiftDown()
    {
        if (menuSide == -1)
        {

            if (currentList.Count > 0)
            {
                if (slotIndex > 5)
                {
                    //Debug.Log("shift down");
                    for (int i = 5; i > 0; i--)
                    {
                        // Debug.Log("i = " + i);
                        itemSlots[i].refItem = itemSlots[i - 1].refItem;
                        //   itemSlots[i].GetComponentInChildren<Text>().text = itemSlots[i - 1].refItem.NAME;
                        if (itemSlots[i].GetComponentInChildren<Text>())
                            itemSlots[i].GetComponentInChildren<Text>().text = itemSlots[i - 1].refItem.NAME;
                        else if (itemSlots[i].GetComponentInChildren<TextMeshProUGUI>())
                            itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = itemSlots[i - 1].refItem.NAME;
                    }
                    slotIndex--;

                    itemSlots[0].refItem = currentList[slotIndex - 5];
                    //  itemSlots[0].GetComponentInChildren<Text>().text = itemSlots[0].refItem.NAME;
                    if (itemSlots[0].GetComponentInChildren<Text>())
                        itemSlots[0].GetComponentInChildren<Text>().text = itemSlots[0].refItem.NAME;
                    else if (itemSlots[0].GetComponentInChildren<TextMeshProUGUI>())
                        itemSlots[0].GetComponentInChildren<TextMeshProUGUI>().text = itemSlots[0].refItem.NAME;

                }
            }
            // UpdateColors(itemSlots);
            // UpdateColors(extraSlots);
        }
    }
    private void ShowBegining()
    {
        if (menuSide == -1)
        {

            for (int useCount = 0; useCount < 6; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
            {
                MenuItem selectableItem = itemSlots[useCount];
                if (useCount < currentList.Count)
                {
                    UsableScript item = currentList[useCount];
                    selectableItem.refItem = item;
                    if (selectableItem.GetComponentInChildren<Text>())
                        selectableItem.GetComponentInChildren<Text>().text = item.NAME;
                    else if (selectableItem.GetComponentInChildren<TextMeshProUGUI>())
                        selectableItem.GetComponentInChildren<TextMeshProUGUI>().text = item.NAME;
                }
            }
            slotIndex = 5;
        }
        //UpdateColors(itemSlots);
        //UpdateColors(extraSlots);
    }

    private void ShowEnd()
    {
        if (menuSide == -1)
        {

            int realIndex = currentList.Count - 6;
            if (realIndex < 0)
            {
                realIndex = 0;
            }
            for (int useCount = 0; useCount < 6; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
            {
                MenuItem selectableItem = itemSlots[useCount];
                if (useCount < currentList.Count)
                {

                    UsableScript item = currentList[realIndex];
                    selectableItem.refItem = item;
                    if (selectableItem.GetComponentInChildren<Text>())
                        selectableItem.GetComponentInChildren<Text>().text = item.NAME;
                    else if (selectableItem.GetComponentInChildren<TextMeshProUGUI>())
                        selectableItem.GetComponentInChildren<TextMeshProUGUI>().text = item.NAME;
                    realIndex++;
                }
            }
            slotIndex = currentList.Count - 1;
        }
        // UpdateColors(itemSlots);
        // UpdateColors(extraSlots);
    }


    public void ForceSelect()
    {
        if (currentContent)
        {

            for (int i = 0; i < currentContent.transform.childCount; i++)
            {
                MenuItem menuItem = currentContent.transform.GetChild(i).GetComponent<MenuItem>();
                if (menuItem)
                {
                    if (menuItem.gameObject.activeInHierarchy)
                    {

                        Text selectedText = menuItem.GetComponentInChildren<Text>();
                        TextMeshProUGUI proText = menuItem.GetComponentInChildren<TextMeshProUGUI>();
                        if (selectedText)
                        {
                            selectedText.color = Color.white;
                            menuItem.GetComponent<Image>().color = Color.black;
                            Vector2 pos = selectedText.GetComponent<RectTransform>().localPosition;
                            pos.x = 0;
                        }

                        if (proText)
                        {
                            proText.color = Color.white;
                            menuItem.GetComponent<Image>().color = Color.black;
                            Vector2 pos = proText.GetComponent<RectTransform>().localPosition;
                            pos.x = 0;
                        }
                    }
                }
            }
            Text selectedText2 = menuItem.GetComponentInChildren<Text>();
            TextMeshProUGUI proText2 = menuItem.GetComponentInChildren<TextMeshProUGUI>();
            if (selectedMenuItem)
            {
                if (selectedMenuItem.gameObject.activeInHierarchy)
                {

                    selectedMenuItem.GetComponent<Image>().color = Color.black;
                    if (selectedText2)
                    {
                        selectedText2.color = Color.white;
                        Vector2 pos = selectedText2.GetComponent<RectTransform>().localPosition;
                        pos.x = 0;
                        selectedText2.GetComponent<RectTransform>().localPosition = pos;
                    }

                    if (proText2)
                    {
                        proText2.color = Color.white;
                        Vector2 pos = proText2.GetComponent<RectTransform>().localPosition;
                        pos.x = 0;
                        proText2.GetComponent<RectTransform>().localPosition = pos;
                    }

                }

            }
            if (currentContent.transform.childCount > 0)
            {
                if (currentIndex < currentContent.transform.childCount)
                {

                    if (currentContent.transform.GetChild(currentIndex).gameObject.activeInHierarchy)
                    {

                        MenuItem hoverItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                        SetSelectedAndUpdateMovement(hoverItem);
                        selectedMenuItem.GetComponent<Image>().color = Color.yellow;
                        selectedText2 = menuItem.GetComponentInChildren<Text>();
                        proText2 = menuItem.GetComponentInChildren<TextMeshProUGUI>();
                        if (selectedText2)
                        {
                            selectedText2.color = Color.yellow;
                            Vector2 pos = selectedText2.GetComponent<RectTransform>().localPosition;
                            pos.x = 15;
                            selectedText2.GetComponent<RectTransform>().localPosition = pos;
                        }

                        if (proText2)
                        {
                            proText2.color = Color.yellow;
                            Vector2 pos = proText2.GetComponent<RectTransform>().localPosition;
                            pos.x = 15;
                            proText2.GetComponent<RectTransform>().localPosition = pos;
                        }

                    }
                    else
                    {
                        currentIndex = 0;
                        for (int i = 0; i < currentContent.transform.childCount; i++)
                        {
                            MenuItem hoverItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                            SetSelectedAndUpdateMovement(hoverItem);
                            if (selectedMenuItem.gameObject.activeInHierarchy)
                            {
                                break;
                            }
                            currentIndex++;
                        }

                        selectedMenuItem.GetComponent<Image>().color = Color.yellow;
                        selectedText2 = menuItem.GetComponentInChildren<Text>();
                        proText2 = menuItem.GetComponentInChildren<TextMeshProUGUI>();
                        if (selectedText2)
                        {
                            selectedText2.color = Color.yellow;
                            Vector2 pos = selectedText2.GetComponent<RectTransform>().localPosition;
                            pos.x = 15;
                            selectedText2.GetComponent<RectTransform>().localPosition = pos;
                        }

                        if (proText2)
                        {
                            proText2.color = Color.yellow;
                            Vector2 pos = proText2.GetComponent<RectTransform>().localPosition;
                            pos.x = 15;
                            proText2.GetComponent<RectTransform>().localPosition = pos;
                        }

                    }
                }
            }


        }
    }

    public void setContentAndScroll(GameObject content, ScrollRect rect, int index, LivingObject liveObject)
    {
        if (liveObject != null)
        {
            lastObject = liveObject;
        }
        currentContent = content;
        currentRect = rect;
        AutoRotate();
    }

    private void AutoRotate()
    {
        int startingDegree = 0;
        if (currentRect)
        {
            if (currentContent)
            {
                int childCount = ActiveChildCount();
                switch (childCount)
                {
                    case 1:
                        {
                            GetActiveChild(0).eulerAngles = Vector3.zero;
                        }
                        break;
                    case 2:
                        {
                            GetActiveChild(0).eulerAngles = new Vector3(0, 0, -10);
                            GetActiveChild(1).eulerAngles = new Vector3(0, 0, 10);
                        }
                        break;
                    default:
                        {
                            startingDegree = -childCount * childCount;
                            for (int i = 0; i < childCount; i++)
                            {
                                GetActiveChild(i).eulerAngles = new Vector3(0, 0, startingDegree);
                                startingDegree += childCount * 2;
                            }

                        }
                        break;
                }

            }
        }
    }
    private int ActiveChildCount()
    {
        int count = 0;
        if (currentRect)
        {
            if (currentContent)
            {
                for (int i = 0; i < currentContent.transform.childCount; i++)
                {
                    if (currentContent.transform.GetChild(i).gameObject.activeInHierarchy)
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    private Transform GetActiveChild(int index)
    {
        int count = 0;
        if (currentRect)
        {
            if (currentContent)
            {
                for (int i = 0; i < currentContent.transform.childCount; i++)
                {
                    if (currentContent.transform.GetChild(i).gameObject.activeInHierarchy)
                    {
                        if (index == count)
                        {
                            return currentContent.transform.GetChild(i);
                        }
                        count++;
                    }
                }
            }
        }
        return null;
    }

    public void loadContents(GameObject content, ScrollRect rect, int index, LivingObject liveObject)
    {

        lastObject = liveObject;
        currentContent = content;
        lastIndex = index;
        currentRect = rect;
        currentList.Clear();
        //UsableScript itemType = new UsableScript();
        int useType = -1;
        int windowType = -1;
        //  Debug.Log("index = " + index);
        switch (index)
        {

            case 0:
                // itemType = ScriptableObject.CreateInstance<WeaponScript>();
                // itemType.TYPE = 0;
                useType = 0;
                for (int i = 0; i < liveObject.INVENTORY.WEAPONS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.WEAPONS[i]);
                }
                break;
            case 1:
                // itemType = new ArmorScript();
                // itemType.TYPE = 1;
                useType = 1;
                for (int i = 0; i < liveObject.INVENTORY.ARMOR.Count; i++)
                {
                    if (liveObject.INVENTORY.ARMOR[i].INDEX < 200)
                        currentList.Add(liveObject.INVENTORY.ARMOR[i]);
                }
                break;
            case 2:
                // itemType = new AccessoryScript();
                // itemType.TYPE = 2;
                useType = 2;

                break;
            case 3:
                // itemType = new ItemScript();
                // itemType.TYPE = 3;
                useType = 4;
                windowType = -1;
                // currentList.Add(genericMove);
                //if (liveObject.WEAPON.EQUIPPED)
                //{
                //    genericAtk.NAME = liveObject.WEAPON.NAME;
                //    currentList.Add(genericAtk);
                //}

                for (int i = 0; i < liveObject.OPP_SLOTS.SKILLS.Count; i++)
                {

                    currentList.Add(liveObject.OPP_SLOTS.SKILLS[i]);
                }
                break;
            case 4:
                // itemType = new SkillScript(); 
                // itemType.TYPE = 4;
                useType = 4;
                windowType = 1; //all skills
                for (int i = 0; i < liveObject.GetComponent<InventoryScript>().SKILLS.Count; i++)
                {
                    currentList.Add(liveObject.GetComponent<InventoryScript>().SKILLS[i]);
                }
                break;
            case 5:
                // itemType = new SkillScript(); 
                // itemType.TYPE = 4;
                useType = 4;
                windowType = 3; //all command skills
                for (int i = 0; i < liveObject.INVENTORY.CSKILLS.Count; i++)
                {
                    if (liveObject.INVENTORY.CSKILLS[i].ETYPE == EType.physical)
                        currentList.Add(liveObject.INVENTORY.CSKILLS[i]);
                }
                break;

            case 6:
                useType = 4;
                windowType = 5; //all passive skills
                for (int i = 0; i < liveObject.INVENTORY.COMBOS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.COMBOS[i]);
                }
                break;

            case 7:
                useType = 4;
                windowType = 3; // useable skill slots
                for (int i = 0; i < liveObject.PHYSICAL_SLOTS.SKILLS.Count; i++)
                {
                    currentList.Add(liveObject.PHYSICAL_SLOTS.SKILLS[i]);
                }
                break;

            case 8:
                useType = 4;
                windowType = 5; // passive skill slots
                for (int i = 0; i < liveObject.COMBO_SLOTS.SKILLS.Count; i++)
                {
                    currentList.Add(liveObject.COMBO_SLOTS.SKILLS[i]);
                }
                break;
            case 9:
                useType = 4;
                windowType = 5; // auto skill slots
                for (int i = 0; i < liveObject.INVENTORY.AUTOS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.AUTOS[i]);
                }
                break;
            case 10:
                useType = 4;
                windowType = 6; // opp skill slots

                for (int i = 0; i < liveObject.INVENTORY.OPPS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.OPPS[i]);
                }
                break;
            case 11:
                useType = 5;
                windowType = 3;
                // items

                for (int i = 0; i < liveObject.INVENTORY.ITEMS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.ITEMS[i]);
                }
                break;
            case 12:

                useType = 4;
                windowType = 3;

                for (int i = 0; i < liveObject.INVENTORY.CSKILLS.Count; i++)
                {
                    if (liveObject.INVENTORY.CSKILLS[i].ETYPE == EType.magical)
                        currentList.Add(liveObject.INVENTORY.CSKILLS[i]);
                }
                break;

            case 13:

                useType = 0;
                windowType = 3;

                for (int i = 0; i < liveObject.INVENTORY.WEAPONS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.WEAPONS[i]);
                }
                break;
        }
        // for (int i = 0; i < 4; i++)
        //{

        // List<UsableScript> invokingObjectsUse = liveObject.GetComponent<InventoryScript>().USEABLES;
        for (int useCount = 0; useCount < 6; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
        {
            MenuItem selectableItem = itemSlots[useCount];
            selectableItem.itemType = 15;
            Image attr = selectableItem.GetComponentsInChildren<Image>()[2];
            Text selectedText = selectableItem.GetComponentInChildren<Text>();
            TextMeshProUGUI proText = selectableItem.GetComponentInChildren<TextMeshProUGUI>();
            SupportText support = selectableItem.GetComponentInChildren<SupportText>();
            if (support)
            {
                support.Setup();
                support.SetTransparent();
            }
            string newText = "";
            selectableItem.gameObject.SetActive(true);
            if (useCount < currentList.Count)
            {
                UsableScript item = currentList[useCount];
                newText = item.NAME;
                if (selectedText)
                {
                    selectedText.resizeTextForBestFit = true;
                    selectedText.text = newText;
                }
                if (proText)
                {
                    proText.text = newText;
                }
                //  if (selectableItem.GetComponentInChildren<Text>())
                {

                    if (attr)
                        attr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);


                    newText = item.NAME;
                    if (selectedText)
                    {
                        selectedText.resizeTextForBestFit = true;
                        selectedText.text = newText;
                    }
                    if (proText)
                    {
                        proText.text = newText;
                        if (item.NAME.Length >= 7)
                        {
                            proText.enableAutoSizing = true;
                        }
                        else
                        {
                            proText.enableAutoSizing = false;
                            proText.fontSize = 25.0f;
                        }
                    }

                    {

                        if (windowType == 3 || windowType == -1)
                        {

                            if (item.GetType() == typeof(CommandSkill))
                            {
                                //  if (manager.GetState() == State.PlayerEquipping || manager.GetState() == State.playerUsingSkills || manager.GetState() == State.PlayerOppOptions)
                                {
                                    if (attr)
                                    {
                                        attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                                        if (item != genericAtk)
                                        {
                                            int indxex = (int)((CommandSkill)item).ELEMENT;
                                            attr.sprite = attributeImages[indxex];
                                        }
                                        else
                                        {
                                            int indxex = (int)(liveObject.WEAPON.ELEMENT);
                                            attr.sprite = attributeImages[indxex];
                                        }
                                    }
                                }
                                CommandSkill cmd = ((CommandSkill)item);
                                if (support)
                                {
                                    List<TileScript> cmdTiles = manager.GetSkillAttackableTilesOneList(cmd.OWNER.currentTile, cmd);
                                    attackbleObjects.Clear();
                                    for (int i = 0; i < cmdTiles.Count; i++)
                                    {
                                        GridObject possibleObject = manager.GetObjectAtTile(cmdTiles[i]);
                                        if (possibleObject != null)
                                        {
                                            if (Common.IsEnemy(possibleObject.FACTION))
                                            {
                                                attackbleObjects.Add(possibleObject as LivingObject);
                                            }
                                        }
                                    }
                                    for (int i = 0; i < attackbleObjects.Count; i++)
                                    {
                                        LivingObject griddy = attackbleObjects[i];
                                        int elIndx = Common.GetElementIndex(cmd.ELEMENT);
                                        if (elIndx <= 7)
                                        {
                                            EHitType hitType = griddy.ARMOR.HITLIST[elIndx];
                                            if (hitType < EHitType.normal)
                                            {
                                                if (support.isVisible == false)
                                                {
                                                    support.SetVisible();
                                                    support.SetText(hitType.ToString(), Color.blue);
                                                }
                                            }
                                            else if (hitType > EHitType.normal)
                                            {
                                                if (support.isVisible == false)
                                                {
                                                    support.SetVisible();
                                                    support.SetText(hitType.ToString(), Color.red);
                                                }
                                            }
                                        }

                                    }
                                }
                                string extraText = "";
                                if (proText)
                                {

                                    proText.text = newText;
                                    if (item.NAME.Length > 7)
                                    {
                                        proText.enableAutoSizing = true;
                                    }
                                    else
                                    {
                                        proText.enableAutoSizing = false;
                                        proText.fontSize = 25.0f;
                                    }
                                    if (((CommandSkill)item).ETYPE == EType.physical)
                                    {
                                        //  int cost = ((CommandSkill)item).GetCost(lastObject, lastObject.STATS.SPCHANGE);

                                        if (cmd.COST > 0)
                                        {
                                            int cost = (cmd.GetCost(lastObject, lastObject.STATS.FTCOSTCHANGE));
                                            extraText = cost.ToString();
                                            proText.text += " <size=32><sprite=0></size><color=#72a8ff><size=28>+ </size>" + extraText + "</color>"; //<sprite=0><color=#72a8ff><size=30> +</size>" + extraText + "</color>";

                                        }
                                        else
                                        {
                                            // extraText = (cmd.COST * -1).ToString();
                                            int cost = (-1 * (cmd.GetCost(lastObject, lastObject.STATS.FTCOSTCHANGE)));
                                            extraText = cost.ToString();
                                            proText.text += " <size=32><sprite=0></size><color=#72a8ff><size=28>- </size>" + extraText + "</color>";

                                        }

                                    }
                                    else
                                    {
                                        //extraText = " SP";
                                        int cost = ((CommandSkill)item).GetCost(lastObject, lastObject.STATS.MANACHANGE);

                                        proText.text += " <color=#a770ef>" + cost.ToString() + "</color><size=32><sprite=1></size><color=#4ba0bc><size=28>";

                                    }
                                }
                                else
                                {
                                    if (selectedText)
                                    {
                                        selectedText.text = newText;
                                        selectedText.supportRichText = true;

                                        if (((CommandSkill)item).ETYPE == EType.physical)
                                        {
                                            extraText = cmd.GetCost().ToString();// " " + "FT";
                                            if (cmd.COST > 0)
                                            {
                                                selectedText.text += " <color=#4ba0bc>FT +" + extraText + "</color>";

                                            }
                                            else
                                            {
                                                selectedText.text += "  <color=#63d5d8>FT " + extraText + "</color>";

                                            }

                                        }
                                        else
                                        {
                                            extraText = " SP";
                                            int cost = ((CommandSkill)item).GetCost(lastObject, lastObject.STATS.MANACHANGE);

                                            selectedText.text += " <color=#a770ef>" + cost.ToString() + extraText + "</color>";

                                        }
                                    }
                                }
                            }
                            else if (item.GetType() == typeof(WeaponScript))
                            {
                                //  if (manager.GetState() == State.PlayerEquipping || manager.GetState() == State.playerUsingSkills || manager.GetState() == State.PlayerOppOptions)
                                {
                                    if (attr)
                                    {
                                        attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                                        if (item != genericAtk)
                                        {
                                            int indxex = (int)((WeaponScript)item).ELEMENT;
                                            attr.sprite = attributeImages[indxex];
                                        }
                                        else
                                        {
                                            int indxex = (int)(liveObject.WEAPON.ELEMENT);
                                            attr.sprite = attributeImages[indxex];
                                        }
                                    }
                                }

                                WeaponScript cmd = ((WeaponScript)item);
                                if (support)
                                {
                                    List<TileScript> cmdTiles = manager.GetWeaponAttackableTilesOneList(cmd.USER.currentTile, cmd);
                                    attackbleObjects.Clear();
                                    for (int i = 0; i < cmdTiles.Count; i++)
                                    {
                                        GridObject possibleObject = manager.GetObjectAtTile(cmdTiles[i]);
                                        if (possibleObject != null)
                                        {
                                            if (Common.IsEnemy(possibleObject.FACTION))
                                            {
                                                attackbleObjects.Add(possibleObject as LivingObject);
                                            }
                                        }
                                    }
                                    for (int i = 0; i < attackbleObjects.Count; i++)
                                    {
                                        LivingObject griddy = attackbleObjects[i];
                                        int elIndx = Common.GetElementIndex(cmd.ELEMENT);
                                        if (elIndx <= 7)
                                        {
                                            EHitType hitType = griddy.ARMOR.HITLIST[elIndx];
                                            if (hitType < EHitType.normal)
                                            {
                                                if (support.isVisible == false)
                                                {
                                                    support.SetVisible();
                                                    support.SetText(hitType.ToString(), Color.blue);
                                                }
                                            }
                                            else if (hitType > EHitType.normal)
                                            {
                                                if (support.isVisible == false)
                                                {
                                                    support.SetVisible();
                                                    support.SetText(hitType.ToString(), Color.red);
                                                }
                                            }
                                        }

                                    }
                                }
                                if (proText)
                                {

                                    // proText.text += newText;
                                    int cost = (cmd.GetCost(lastObject, lastObject.STATS.HPCOSTCHANGE));

                                    proText.text += "<size=32><color=#FFC0CB>-" + cost.ToString() + "</color></size> <size=32><sprite=2></size>"; //<sprite=0><color=#72a8ff><size=30> +</size>" + extraText + "</color>";

                                    //if (((WeaponScript)item).ATTACK_TYPE == EType.physical)
                                    //{
                                    //    proText.text = "<sprite=0>";

                                    //}
                                    //else
                                    //{

                                    //    proText.text = "<sprite=1>";
                                    //}

                                    proText.enableAutoSizing = true;


                                }
                                else
                                {
                                    if (selectedText)
                                    {
                                        selectedText.text = newText;
                                        selectedText.supportRichText = true;


                                    }
                                }

                            }
                            else if (item.GetType() == typeof(ArmorScript))
                            {
                                ArmorScript armor = item as ArmorScript;
                                if (attr)
                                {
                                    attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                                    // int indxex = (int)realitem.ELEMENT;
                                    attr.sprite = armor.FACE; //attributeImages[indxex];

                                }

                            }
                            else if (item.GetType() == typeof(ItemScript))
                            {
                                ItemScript realitem = item as ItemScript;
                                if (attr)
                                {
                                    attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                                    if (item != genericAtk)
                                    {
                                        int indxex = (int)realitem.ELEMENT;
                                        attr.sprite = attributeImages[indxex];
                                    }
                                    else
                                    {
                                        int indxex = (int)(liveObject.WEAPON.ELEMENT);
                                        attr.sprite = attributeImages[indxex];
                                    }
                                }

                                if (realitem.ITYPE == ItemType.dmg)
                                {
                                    if (proText)
                                    {


                                        proText.text = "<sprite=1>";

                                        proText.text += newText;

                                        proText.enableAutoSizing = true;


                                    }
                                    else
                                    {
                                        if (selectedText)
                                        {
                                            selectedText.text = newText;
                                            selectedText.supportRichText = true;


                                        }
                                    }
                                }
                            }
                            else if (item.GetType() == typeof(OppSkill))
                            {
                                OppSkill opp = item as OppSkill;
                                if (attr)
                                {
                                    attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                                    int indxex = (int)opp.REACTION;
                                    attr.sprite = attributeImages[indxex];

                                }


                                if (proText)
                                {


                                    proText.text = "<sprite=1>";

                                    proText.text += newText;

                                    proText.enableAutoSizing = true;


                                }
                                else
                                {
                                    if (selectedText)
                                    {
                                        selectedText.text = newText;
                                        selectedText.supportRichText = true;


                                    }
                                }

                            }

                        }


                    }

                }
                if (selectableItem.GetComponent<MenuItem>())
                {
                    //    //  MenuItem selectedItem = selectableItem.GetComponent<MenuItem>();
                    item.TYPE = useType;//itemType.TYPE;
                                        //    if (item.NAME.Equals("MOVE") || item.NAME.Equals("ATTACK"))
                                        //    {
                                        //        item.TYPE = 3;
                                        //    }
                    selectableItem.refItem = item;
                    //    if (index == 3)
                    //    {
                    //        //   if (item == genericMove)
                    //        {

                    //            //     selectableItem.refItem.DESC = "Move a number of tiles";

                    //        }


                    //    }
                }


            }
            else
            {
                if (selectedText)
                    selectedText.text = "";
                if (proText)
                    proText.text = "";
                selectableItem.refItem = null;
                if (windowType < 5)
                    selectableItem.gameObject.SetActive(false);
                if (attr)
                    attr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }


        }

        slotIndex = 5;

        //UpdateColors(itemSlots);

        if (currentRect)
        {
            if (currentContent)
            {
                if (currentIndex < ActiveChildCount())
                    selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                else
                    ForceSelect();
            }
        }
        if (menuManager)
        {
            if (menuManager.DESC)
            {


                if (selectedMenuItem)
                {
                    if (selectedMenuItem.refItem)
                    {
                        menuManager.DESC.text = selectedMenuItem.refItem.DESC;

                    }
                }
            }
        }
        AutoRotate();

    }
    public void loadExtra(UsableScript useable, LivingObject liveObject)
    {
        extraList.Clear();
        int useType = -1;
        int windowType = -1;
        extraList.Add(useable);
        for (int useCount = 0; useCount < 6; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
        {
            MenuItem selectableItem = extraSlots[useCount];
            selectableItem.itemType = 15;
            Image attr = selectableItem.GetComponentsInChildren<Image>()[2];
            Text selectedText = selectableItem.GetComponentInChildren<Text>();
            TextMeshProUGUI proText = selectableItem.GetComponentInChildren<TextMeshProUGUI>();
            string newText = "";
            selectableItem.gameObject.SetActive(true);
            if (useCount < extraList.Count)
            {
                UsableScript item = extraList[useCount];
                newText = item.NAME;
                if (selectedText)
                {
                    selectedText.resizeTextForBestFit = true;
                    selectedText.text = newText;
                }
                if (proText)
                {
                    proText.text = newText;
                }
                //  if (selectableItem.GetComponentInChildren<Text>())
                {

                    if (attr)
                        attr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);


                    newText = item.NAME;
                    if (selectedText)
                    {
                        selectedText.resizeTextForBestFit = true;
                        selectedText.text = newText;
                    }
                    if (proText)
                    {
                        proText.text = newText;
                        if (item.NAME.Length >= 7)
                        {
                            proText.enableAutoSizing = true;
                        }
                        else
                        {
                            proText.enableAutoSizing = false;
                            proText.fontSize = 25.0f;
                        }
                    }

                    {

                        if (windowType == 3 || windowType == -1)
                        {

                            if (item.GetType() == typeof(CommandSkill))
                            {
                                ////  if (manager.GetState() == State.PlayerEquipping || manager.GetState() == State.playerUsingSkills || manager.GetState() == State.PlayerOppOptions)
                                {
                                    if (attr)
                                    {
                                        attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                                        if (item != genericAtk)
                                        {
                                            int indxex = (int)((CommandSkill)item).ELEMENT;
                                            attr.sprite = attributeImages[indxex];
                                        }
                                        else
                                        {
                                            int indxex = (int)(liveObject.WEAPON.ELEMENT);
                                            attr.sprite = attributeImages[indxex];
                                        }
                                    }
                                }
                                CommandSkill cmd = ((CommandSkill)item);
                                string extraText = "";
                                if (proText)
                                {

                                    proText.text = newText;
                                    if (item.NAME.Length > 7)
                                    {
                                        proText.enableAutoSizing = true;
                                    }
                                    else
                                    {
                                        proText.enableAutoSizing = false;
                                        proText.fontSize = 25.0f;
                                    }
                                    if (((CommandSkill)item).ETYPE == EType.physical)
                                    {
                                        //  int cost = ((CommandSkill)item).GetCost(lastObject, lastObject.STATS.SPCHANGE);

                                        if (cmd.COST > 0)
                                        {
                                            int cost = (cmd.GetCost(lastObject, lastObject.STATS.FTCOSTCHANGE));
                                            extraText = cost.ToString();
                                            proText.text += " <size=32><sprite=0></size><color=#72a8ff><size=28>+ </size>" + extraText + "</color>"; //<sprite=0><color=#72a8ff><size=30> +</size>" + extraText + "</color>";

                                        }
                                        else
                                        {
                                            // extraText = (cmd.COST * -1).ToString();
                                            int cost = (-1 * (cmd.GetCost(lastObject, lastObject.STATS.FTCOSTCHANGE)));
                                            extraText = cost.ToString();
                                            proText.text += " <size=32><sprite=0></size><color=#72a8ff><size=28>- </size>" + extraText + "</color>";

                                        }

                                    }
                                    else
                                    {
                                        //extraText = " SP";
                                        int cost = ((CommandSkill)item).GetCost(lastObject, lastObject.STATS.MANACHANGE);

                                        proText.text += " <color=#a770ef>" + cost.ToString() + "</color><size=32><sprite=1></size><color=#4ba0bc><size=28>";

                                    }
                                }
                                else
                                {
                                    if (selectedText)
                                    {
                                        selectedText.text = newText;
                                        selectedText.supportRichText = true;

                                        if (((CommandSkill)item).ETYPE == EType.physical)
                                        {
                                            extraText = cmd.COST.ToString();// " " + "FT";
                                            if (cmd.COST > 0)
                                            {
                                                selectedText.text += " <color=#4ba0bc>FT +" + extraText + "</color>";

                                            }
                                            else
                                            {
                                                selectedText.text += "  <color=#63d5d8>FT " + extraText + "</color>";

                                            }

                                        }
                                        else
                                        {
                                            extraText = " SP";
                                            int cost = ((CommandSkill)item).GetCost(lastObject, lastObject.STATS.MANACHANGE);

                                            selectedText.text += " <color=#a770ef>" + cost.ToString() + extraText + "</color>";

                                        }
                                    }
                                }
                            }
                            else if (item.GetType() == typeof(WeaponScript))
                            {
                                // //  if (manager.GetState() == State.PlayerEquipping || manager.GetState() == State.playerUsingSkills || manager.GetState() == State.PlayerOppOptions)
                                {
                                    if (attr)
                                    {
                                        attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                                        if (item != genericAtk)
                                        {
                                            int indxex = (int)((WeaponScript)item).ELEMENT;
                                            attr.sprite = attributeImages[indxex];
                                        }
                                        else
                                        {
                                            int indxex = (int)(liveObject.WEAPON.ELEMENT);
                                            attr.sprite = attributeImages[indxex];
                                        }
                                    }
                                }

                                WeaponScript cmd = ((WeaponScript)item);

                                if (proText)
                                {
                                    // if (((WeaponScript)item).ATTACK_TYPE == EType.physical)
                                    int cost = (cmd.GetCost(lastObject, lastObject.STATS.HPCOSTCHANGE));

                                    proText.text += "<size=32><color=#FFC0CB> -" + cost.ToString() + "</color></size> <size=32><sprite=2></size>"; //<sprite=0><color=#72a8ff><size=30> +</size>" + extraText + "</color>";




                                    // proText.text += newText;

                                    proText.enableAutoSizing = true;


                                }
                                else
                                {
                                    if (selectedText)
                                    {
                                        selectedText.text = newText;
                                        selectedText.supportRichText = true;


                                    }
                                }

                            }
                            else if (item.GetType() == typeof(ArmorScript))
                            {
                                ArmorScript armor = item as ArmorScript;
                                if (attr)
                                {
                                    attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                                    // int indxex = (int)realitem.ELEMENT;
                                    attr.sprite = armor.FACE; //attributeImages[indxex];

                                }

                            }
                            else if (item.GetType() == typeof(ItemScript))
                            {
                                ItemScript realitem = item as ItemScript;
                                if (attr)
                                {
                                    attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                                    if (item != genericAtk)
                                    {
                                        int indxex = (int)realitem.ELEMENT;
                                        attr.sprite = attributeImages[indxex];
                                    }
                                    else
                                    {
                                        int indxex = (int)(liveObject.WEAPON.ELEMENT);
                                        attr.sprite = attributeImages[indxex];
                                    }
                                }

                                if (realitem.ITYPE == ItemType.dmg)
                                {
                                    if (proText)
                                    {


                                        proText.text = "<sprite=1>";

                                        proText.text += newText;

                                        proText.enableAutoSizing = true;


                                    }
                                    else
                                    {
                                        if (selectedText)
                                        {
                                            selectedText.text = newText;
                                            selectedText.supportRichText = true;


                                        }
                                    }
                                }
                            }
                            else if (item.GetType() == typeof(OppSkill))
                            {
                                OppSkill opp = item as OppSkill;
                                if (attr)
                                {
                                    attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                                    int indxex = (int)opp.REACTION;
                                    attr.sprite = attributeImages[indxex];

                                }


                                if (proText)
                                {


                                    proText.text = "<sprite=1>";

                                    proText.text += newText;

                                    proText.enableAutoSizing = true;


                                }
                                else
                                {
                                    if (selectedText)
                                    {
                                        selectedText.text = newText;
                                        selectedText.supportRichText = true;


                                    }
                                }

                            }

                        }


                    }

                }
                if (selectableItem.GetComponent<MenuItem>())
                {
                    //    //  MenuItem selectedItem = selectableItem.GetComponent<MenuItem>();
                    item.TYPE = useType;//itemType.TYPE;
                                        //    if (item.NAME.Equals("MOVE") || item.NAME.Equals("ATTACK"))
                                        //    {
                                        //        item.TYPE = 3;
                                        //    }
                    selectableItem.refItem = item;
                    //    if (index == 3)
                    //    {
                    //        //   if (item == genericMove)
                    //        {

                    //            //     selectableItem.refItem.DESC = "Move a number of tiles";

                    //        }


                    //    }
                }


            }
            else
            {
                if (selectedText)
                    selectedText.text = "";
                if (proText)
                    proText.text = "";
                selectableItem.refItem = null;
                //if (windowType < 5)
                //    selectableItem.gameObject.SetActive(false);
                if (attr)
                    attr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }


        }

        slotIndex = 5;

        //UpdateColors(itemSlots);


        if (menuManager)
        {
            if (menuManager.DESC)
            {


                if (selectedMenuItem)
                {
                    if (selectedMenuItem.refItem)
                    {
                        menuManager.DESC.text = selectedMenuItem.refItem.DESC;

                    }
                }
            }
        }

    }
    public void loadExtra(int index, LivingObject liveObject)
    {
        extraList.Clear();
        int useType = -1;

        switch (index)
        {

            case 0:
                useType = 4;
                for (int i = 0; i < liveObject.PHYSICAL_SLOTS.SKILLS.Count; i++)
                {
                    extraList.Add(liveObject.PHYSICAL_SLOTS.SKILLS[i]);
                }
                break;

            case 1:
                useType = 4;
                for (int i = 0; i < liveObject.COMBO_SLOTS.SKILLS.Count; i++)
                {
                    extraList.Add(liveObject.COMBO_SLOTS.SKILLS[i]);
                }
                break;


            case 2:
                useType = 4;
                for (int i = 0; i < liveObject.AUTO_SLOTS.SKILLS.Count; i++)
                {
                    extraList.Add(liveObject.AUTO_SLOTS.SKILLS[i]);
                }
                break;


            case 3:
                useType = 4;
                for (int i = 0; i < liveObject.OPP_SLOTS.SKILLS.Count; i++)
                {
                    extraList.Add(liveObject.OPP_SLOTS.SKILLS[i]);
                }
                break;

            case 4:
                //equipped weapon
                useType = 0;
                for (int i = 0; i < liveObject.INVENTORY.WEAPONS.Count; i++)
                {
                    if (liveObject.INVENTORY.WEAPONS[i].NAME == liveObject.WEAPON.NAME)
                    {
                        extraList.Add(liveObject.INVENTORY.WEAPONS[i]);
                        break;
                    }
                }
                break;
            case 5:
                //equipped armor
                useType = 1;
                for (int i = 0; i < liveObject.INVENTORY.ARMOR.Count; i++)
                {
                    if (liveObject.INVENTORY.ARMOR[i].NAME == liveObject.ARMOR.NAME)
                    {
                        extraList.Add(liveObject.INVENTORY.ARMOR[i]);
                        break;
                    }
                }
                break;

        }
        //UpdateColors(extraSlots);
        //UsableScript item in liveObject.GetComponents<UsableScript>())

        for (int useCount = 0; useCount < 6; useCount++)
        {
            MenuItem selectableItem = extraSlots[useCount];
            selectableItem.gameObject.SetActive(true);

            if (useType < 4)
            {
                if (useCount > 0)
                {
                    //  selectableItem.gameObject.SetActive(false);
                    continue;
                }
            }
            Text selectedText = selectableItem.GetComponentInChildren<Text>();
            if (useCount < extraList.Count)
            {
                UsableScript item = extraList[useCount];

                if (selectableItem.GetComponentInChildren<Text>())
                {

                    selectedText.text = item.NAME;

                    selectedText.resizeTextForBestFit = true;
                }
                if (selectableItem.GetComponent<MenuItem>())
                {
                    item.TYPE = useType;//itemType.TYPE;
                    selectableItem.refItem = item;


                }
            }
            else
            {
                selectableItem.refItem = null;
                selectedText.text = "";
                //   selectableItem.gameObject.SetActive(false);
            }


        }

        if (menuManager)
        {
            if (menuManager.DESC)
            {


                if (selectedMenuItem)
                {
                    if (selectedMenuItem.refItem)
                    {
                        menuManager.DESC.text = selectedMenuItem.refItem.DESC;

                    }
                }
            }
        }

    }
    private void DetermineAndFillExtra()
    {
        if (menuSide == -1)
            if (currentIndex < currentList.Count)
            {
                if (selectedMenuItem.refItem)
                {

                    if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                    {
                        loadExtra(0, lastObject);
                    }

                    if (selectedMenuItem.refItem.GetType() == typeof(ComboSkill))
                    {
                        loadExtra(1, lastObject);
                    }

                    if (selectedMenuItem.refItem.GetType() == typeof(AutoSkill))
                    {
                        loadExtra(2, lastObject);
                    }

                    if (selectedMenuItem.refItem.GetType() == typeof(OppSkill))
                    {
                        loadExtra(3, lastObject);
                    }
                }
            }
        //UpdateColors(itemSlots);
        // UpdateColors(extraSlots);
    }

    public void EquipSkill()
    {
        if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
        {
            CommandSkill skill = selectedMenuItem.refItem as CommandSkill;
            if (skill.ETYPE == EType.physical)
            {

                if (!lastObject.PHYSICAL_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                {
                    if (lastObject.PHYSICAL_SLOTS.SKILLS.Count < 6)
                    {

                        lastObject.PHYSICAL_SLOTS.SKILLS.Add((SkillScript)selectedMenuItem.refItem);
                        loadExtra(0, lastObject);
                    }
                }
                else
                {
                    UnequipSkill();
                }
            }
            else
            {
                if (!lastObject.MAGICAL_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                {
                    if (lastObject.MAGICAL_SLOTS.SKILLS.Count < 6)
                    {

                        lastObject.MAGICAL_SLOTS.SKILLS.Add((SkillScript)selectedMenuItem.refItem);
                        loadExtra(0, lastObject);
                    }
                }
                else
                {
                    UnequipSkill();
                }
            }

        }

        else if (selectedMenuItem.refItem.GetType() == typeof(ComboSkill))
        {
            if (!lastObject.COMBO_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
            {
                if (lastObject.COMBO_SLOTS.SKILLS.Count < 6)
                {

                    lastObject.COMBO_SLOTS.SKILLS.Add((SkillScript)selectedMenuItem.refItem);
                    lastObject.UpdateBuffsAndDebuffs();
                    loadExtra(1, lastObject);
                }
            }
            else
            {
                UnequipSkill();
            }
        }

        else if (selectedMenuItem.refItem.GetType() == typeof(AutoSkill))
        {
            if (!lastObject.AUTO_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
            {

                if (lastObject.AUTO_SLOTS.SKILLS.Count < 6)
                {

                    lastObject.AUTO_SLOTS.SKILLS.Add((SkillScript)selectedMenuItem.refItem);
                    loadExtra(2, lastObject);
                }

            }
            else
            {
                UnequipSkill();
            }
        }

        else if (selectedMenuItem.refItem.GetType() == typeof(OppSkill))
        {
            if (!lastObject.OPP_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
            {

                if (lastObject.OPP_SLOTS.SKILLS.Count < 6)
                {
                    lastObject.OPP_SLOTS.SKILLS.Add((SkillScript)selectedMenuItem.refItem);
                    loadExtra(3, lastObject);
                }
            }
            else
            {
                UnequipSkill();
            }
        }
        else
        {
            Debug.Log("No match!");
            Debug.Log(selectedMenuItem.refItem.GetType());
        }
        //  UpdateColors(itemSlots);
        //  UpdateColors(extraSlots);

    }

    public void UnequipSkill()
    {
        if (selectedMenuItem)
        {
            if (selectedMenuItem.refItem)
            {

                if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                {

                    if (lastObject.PHYSICAL_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                    {
                        bool check = lastObject.PHYSICAL_SLOTS.SKILLS.Remove((SkillScript)selectedMenuItem.refItem);

                        loadExtra(0, lastObject);
                    }
                }


                else if (selectedMenuItem.refItem.GetType() == typeof(ComboSkill))
                {
                    if (lastObject.COMBO_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                    {

                        lastObject.COMBO_SLOTS.SKILLS.Remove((SkillScript)selectedMenuItem.refItem);
                        // lastObject.UpdateBuffsAndDebuffs();
                        loadExtra(1, lastObject);
                    }
                }

                else if (selectedMenuItem.refItem.GetType() == typeof(AutoSkill))
                {
                    if (lastObject.AUTO_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                    {

                        lastObject.AUTO_SLOTS.SKILLS.Remove((SkillScript)selectedMenuItem.refItem);
                        loadExtra(2, lastObject);
                    }
                }

                else if (selectedMenuItem.refItem.GetType() == typeof(OppSkill))
                {
                    if (lastObject.OPP_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                    {

                        lastObject.OPP_SLOTS.SKILLS.Remove((SkillScript)selectedMenuItem.refItem);
                        loadExtra(3, lastObject);
                    }
                }
                else
                {
                    Debug.Log("No match!");
                    Debug.Log(selectedMenuItem.refItem.GetType());
                }
            }
        }
        // UpdateColors(itemSlots);
        //UpdateColors(extraSlots);
    }
    public void unloadContents()
    {
        currentList.Clear();
        //if (lastObject)
        //{
        //    Debug.Log(lastObject.FullName);
        //    List<UsableScript> invokingObjectsUse = lastObject.GetComponent<InventoryScript>().USEABLES;
        //    for (int useCount = 0; useCount < invokingObjectsUse.Count; useCount++)
        //    {
        //        UsableScript item = invokingObjectsUse[useCount];
        //        if (tempObjects.ContainsKey(item.NAME))
        //        {
        //            GameObject obj = tempObjects[item.NAME];
        //            obj.transform.SetParent(null);
        //            obj.gameObject.SetActive(false);
        //        }
        //    }
        //}
    }

}
