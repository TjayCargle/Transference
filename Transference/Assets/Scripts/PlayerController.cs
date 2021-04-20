using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public CommandSkill currentSkill;


    [SerializeField]
    public LivingObject current;

    [SerializeField]
    protected ManagerScript myManager;

    [SerializeField]
    InventoryMangager invm;


    [SerializeField]
    public ItemScript currentItem;
    //public bool canMove = false;
    private List<TileScript> newTarget = new List<TileScript>();
    // Use this for initialization
    void Start()
    {
        myManager = Common.GetManager();
        invm = myManager.GetComponent<InventoryMangager>();
        if (!myManager.isSetup)
        {
            myManager.Setup();
        }
        if (myManager.turnOrder.Count > 0)
        {
            //  current = myManager.turnOrder[0];
        }
        // base.Start();
        // MoveDist = 1;
        // MAX_ATK_DIST = 2;
        // SkillManager.CreateSkill(1);
    }

    // Update is called once per frame
    void Update()
    {
        //base.Update();
        if (current)
        {

            switch (myManager.currentState)
            {

                case State.PlayerInput:
                    //if (Input.GetKeyDown(KeyCode.W))
                    //{
                    //    myManager.currentMenuitem--;
                    //    if (myManager.currentMenuitem < 0)
                    //    {
                    //        myManager.currentMenuitem = 5;
                    //    }
                    //    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    //}

                    //if (Input.GetKeyDown(KeyCode.S))
                    //{
                    //    myManager.currentMenuitem++;
                    //    if (myManager.currentMenuitem > 5)
                    //    {
                    //        myManager.currentMenuitem = 0;
                    //    }
                    //    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    //}

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        //myManager.invManager.prevIndex = myManager.invManager.currentIndex;
                        //myManager.invManager.currentIndex = 0;

                        myManager.SelectMenuItem(current);
                    }




                    break;
                case State.PlayerAllocate:

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        myManager.SelectMenuItem(current);
                    }

                    break;
                case State.PlayerMove:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        myManager.MoveGridObject(current, new Vector3(0, 0, 1));
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        myManager.MoveGridObject(current, new Vector3(-1, 0, 0));
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        myManager.MoveGridObject(current, new Vector3(0, 0, -1));
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        myManager.MoveGridObject(current, new Vector3(1, 0, 0));
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        if (myManager.ComfirmMenuAction(current))
                        {
                            bool useation = true;


                            if (useation)
                            {
                                current.TakeAction();
                            }
                            // myManager.ComfirmMoveGridObject(current,myManager.GetTileIndex(current));
                        }

                    }
                    //if (Input.GetKeyDown(KeyCode.Escape))
                    //{
                    //    myManager.CancelMenuAction(current);
                    //}
                    break;
                case State.PlayerAttacking:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {

                            UseOrAttack();

                        }
                    }
                    break;
                case State.PlayerEquippingMenu:
                    //if (Input.GetKeyDown(KeyCode.W))
                    //{
                    //    myManager.currentMenuitem--;
                    //    if (myManager.currentMenuitem < 6)
                    //    {
                    //        myManager.currentMenuitem = 8;
                    //    }
                    //    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    //}

                    //if (Input.GetKeyDown(KeyCode.S))
                    //{
                    //    myManager.currentMenuitem++;
                    //    if (myManager.currentMenuitem > 8)
                    //    {
                    //        myManager.currentMenuitem = 6;
                    //    }
                    //    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    //}
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        //myManager.invManager.prevIndex = myManager.invManager.currentIndex;
                        // myManager.invManager.currentIndex = 0;
                        myManager.SelectMenuItem(current);
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        // myManager.invManager.currentIndex = myManager.invManager.prevIndex;
                        myManager.CancelMenuAction(current);
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        currentSkill = null;
                        myManager.CancelMenuAction(current);
                    }
                    break;

                case State.PlayerEquipping:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            //  useOrEquip();
                            myManager.CreateEvent(this, null, "buffered use", myManager.ReturnTrue, BuffereduseOrEquip);

                        }
                        break;
                    }
                    break;
                case State.playerUsingSkills:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            //  useOrEquip();

                            myManager.CreateEvent(this, null, "buffered use", myManager.ReturnTrue, BuffereduseOrEquip);

                        }

                        break;
                    }
                case State.PlayerWait:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        current.Wait();
                        myManager.ComfirmMenuAction(current);
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        myManager.CancelMenuAction(current);
                    }
                    break;
                case State.FreeCamera:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        myManager.ComfirmMenuAction(current);
                    }

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        //myManager.invManager.currentIndex = myManager.invManager.prevIndex;
                        myManager.CancelMenuAction(current);
                    }
                    break;
                case State.PlayerEquippingSkills:

                    if (Input.GetKeyDown(KeyCode.Return))
                    {

                        //  UseItem();
                        myManager.CreateEvent(this, null, "buffered use", myManager.ReturnTrue, BuffereduseOrEquip);
                        //   useOrEquip();
                    }



                    break;
                case State.PlayerSkillsMenu:
                    if (Input.GetMouseButtonDown(1))
                    {

                        myManager.CancelMenuAction(current);
                    }
                    break;
                case State.EnemyTurn:
                    break;
                case State.PlayerOppOptions:
                    {

                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            myManager.oppEvent.caller = null;
                            myManager.CancelMenuAction(current);
                        }

                        if (Input.GetMouseButtonDown(1))
                        {
                            myManager.oppEvent.caller = null;
                            myManager.CancelMenuAction(current);
                        }
                    }
                    break;

                case State.PlayerSelectItem:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            Debug.Log("playa itms");
                            //  UseItem();
                            myManager.CreateEvent(this, null, "buffered use", myManager.ReturnTrue, BuffereduseOrEquip);
                            //   useOrEquip();
                        }
                        //if (Input.GetKeyDown(KeyCode.Escape))
                        //{
                        //    myManager.CancelMenuAction(current);
                        //}
                        //if (Input.GetMouseButtonDown(1))
                        //{
                        //    myManager.CancelMenuAction(current);
                        //}
                    }
                    break;
                case State.PlayerUsingItems:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {

                            if (Input.GetKeyDown(KeyCode.Return))
                            {

                                if (myManager.myCamera.infoObject)
                                {

                                    if (myManager.myCamera.infoObject)
                                    {

                                        if (myManager.myCamera.infoObject.GetComponent<LivingObject>())
                                        {

                                            UseItem(myManager.myCamera.infoObject.GetComponent<LivingObject>(), myManager.myCamera.currentTile);
                                        }
                                        else if (currentItem.ITYPE == ItemType.dmg || currentItem.ITYPE == ItemType.dart)
                                        {
                                            UseItem(myManager.myCamera.infoObject, myManager.myCamera.currentTile);
                                        }
                                        else
                                        {
                                            myManager.CreateTextEvent(this, "Invalid target", "validation text", myManager.CheckText, myManager.TextStart);
                                            myManager.PlayExitSnd();
                                        }
                                    }
                                    else
                                    {
                                        UseItem(null, myManager.myCamera.currentTile);
                                    }
                                }
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {

                            myManager.CancelMenuAction(current);
                            currentItem = null;
                            currentSkill = null;
                        }

                    }
                    break;

                default:
                    break;
            }


        }
    }

    public void UseItem(GridObject target, TileScript TargetTile)
    {
        if (TargetTile)
        {

            myManager.CreateEvent(this, TargetTile, "use item 1 event", UseItemEvent);
        }
        else if (target)
        {

            myManager.CreateEvent(this, target.currentTile, "use item 1 event", UseItemEvent);
        }
    }
    public bool UseItemEvent(Object data)
    {

        TileScript TargetTile = data as TileScript;
        GridObject target = null;
        if (TargetTile)
        {
            target = myManager.GetObjectAtTile(TargetTile);
        }

        if (currentItem)
        {
            bool useditem = false;
            if (!target && TargetTile)
            {
                if (currentItem.TTYPE != TargetType.adjecent)
                {
                    return true;
                }
                useditem = currentItem.useItem(current, current, TargetTile);
                if (useditem == true)
                {
                    GridAnimationObj gao = null;
                    gao = myManager.PrepareGridAnimation(null, current);
                    gao.type = -2;
                    gao.magnitute = 0;
                    gao.LoadGridAnimation();

                    myManager.menuManager.ShowNone();
                    myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

                    if (myManager.log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                        myManager.log.Log(coloroption + current.NAME + "</color> used " + currentItem.NAME);
                    }

                    myManager.CreateTextEvent(this, currentItem.NAME, "item used", myManager.CheckText, myManager.TextStart);

                    current.INVENTORY.ITEMS.Remove(currentItem);

                    current.TakeAction();
                    // if (current.ACTIONS > 0)
                    {
                        myManager.CreateEvent(this, null, "return state", myManager.BufferedReturnEvent);
                    }
                    currentItem = null;
                    currentSkill = null;
                }
            }
            if (target)
            {

                if (target.FACTION == Faction.ally && currentItem.TTYPE != TargetType.ally)
                {
                    myManager.CreateTextEvent(this, "Cannot use " + currentItem.NAME + " on " + target.NAME, "item invalid target", myManager.CheckText, myManager.TextStart);
                    return true;
                }
                if (target.FACTION == Faction.enemy && currentItem.TTYPE == TargetType.ally)
                {
                    myManager.CreateTextEvent(this, "Cannot use " + currentItem.NAME + " on " + target.NAME, "item invalid target", myManager.CheckText, myManager.TextStart);
                    return true;
                }


                useditem = currentItem.useItem(target, current);

                if (useditem == false)
                {
                    switch (currentItem.ITYPE)
                    {
                        case ItemType.healthPotion:
                            myManager.CreateTextEvent(this, " Health Full", "action item", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.manaPotion:
                            myManager.CreateTextEvent(this, "Mana Full", "action item", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.fatiguePotion:
                            myManager.CreateTextEvent(this, "FT Maxxed. ", "item invalid target", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.cure:
                            myManager.CreateTextEvent(this, "No Ailments. ", "item invalid target", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.buff:
                            break;
                        case ItemType.dmg:
                            break;
                        case ItemType.dart:
                            myManager.CreateTextEvent(this, "Ailment Exists. ", "item invalid target", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.actionBoost:
                            myManager.CreateTextEvent(this, "Chance of gaining action points failed", "action item", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.random:
                            break;
                    }
                }

                if (useditem == true || currentItem.ITYPE == ItemType.actionBoost)
                {
                    if (currentItem.ITYPE != ItemType.dmg && currentItem.ITYPE == ItemType.dart)
                    {
                        GridAnimationObj gao = null;
                        gao = myManager.PrepareGridAnimation(null, current);
                        gao.type = -2;
                        if (currentItem.ITYPE == ItemType.dart)
                            gao.type = 2;
                        gao.magnitute = 0;
                        gao.LoadGridAnimation();

                        myManager.menuManager.ShowNone();
                        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);



                    }
                    if (myManager.log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                        string coloroption2 = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(target.FACTION)) + ">";
                        myManager.log.Log(coloroption + current.NAME + "</color> used " + currentItem.NAME + " on " + coloroption2 + target.NAME + "</color>");
                    }
                    myManager.CreateTextEvent(this, currentItem.NAME, "item used", myManager.CheckText, myManager.TextStart);
                    current.INVENTORY.ITEMS.Remove(currentItem);
                    current.INVENTORY.USEABLES.Remove(currentItem);
                    //   myManager.menuManager.ShowItemCanvas(11, current);

                    current.TakeAction();
                    if (current.ACTIONS > 0)
                    {
                        myManager.CreateEvent(this, null, "return state", myManager.BufferedReturnEvent);
                    }

                    currentItem = null;
                    currentSkill = null;
                }

            }
        }
        return true;
    }
    public void useOrEquip()
    {
        if (current)
        {

            if (invm.selectedMenuItem)
            {
                if (invm.selectedMenuItem.refItem)
                {
                    switch (invm.selectedMenuItem.refItem.TYPE)
                    {
                        case 0:
                            if (invm.selectedMenuItem.refItem.GetType() == typeof(WeaponScript))
                            {


                                WeaponScript selectedSkil = (WeaponScript)invm.selectedMenuItem.refItem;

                                if (selectedSkil)
                                {
                                    if (current.WEAPON.EQUIPPED != selectedSkil)
                                    {
                                        current.WEAPON.Equip(selectedSkil);
                                    }
                                }


                                if (selectedSkil.CanUse(current.STATS.HPCOSTCHANGE))
                                    prepareAttack(selectedSkil);
                                else
                                {
                                    myManager.ShowCantUseText(selectedSkil);
                                    myManager.PlayExitSnd();
                                }
                            }
                            break;
                        case 1:
                            if (invm.selectedMenuItem.refItem.GetType() == typeof(ArmorScript))
                            {
                                if (myManager.invManager.menuSide == -1)
                                {
                                    ArmorScript newArmor = (ArmorScript)invm.selectedMenuItem.refItem;
                                    if (current.ARMOR.SCRIPT != newArmor)
                                    {
                                        if (myManager.liveEnemies.Count == 0)
                                        {
                                            myManager.CreateTextEvent(this, " No enemies around, no need to use a barrier ", "Barrier active event", myManager.CheckText, myManager.TextStart);
                                            myManager.PlayExitSnd();
                                        }
                                        else
                                        {
                                            //todo, call prepare barrier instead
                                            GridAnimationObj gao = null;
                                            gao = myManager.PrepareGridAnimation(null, current);
                                            gao.type = -4;
                                            gao.magnitute = 0;
                                            gao.LoadGridAnimation();

                                            myManager.menuManager.ShowNone();
                                            myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

                                            myManager.CreateTextEvent(this, newArmor.NAME, "wait event", myManager.CheckText, myManager.TextStart);
                                            if (myManager.log)
                                            {
                                                string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                                                myManager.log.Log(coloroption + current.NAME + "</color> summoned a " + newArmor.NAME);
                                            }

                                            myManager.CreateEvent(this, newArmor, "Neo barrier equip", current.SummonBarrier);

                                        }
                                    }
                                    else
                                    {
                                        myManager.CreateTextEvent(this, " That barrier is already active ", "Barrier active event", myManager.CheckText, myManager.TextStart);
                                        myManager.PlayExitSnd();
                                    }
                                    // myManager.menuManager.ShowExtraCanvas(5, current);
                                }
                                else
                                {
                                    //  current.ARMOR.unEquip();
                                    //current.ARMOR.NAME = "none";
                                    //current.ARMOR.DEFENSE = 0;
                                    //current.ARMOR.RESISTANCE = 0;
                                    //current.ARMOR.HITLIST = Common.noAmor;
                                    //for (int i = 0; i < 7; i++)
                                    //{
                                    //    current.ARMOR.HITLIST.Add(EHitType.normal);
                                    //}
                                }
                                //  myManager.menuManager.ShowExtraCanvas(5, current);

                            }
                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                        case 4:
                            {
                                if (myManager.GetState() == State.PlayerEquippingSkills)
                                {

                                    if (myManager.invManager.menuSide == -1)
                                    {

                                        myManager.invManager.EquipSkill();
                                    }
                                    else
                                    {

                                        myManager.invManager.UnequipSkill();
                                    }
                                }

                                else
                                {
                                    if (((SkillScript)invm.selectedMenuItem.refItem).ELEMENT == Element.Passive)
                                    {
                                        break;
                                    }
                                    if (((SkillScript)invm.selectedMenuItem.refItem).ELEMENT == Element.Auto)
                                    {
                                        break;
                                    }
                                    if (((SkillScript)invm.selectedMenuItem.refItem).ELEMENT == Element.Opp)
                                    {
                                        OppSkill opp = (OppSkill)invm.selectedMenuItem.refItem;
                                        if (opp)
                                        {

                                            CommandSkill oppSkill = Common.GenericSkill;
                                            oppSkill.ACCURACY = 100;
                                            oppSkill.COST = 0;
                                            oppSkill.ELEMENT = opp.REACTION;
                                            oppSkill.DAMAGE = opp.DAMAGE;
                                            oppSkill.HITS = 1;
                                            oppSkill.SKILLTYPE = SkillType.Command;
                                            oppSkill.SUBTYPE = opp.SUBTYPE;
                                            oppSkill.OWNER = current;
                                            oppSkill.NAME = opp.NAME;
                                            currentSkill = oppSkill;

                                            prepareAttack(oppSkill, myManager.oppObj);
                                        }
                                        break;
                                    }
                                    CommandSkill selectedSkil = (CommandSkill)invm.selectedMenuItem.refItem;
                                    if (current.GetComponent<InventoryScript>().ContainsSkillName(selectedSkil.NAME) != null)
                                    {
                                        //  Debug.Log("Got it");
                                        selectedSkil = (CommandSkill)current.GetComponent<InventoryScript>().ContainsSkillName(selectedSkil.NAME);
                                    }
                                    float modification = 1.0f;
                                    if (selectedSkil.ETYPE == EType.magical)
                                        modification = current.STATS.MANACHANGE;
                                    if (selectedSkil.ETYPE == EType.physical)
                                    {
                                        if (selectedSkil.COST > 0)
                                        {
                                            modification = current.STATS.FTCHARGECHANGE;
                                        }
                                        else
                                        {
                                            modification = current.STATS.FTCOSTCHANGE;
                                        }
                                    }
                                    if (selectedSkil.CanUse(modification))
                                    {

                                        currentSkill = selectedSkil;
                                        prepareAttack(currentSkill);
                                    }
                                    else
                                    {
                                        myManager.ShowCantUseText(selectedSkil);
                                        myManager.PlayExitSnd();
                                    }

                                }
                                break;


                            }

                        case 5:
                            {
                                //UseItem();
                                if (invm.selectedMenuItem)
                                {
                                    if (invm.selectedMenuItem.refItem)
                                    {
                                        if (invm.selectedMenuItem.refItem.TYPE == 5)
                                        {
                                            currentItem = (ItemScript)invm.selectedMenuItem.refItem;
                                            if (currentItem)
                                            {
                                                if (currentItem.ITYPE == ItemType.dmg)
                                                {

                                                    CommandSkill itemskill = Common.GenericSkill;
                                                    itemskill.ACCURACY = 100;
                                                    itemskill.COST = 0;
                                                    itemskill.ELEMENT = currentItem.ELEMENT;
                                                    itemskill.DAMAGE = currentItem.PDMG;
                                                    itemskill.HITS = 1;
                                                    itemskill.SKILLTYPE = SkillType.Command;
                                                    itemskill.SUBTYPE = SubSkillType.Item;
                                                    itemskill.OWNER = current;
                                                    itemskill.NAME = currentItem.NAME;
                                                    currentSkill = itemskill;
                                                }
                                            }
                                            myManager.attackableTiles = myManager.GetItemUseableTiles(current, currentItem);

                                            myManager.ShowWhite();

                                            prepareAttack(currentItem);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }
    }

    public void forgetSkill()
    {
        if (current)
        {

            if (invm.selectedMenuItem)
            {
                if (invm.selectedMenuItem.refItem)
                {
                    switch (invm.selectedMenuItem.refItem.TYPE)
                    {
                        case 0:
                            if (invm.selectedMenuItem.refItem.GetType() == typeof(WeaponScript))
                            {


                                WeaponScript selectedSkil = (WeaponScript)invm.selectedMenuItem.refItem;

                                if (selectedSkil)
                                {
                                    if (current.INVENTORY.WEAPONS.Contains(selectedSkil))
                                    {
                                        current.INVENTORY.WEAPONS.Remove(selectedSkil);
                                    }
                                    else
                                    {
                                        selectedSkil = null;
                                    }
                                    if (myManager.newSkillEvent.data)
                                    {
                                        if (myManager.newSkillEvent.data.GetType() == typeof(WeaponScript))
                                        {
                                            current.INVENTORY.WEAPONS.Add(myManager.newSkillEvent.data as WeaponScript);
                                            (myManager.newSkillEvent.data as UsableScript).USER = current;
                                            myManager.CreateEvent(this, myManager.newSkillEvent.data, "New Skill Event", myManager.CheckCount, null, 0, myManager.CountStart);
                                           // myManager.CreateTextEvent(this, "Learned " + (myManager.newSkillEvent.data as WeaponScript).NAME, "new skill event", myManager.CheckText, myManager.TextStart);

                                            if (myManager.log)
                                            {
                                                string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                                                myManager.log.Log(coloroption + current.NAME + "</color> learned " + (myManager.newSkillEvent.data as WeaponScript).NAME);
                                            }
                                        }
                                    }

                                }



                            }
                            break;
                        case 1:
                            if (invm.selectedMenuItem.refItem.GetType() == typeof(ArmorScript))
                            {


                                ArmorScript newArmor = (ArmorScript)invm.selectedMenuItem.refItem;
                                if (newArmor)
                                {

                                    if (current.INVENTORY.ARMOR.Contains(newArmor))
                                    {
                                        if (current.ARMOR == newArmor)
                                        {
                                            current.ARMOR.unEquip();
                                        }
                                        current.INVENTORY.ARMOR.Remove(newArmor);
                                    }
                                    else
                                    {
                                        newArmor = null;
                                    }

                                    if (myManager.newSkillEvent.data)
                                    {
                                        if (myManager.newSkillEvent.data.GetType() == typeof(ArmorScript))
                                        {
                                            current.INVENTORY.ARMOR.Add(myManager.newSkillEvent.data as ArmorScript);
                                            (myManager.newSkillEvent.data as UsableScript).USER = current;
                                            myManager.CreateEvent(this, myManager.newSkillEvent.data, "New Skill Event", myManager.CheckCount, null, 0, myManager.CountStart);
                                           // myManager.CreateTextEvent(this, "" + current.FullName + " learned " + (myManager.newSkillEvent.data as ArmorScript).NAME, "new skill event", myManager.CheckText, myManager.TextStart);

                                            if (myManager.log)
                                            {
                                                string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                                                myManager.log.Log(coloroption + current.NAME + "</color> learned " + (myManager.newSkillEvent.data as ArmorScript).NAME);
                                            }
                                        }
                                    }
                                }



                            }
                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                        case 4:
                            {


                                //if (invm.selectedMenuItem.refItem.GetType().IsSubclassOf(typeof(SkillScript)))
                                {
                                    if (((SkillScript)invm.selectedMenuItem.refItem).ELEMENT == Element.Passive)
                                    {
                                        ComboSkill passiveSkill = (ComboSkill)invm.selectedMenuItem.refItem;
                                        if (passiveSkill)
                                        {
                                            if (current.INVENTORY.COMBOS.Contains(passiveSkill))
                                            {
                                                current.INVENTORY.COMBOS.Remove(passiveSkill);
                                            }
                                            else
                                            {
                                                passiveSkill = null;
                                            }

                                            if (myManager.newSkillEvent.data)
                                            {
                                                if (myManager.newSkillEvent.data.GetType() == typeof(ComboSkill))
                                                {
                                                    current.INVENTORY.COMBOS.Add(myManager.newSkillEvent.data as ComboSkill);
                                                    (myManager.newSkillEvent.data as UsableScript).USER = current;
                                                    myManager.CreateEvent(this, myManager.newSkillEvent.data, "New Skill Event", myManager.CheckCount, null, 0, myManager.CountStart);
                                                   // myManager.CreateTextEvent(this, "" + current.FullName + " learned " + (myManager.newSkillEvent.data as ComboSkill).NAME, "new skill event", myManager.CheckText, myManager.TextStart);

                                                    if (myManager.log)
                                                    {
                                                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                                                        myManager.log.Log(coloroption + current.NAME + "</color> learned " + (myManager.newSkillEvent.data as ComboSkill).NAME);
                                                    }
                                                }
                                            }

                                        }

                                        break;
                                    }
                                    else if (((SkillScript)invm.selectedMenuItem.refItem).ELEMENT == Element.Auto)
                                    {
                                        AutoSkill autoSkill = (AutoSkill)invm.selectedMenuItem.refItem;
                                        if (autoSkill)
                                        {
                                            if (current.INVENTORY.AUTOS.Contains(autoSkill))
                                            {
                                                current.INVENTORY.AUTOS.Remove(autoSkill);
                                            }
                                            else
                                            {
                                                autoSkill = null;
                                            }

                                            if (myManager.newSkillEvent.data)
                                            {
                                                if (myManager.newSkillEvent.data.GetType() == typeof(AutoSkill))
                                                {
                                                    current.INVENTORY.AUTOS.Add(myManager.newSkillEvent.data as AutoSkill);
                                                    (myManager.newSkillEvent.data as UsableScript).USER = current;
                                                    myManager.CreateEvent(this, myManager.newSkillEvent.data, "New Skill Event", myManager.CheckCount, null, 0, myManager.CountStart);
                                                    //myManager.CreateTextEvent(this, "" + current.FullName + " learned " + (myManager.newSkillEvent.data as AutoSkill).NAME, "new skill event", myManager.CheckText, myManager.TextStart);

                                                    if (myManager.log)
                                                    {
                                                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                                                        myManager.log.Log(coloroption + current.NAME + "</color> learned " + (myManager.newSkillEvent.data as AutoSkill).NAME);
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                    }
                                    else if (((SkillScript)invm.selectedMenuItem.refItem).ELEMENT == Element.Opp)
                                    {
                                        OppSkill opp = (OppSkill)invm.selectedMenuItem.refItem;

                                        if (opp)
                                        {
                                            if (current.INVENTORY.OPPS.Contains(opp))
                                            {
                                                current.INVENTORY.OPPS.Remove(opp);
                                            }
                                            else
                                            {
                                                opp = null;
                                            }

                                            if (myManager.newSkillEvent.data)
                                            {
                                                if (myManager.newSkillEvent.data.GetType() == typeof(OppSkill))
                                                {
                                                    current.INVENTORY.OPPS.Add(myManager.newSkillEvent.data as OppSkill);
                                                    (myManager.newSkillEvent.data as UsableScript).USER = current;
                                                    myManager.CreateEvent(this, myManager.newSkillEvent.data, "New Skill Event", myManager.CheckCount, null, 0, myManager.CountStart);
                                                   // myManager.CreateTextEvent(this, "" + current.FullName + " learned " + (myManager.newSkillEvent.data as OppSkill).NAME, "new skill event", myManager.CheckText, myManager.TextStart);

                                                    if (myManager.log)
                                                    {
                                                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                                                        myManager.log.Log(coloroption + current.NAME + "</color> learned " + (myManager.newSkillEvent.data as OppSkill).NAME);
                                                    }
                                                }

                                            }
                                        }
                                        break;
                                    }
                                    else
                                    {


                                        CommandSkill selectedSkil = (CommandSkill)invm.selectedMenuItem.refItem;
                                        if (selectedSkil)
                                        {
                                            if (current.INVENTORY.CSKILLS.Contains(selectedSkil))
                                            {
                                                current.INVENTORY.CSKILLS.Remove(selectedSkil);
                                            }
                                            else
                                            {
                                                selectedSkil = null;
                                            }

                                            if (myManager.newSkillEvent.data)
                                            {
                                                if (myManager.newSkillEvent.data.GetType() == typeof(CommandSkill))
                                                {
                                                    current.INVENTORY.CSKILLS.Add(myManager.newSkillEvent.data as CommandSkill);
                                                    (myManager.newSkillEvent.data as UsableScript).USER = current;
                                                    myManager.CreateEvent(this, myManager.newSkillEvent.data, "New Skill Event", myManager.CheckCount, null, 0, myManager.CountStart);
                                                    //myManager.CreateTextEvent(this, "" + current.FullName + " learned " + (myManager.newSkillEvent.data as CommandSkill).NAME, "new skill event", myManager.CheckText, myManager.TextStart);

                                                    if (myManager.log)
                                                    {
                                                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                                                        myManager.log.Log(coloroption + current.NAME + "</color> learned " + (myManager.newSkillEvent.data as CommandSkill).NAME);
                                                    }
                                                }
                                            }

                                        }
                                    }

                                }
                                break;


                            }

                        case 5:
                            {
                                //UseItem();
                                if (invm.selectedMenuItem)
                                {
                                    if (invm.selectedMenuItem.refItem)
                                    {
                                        if (invm.selectedMenuItem.refItem.TYPE == 5)
                                        {
                                            currentItem = (ItemScript)invm.selectedMenuItem.refItem;
                                            if (currentItem)
                                            {

                                                if (current.INVENTORY.ITEMS.Contains(currentItem))
                                                {
                                                    current.INVENTORY.ITEMS.Remove(currentItem);
                                                }
                                                else
                                                {
                                                    currentItem = null;
                                                }

                                                if (myManager.newSkillEvent.data)
                                                {
                                                    if (myManager.newSkillEvent.data.GetType() == typeof(ItemScript))
                                                    {
                                                        current.INVENTORY.ITEMS.Add(myManager.newSkillEvent.data as ItemScript);
                                                        (myManager.newSkillEvent.data as UsableScript).USER = current;

                                                        myManager.CreateEvent(this, myManager.newSkillEvent.data, "New Skill Event", myManager.CheckCount, null, 0, myManager.CountStart);
                                                       // myManager.CreateTextEvent(this, "" + current.FullName + " learned " + (myManager.newSkillEvent.data as ItemScript).NAME, "new skill event", myManager.CheckText, myManager.TextStart);

                                                        if (myManager.log)
                                                        {
                                                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(current.FACTION)) + ">";
                                                            myManager.log.Log(coloroption + current.NAME + "</color> learned " + (myManager.newSkillEvent.data as ItemScript).NAME);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }
        if (myManager)
        {
            myManager.CreateEvent(this, null, "return state event", myManager.BufferedReturnEvent);
            myManager.enterStateTransition();
            myManager.menuManager.ShowNone();
            myManager.newSkillEvent.caller = null;
            myManager.newSkillEvent.data = null;
        }
    }
    //public void useOrEquip(LivingObject living, bool takeAction = true)
    //{
    //    if (living)
    //    {

    //        if (invm.selectedMenuItem)
    //        {
    //            if (invm.selectedMenuItem.refItem)
    //            {
    //                switch (invm.selectedMenuItem.refItem.TYPE)
    //                {
    //                    case 0:
    //                        if (invm.selectedMenuItem.refItem.GetType() == typeof(WeaponScript))
    //                        {
    //                            if (myManager.invManager.menuSide == -1)
    //                            {
    //                                WeaponScript newWeapon = (WeaponScript)invm.selectedMenuItem.refItem;
    //                                living.WEAPON.Equip(newWeapon);
    //                                myManager.ShowGridObjectAffectArea(living);
    //                                myManager.menuManager.ShowExtraCanvas(4, living);
    //                            }
    //                            else
    //                            {
    //                                living.WEAPON.unEquip();
    //                                myManager.menuManager.ShowExtraCanvas(4, living);
    //                                myManager.ShowGridObjectAffectArea(living);

    //                            }
    //                        }
    //                        break;
    //                    case 1:
    //                        if (invm.selectedMenuItem.refItem.GetType() == typeof(ArmorScript))
    //                        {
    //                            if (myManager.invManager.menuSide == -1)
    //                            {
    //                                ArmorScript newArmor = (ArmorScript)invm.selectedMenuItem.refItem;
    //                                living.ARMOR.Equip(newArmor);
    //                                myManager.menuManager.ShowExtraCanvas(5, current);
    //                            }
    //                            else
    //                            {
    //                                living.ARMOR.unEquip();
    //                                //current.ARMOR.NAME = "none";
    //                                //current.ARMOR.DEFENSE = 0;
    //                                //current.ARMOR.RESISTANCE = 0;
    //                                //current.ARMOR.HITLIST = Common.noAmor;
    //                                //for (int i = 0; i < 7; i++)
    //                                //{
    //                                //    current.ARMOR.HITLIST.Add(EHitType.normal);
    //                                //}
    //                            }
    //                            myManager.menuManager.ShowExtraCanvas(5, current);

    //                        }
    //                        break;
    //                    case 2:

    //                        break;
    //                    case 3:

    //                        break;
    //                    case 4:
    //                        {
    //                            if (myManager. GetState() == State.PlayerEquippingSkills)
    //                            {

    //                                if (myManager.invManager.menuSide == -1)
    //                                {

    //                                    myManager.invManager.EquipSkill();
    //                                }
    //                                else
    //                                {

    //                                    myManager.invManager.UnequipSkill();
    //                                }
    //                            }

    //                            else
    //                            {
    //                                if (((SkillScript)invm.selectedMenuItem.refItem).ELEMENT == Element.Passive)
    //                                {
    //                                    break;
    //                                }
    //                                if (((SkillScript)invm.selectedMenuItem.refItem).ELEMENT == Element.Auto)
    //                                {
    //                                    break;
    //                                }
    //                                if (((SkillScript)invm.selectedMenuItem.refItem).ELEMENT == Element.Opp)
    //                                {
    //                                    break;
    //                                }
    //                                CommandSkill selectedSkil = (CommandSkill)invm.selectedMenuItem.refItem;
    //                                if (current.GetComponent<InventoryScript>().ContainsSkillName(selectedSkil.NAME) != null)
    //                                {
    //                                    //  Debug.Log("Got it");
    //                                    selectedSkil = (CommandSkill)current.GetComponent<InventoryScript>().ContainsSkillName(selectedSkil.NAME);
    //                                }
    //                                float modification = 1.0f;
    //                                if (selectedSkil.ETYPE == EType.magical)
    //                                    modification = living.STATS.SPCHANGE;
    //                                if (selectedSkil.ETYPE == EType.physical)
    //                                {
    //                                    if (selectedSkil.COST > 0)
    //                                    {
    //                                        modification = living.STATS.FTCHARGECHANGE;
    //                                    }
    //                                    else
    //                                    {
    //                                        modification = living.STATS.FTCOSTCHANGE;
    //                                    }
    //                                }
    //                                if (selectedSkil.CanUse(modification))
    //                                {

    //                                    currentSkill = selectedSkil;
    //                                    prepareAttack(currentSkill);

    //                                    //  menuStackEntry entry = new menuStackEntry();
    //                                    //  entry.state = State.PlayerAttacking;
    //                                    //  entry.index = invm.currentIndex;
    //                                    //  entry.menu = currentMenu.CmdSkills;
    //                                    //  myManager.enterState(entry);

    //                                }
    //                                else
    //                                {
    //                                    myManager.ShowCantUseText(selectedSkil);
    //                                    myManager.PlayExitSnd();
    //                                }

    //                            }
    //                            break;


    //                        }

    //                    case 5:
    //                        {
    //                            //UseItem();
    //                            if (invm.selectedMenuItem)
    //                            {
    //                                if (invm.selectedMenuItem.refItem)
    //                                {
    //                                    if (invm.selectedMenuItem.refItem.TYPE == 5)
    //                                    {
    //                                        currentItem = (ItemScript)invm.selectedMenuItem.refItem;
    //                                        myManager.attackableTiles = myManager.GetItemUseableTiles(living, currentItem);

    //                                        myManager.ShowWhite();
    //                                        if (myManager.attackableTiles.Count > 0)
    //                                        {
    //                                            for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
    //                                            {
    //                                                for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
    //                                                {
    //                                                    if (currentItem.TTYPE == TargetType.ally)
    //                                                    {
    //                                                        myManager.attackableTiles[i][j].myColor = Common.lime;
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        myManager.attackableTiles[i][j].myColor = Common.pink;// Color.red;
    //                                                    }
    //                                                }
    //                                            }
    //                                            myManager.currentAttackList = myManager.attackableTiles[0];

    //                                            for (int i = 0; i < myManager.currentAttackList.Count; i++)
    //                                            {
    //                                                if (currentItem.TTYPE == TargetType.ally)
    //                                                {
    //                                                    myManager.currentAttackList[i].myColor = Common.green;

    //                                                }
    //                                                else
    //                                                {
    //                                                    myManager.currentAttackList[i].myColor = Common.red;
    //                                                }
    //                                            }

    //                                            myManager.tempObject.transform.position = myManager.currentAttackList[0].transform.position;
    //                                            myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
    //                                            myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];

    //                                            MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
    //                                            if (myMenuManager)
    //                                            {
    //                                                myMenuManager.ShowNone();
    //                                            }
    //                                            myManager.StackNewSelection(State.PlayerUsingItems, currentMenu.CmdItems);
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                        break;
    //                }
    //            }
    //        }
    //    }
    //}
    public void useOppAction(LivingObject oppTarget)
    {
        if (invm.selectedMenuItem)
        {
            if (invm.selectedMenuItem.refItem)
            {
                switch (invm.selectedMenuItem.refItem.TYPE)
                {
                    case 4:
                        {
                            //      Debug.Log("use or atk");
                            if (invm.selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                            {
                                CommandSkill selectedSkill = (CommandSkill)invm.selectedMenuItem.refItem;
                                currentSkill = selectedSkill;
                            }
                            if (invm.selectedMenuItem.refItem.GetType() == typeof(OppSkill))
                            {
                                OppSkill opp = (OppSkill)invm.selectedMenuItem.refItem;
                                if (opp)
                                {
                                    CommandSkill oppSkill = Common.GenericSkill;
                                    oppSkill.ACCURACY = 100;
                                    oppSkill.COST = 0;
                                    oppSkill.ELEMENT = opp.REACTION;
                                    oppSkill.DAMAGE = opp.DAMAGE;
                                    oppSkill.HITS = 1;
                                    oppSkill.SKILLTYPE = SkillType.Command;
                                    oppSkill.SUBTYPE = opp.SUBTYPE;
                                    oppSkill.OWNER = current;
                                    oppSkill.NAME = opp.NAME;
                                    oppSkill.RTYPE = opp.RTYPE;
                                    currentSkill = oppSkill;

                                    prepareAttack(oppSkill, myManager.oppObj);
                                }
                            }
                            // OppUseOrAttack(oppTarget);
                        }
                        break;
                    default:

                        //  Debug.Log("default  use");
                        //  Debug.Log(invm.selectedMenuItem.refItem.NAME + " " + invm.selectedMenuItem.refItem.TYPE);
                        if (invm.selectedMenuItem.refItem.NAME.Equals("ATTACK"))
                        {
                            OppUseOrAttack(oppTarget);
                        }
                        else if (invm.selectedMenuItem.refItem.NAME.Equals("MOVE"))
                        {
                            myManager.ShowGridObjectMoveArea(oppTarget);
                            //menuStackEntry entry = new menuStackEntry();
                            //entry.state = State.PlayerOppMove;
                            //entry.index = invm.currentIndex;
                            //entry.menu = currentMenu.OppOptions;
                            //myManager.enterState(entry);
                            myManager.StackNewSelection(State.PlayerOppMove, currentMenu.OppOptions);
                        }
                        break;
                }
            }

        }
    }
    public void UseOppSkill(int index)
    {
        if (myManager)
        {
            if (current)
            {
                if (index < current.OPP_SLOTS.SKILLS.Count)
                {
                    myManager.OppAttackTargets(current.OPP_SLOTS.lastTarget, current.OPP_SLOTS.SKILLS[index].USER, current.OPP_SLOTS.SKILLS[index] as CommandSkill);
                }
            }
        }

    }

    public void OppUseOrAttack(LivingObject invoker)
    {
        if (!invoker)
        {
            Debug.Log("no opp");
            return;
        }
        bool check = false;
        if (currentSkill != null)
        {


            if (invoker.SSTATUS != SecondaryStatus.seal)
            {

                CommandSkill tempSKill = currentSkill;
                tempSKill.ACCURACY = 100;

                for (int i = 0; i < myManager.opptargets.Count; i++)
                {
                    bool testcheck = myManager.AttackTarget(invoker, myManager.opptargets[i], tempSKill);
                    if (testcheck == true)
                    {
                        check = true;
                    }
                }


                if (check == true)
                {
                    if (myManager.currentObject)
                    {

                        if (myManager.currentObject.GetComponent<LivingObject>())
                        {
                            myManager.currentObject.GetComponent<LivingObject>().OPP_SLOTS.SKILLS.Clear();
                        }
                    }
                    myManager.oppEvent.caller = null;
                    myManager.oppObj = null;
                }

                if (currentSkill != null)
                    currentSkill = null;


            }
        }



        if (check == true)
        {
            if (myManager.currentObject)
            {

                if (myManager.currentObject.GetComponent<LivingObject>())
                {
                    myManager.currentObject.GetComponent<LivingObject>().OPP_SLOTS.SKILLS.Clear();
                }
            }
            myManager.enterStateTransition();
            myManager.oppEvent.caller = null;
            // myManager.CleanMenuStack();

        }
        //   current.TakeAction();
    }

    public void UseOrAttack(bool takeAction = true)
    {
        bool check = false;
        if (myManager.prevState == State.PlayerOppOptions)
        {
            takeAction = false;
        }
        if (currentSkill != null)
        {
            float modification = 1.0f;
            if (currentSkill.ETYPE == EType.magical)
                modification = current.STATS.MANACHANGE;
            if (currentSkill.ETYPE == EType.physical)
            {
                if (currentSkill.COST > 0)
                {
                    modification = current.STATS.FTCHARGECHANGE;
                }
                else
                {
                    modification = current.STATS.FTCOSTCHANGE;
                }
            }
            if (current.SSTATUS != SecondaryStatus.seal)
            {
                if (currentSkill.CanUse(modification))
                {
                    if (current.SSTATUS == SecondaryStatus.confusion)
                    {
                        int chance = Random.Range(0, 2);
                        if (chance <= 2)
                        {
                            Debug.Log("They hit themselves");
                            newTarget.Clear();
                            newTarget.Add(current.currentTile);
                            myManager.SetTargetList(newTarget);
                            check = myManager.AttackTargets(current, currentSkill);
                        }
                        else
                        {
                            LivingObject attacker = current;
                            if (myManager.prevState == State.PlayerOppOptions)
                            {

                                if (myManager.currentObject)
                                {
                                    if (myManager.currentObject.GetComponent<LivingObject>())
                                    {
                                        attacker = myManager.currentObject as LivingObject;
                                    }
                                }
                                check = myManager.AttackTargets(attacker, currentSkill, true);
                            }
                            else
                            {
                                check = myManager.AttackTargets(attacker, currentSkill);
                            }
                        }
                    }
                    else
                    {
                        LivingObject attacker = current;
                        if (myManager.prevState == State.PlayerOppOptions)
                        {
                            if (myManager.currentObject)
                            {
                                if (myManager.currentObject.GetComponent<LivingObject>())
                                {
                                    attacker = myManager.currentObject as LivingObject;
                                }
                            }
                            check = myManager.AttackTargets(attacker, currentSkill, true);
                        }
                        else
                        {
                            check = myManager.AttackTargets(attacker, currentSkill);
                        }

                    }

                    if (check == true)
                    {
                        myManager.invManager.currentIndex = 2;
                        //current.TakeAction();
                        myManager.enterStateTransition();
                        if (currentSkill != null)
                            currentSkill = null;
                    }
                    else
                    {

                        myManager.invManager.currentIndex = 0;

                    }

                }
                else
                {
                    myManager.ShowCantUseText(currentSkill);
                }
            }
        }
        else
        {
            if (current.WEAPON.EQUIPPED != null)
            {
                if (current.WEAPON.CanUse())
                {
                    if (current.SSTATUS == SecondaryStatus.confusion)
                    {
                        int chance = Random.Range(0, 2);

                        if (chance <= 2)
                        {
                            Debug.Log("They hit themselves");
                            newTarget.Clear();
                            newTarget.Add(current.currentTile);
                            myManager.SetTargetList(newTarget);
                            check = myManager.AttackTargets(current, current.WEAPON);
                        }
                    }
                    else
                    {

                        LivingObject attacker = current;
                        if (myManager.prevState == State.PlayerOppOptions)
                        {
                            if (myManager.currentObject)
                            {
                                if (myManager.currentObject.GetComponent<LivingObject>())
                                {
                                    attacker = myManager.currentObject as LivingObject;
                                }
                            }
                            check = myManager.AttackTargets(attacker, current.WEAPON, true);
                        }
                        else
                        {
                            check = myManager.AttackTargets(attacker, current.WEAPON);
                        }
                    }
                }
                else
                {
                    myManager.ShowCantUseText(current.WEAPON.EQUIPPED);
                }
            }


            if (check == true)
            {
                myManager.enterStateTransition();
            }

        }


        if (myManager.prevState == State.PlayerOppOptions)
        {
            myManager.oppEvent.caller = null;
        }
        //  if (check == true)
        // {
        //if (takeAction)
        //{

        //    current.TakeAction();
        //}


        // myManager.menuManager.ShowNone();
        //  }
    }

    public void prepareAttack(CommandSkill attack)
    {
        if (!current)
        {
            return;
        }
        myManager.attackableTiles = myManager.GetSkillsAttackableTiles(current, attack);
        myManager.ShowWhite();
        GridObject possibleObject = null;
        myManager.currentAttackList.Clear();
        if (myManager.attackableTiles.Count > 0)
        {
            for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
            {
                for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                {
                    if (possibleObject == null)
                    {
                        if (myManager.attackableTiles[i][j].isOccupied)
                        {
                            possibleObject = myManager.GetObjectAtTile(myManager.attackableTiles[i][j]);
                            myManager.currentAttackList = myManager.attackableTiles[i];
                            if (possibleObject)
                            {
                                if (possibleObject.FACTION == current.FACTION)
                                {
                                    possibleObject = null;
                                }
                            }
                        }
                    }
                    if (currentSkill.SUBTYPE == SubSkillType.Buff)
                    {
                        myManager.attackableTiles[i][j].MYCOLOR = Common.lime;

                    }
                    else
                    {
                        myManager.attackableTiles[i][j].MYCOLOR = Common.pink;// Color.red;

                    }
                }
            }
            if (myManager.currentAttackList.Count == 0)
            {
                myManager.currentAttackList = myManager.attackableTiles[0];
            }

            if (possibleObject == null)
            {
                myManager.tempObject.transform.position = myManager.currentAttackList[0].transform.position;
                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];
            }
            else
            {
                myManager.tempObject.transform.position = possibleObject.transform.position;
                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];
            }

            for (int i = 0; i < myManager.currentAttackList.Count; i++)
            {
                if (currentSkill.SUBTYPE == SubSkillType.Buff)
                {
                    myManager.currentAttackList[i].MYCOLOR = Common.green;

                }
                else
                {
                    myManager.currentAttackList[i].MYCOLOR = Common.red;
                }
            }



        }

        else
        {
            currentSkill = null;
            myManager.attackableTiles.Clear();
            myManager.tempObject.transform.position = myManager.currentObject.transform.position;
            myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;
        }

        myManager.myCamera.potentialDamage = 0;
        myManager.myCamera.UpdateCamera();
        myManager.anchorHpBar();

        GridObject griddy = possibleObject;//myManager.GetObjectAtTile(myManager.tempObject.GetComponent<GridObject>().currentTile);
        if (possibleObject)
        {
            DmgReaction reac;
            if (griddy.FACTION != myManager.player.current.FACTION)
            {
                if (griddy.GetComponent<LivingObject>())
                {
                    LivingObject livvy = griddy.GetComponent<LivingObject>();
                    if (myManager.player.currentSkill)
                        reac = myManager.CalcDamage(current, livvy, myManager.player.currentSkill, Reaction.none, false);
                    else
                        reac = myManager.CalcDamage(current, livvy, myManager.player.current.WEAPON, Reaction.none, false);
                }
                else
                {
                    if (myManager.player.currentSkill)
                        reac = myManager.CalcDamage(current, griddy, myManager.player.currentSkill, Reaction.none, false);
                    else
                        reac = myManager.CalcDamage(current, griddy, myManager.player.current.WEAPON, Reaction.none, false);
                }
                myManager.myCamera.potentialDamage = reac.damage;
                myManager.myCamera.UpdateCamera();
                if (myManager.potential)
                {
                    myManager.potential.pulsing = true;
                }

            }
        }
        else
        {

        }
        MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
        if (myMenuManager)
        {
            myMenuManager.ShowNone();
        }
        if (currentSkill.ETYPE == EType.physical)
            myManager.StackNewSelection(State.PlayerAttacking, currentMenu.CmdSkills);
        else
            myManager.StackNewSelection(State.PlayerAttacking, currentMenu.CmdSpells);
    }


    public void prepareAttack(CommandSkill attack, LivingObject attacker)
    {
        if (!attacker)
        {
            return;
        }
        myManager.attackableTiles = myManager.GetSkillsAttackableTiles(attacker, attack);
        myManager.ShowWhite();
        GridObject possibleObject = null;
        myManager.currentAttackList.Clear();
        if (myManager.attackableTiles.Count > 0)
        {
            for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
            {
                for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                {
                    if (possibleObject == null)
                    {
                        if (myManager.attackableTiles[i][j].isOccupied)
                        {
                            possibleObject = myManager.GetObjectAtTile(myManager.attackableTiles[i][j]);
                            myManager.currentAttackList = myManager.attackableTiles[i];
                            if (possibleObject)
                            {
                                if (possibleObject.FACTION == attacker.FACTION)
                                {
                                    possibleObject = null;
                                }
                            }
                        }
                    }
                    if (currentSkill.SUBTYPE == SubSkillType.Buff)
                    {
                        myManager.attackableTiles[i][j].MYCOLOR = Common.lime;

                    }
                    else
                    {
                        myManager.attackableTiles[i][j].MYCOLOR = Common.pink;// Color.red;

                    }
                }
            }
            if (myManager.currentAttackList.Count == 0)
            {
                myManager.currentAttackList = myManager.attackableTiles[0];
            }

            if (possibleObject == null)
            {
                myManager.tempObject.transform.position = myManager.currentAttackList[0].transform.position;
                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];
            }
            else
            {
                myManager.tempObject.transform.position = possibleObject.transform.position;
                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                myManager.tempObject.GetComponent<GridObject>().currentTile = possibleObject.currentTile;
            }

            for (int i = 0; i < myManager.currentAttackList.Count; i++)
            {
                if (currentSkill.SUBTYPE == SubSkillType.Buff)
                {
                    myManager.currentAttackList[i].MYCOLOR = Common.green;

                }
                else
                {
                    myManager.currentAttackList[i].MYCOLOR = Common.red;
                }
            }



        }

        else
        {
            currentSkill = null;
            myManager.attackableTiles.Clear();
            myManager.tempObject.transform.position = myManager.currentObject.transform.position;
            myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;
        }
        Debug.Log("1");
        myManager.myCamera.potentialDamage = 0;
        myManager.myCamera.UpdateCamera();
        myManager.anchorHpBar();

        GridObject griddy = myManager.GetObjectAtTile(myManager.tempObject.GetComponent<GridObject>().currentTile);
        if (griddy)
        {
            DmgReaction reac;
            if (griddy.FACTION != attacker.FACTION)
            {
                if (griddy.GetComponent<LivingObject>())
                {
                    LivingObject livvy = griddy.GetComponent<LivingObject>();
                    if (myManager.player.currentSkill)
                        reac = myManager.CalcDamage(attacker, livvy, currentSkill, Reaction.none, false);
                    else
                        reac = myManager.CalcDamage(attacker, livvy, attacker.WEAPON, Reaction.none, false);
                }
                else
                {
                    if (myManager.player.currentSkill)
                        reac = myManager.CalcDamage(attacker, griddy, currentSkill, Reaction.none, false);
                    else
                        reac = myManager.CalcDamage(attacker, griddy, attacker.WEAPON, Reaction.none, false);
                }
     
                myManager.myCamera.potentialDamage = reac.damage;
                myManager.myCamera.UpdateCamera();
                if (myManager.potential)
                {
                    myManager.potential.pulsing = true;
                }

            }
        }
        MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
        if (myMenuManager)
        {
            myMenuManager.ShowNone();
        }
        if (currentSkill.ETYPE == EType.physical)
            myManager.StackNewSelection(State.PlayerAttacking, currentMenu.CmdSkills);
        else
            myManager.StackNewSelection(State.PlayerAttacking, currentMenu.CmdSpells);
    }


    public void prepareAttack(WeaponScript attack)
    {

        if (!current)
        {
            return;
        }
        myManager.player.current.WEAPON.Equip(attack);
        myManager.attackableTiles = myManager.GetAttackableTiles(current, attack);
        myManager.ShowWhite();
        GridObject possibleObject = null;
        myManager.currentAttackList.Clear();
        if (myManager.attackableTiles.Count > 0)
        {
            for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
            {
                for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                {
                    if (possibleObject == null)
                    {
                        if (myManager.attackableTiles[i][j].isOccupied)
                        {
                            possibleObject = myManager.GetObjectAtTile(myManager.attackableTiles[i][j]);
                            myManager.currentAttackList = myManager.attackableTiles[i];
                            if (possibleObject)
                            {
                                if (possibleObject.FACTION == current.FACTION)
                                {
                                    possibleObject = null;
                                }
                            }
                        }
                    }

                    myManager.attackableTiles[i][j].MYCOLOR = Common.pink;// Color.red;


                }
            }
            if (myManager.currentAttackList.Count == 0)
            {
                myManager.currentAttackList = myManager.attackableTiles[0];
            }

            if (possibleObject == null)
            {
                myManager.tempObject.transform.position = myManager.currentAttackList[0].transform.position;
                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];
            }
            else
            {
                myManager.tempObject.transform.position = possibleObject.transform.position;
                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                myManager.tempObject.GetComponent<GridObject>().currentTile = possibleObject.currentTile;
            }

            for (int i = 0; i < myManager.currentAttackList.Count; i++)
            {

                myManager.currentAttackList[i].MYCOLOR = Common.red;

            }



        }

        else
        {
            currentSkill = null;
            myManager.attackableTiles.Clear();
            myManager.tempObject.transform.position = myManager.currentObject.transform.position;
            myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;
        }
        myManager.myCamera.potentialDamage = 0;
        myManager.myCamera.UpdateCamera();
        myManager.anchorHpBar();

        GridObject griddy = myManager.GetObjectAtTile(myManager.tempObject.GetComponent<GridObject>().currentTile);
        if (griddy)
        {
            DmgReaction reac;
            if (griddy.FACTION != myManager.player.current.FACTION)
            {
                if (griddy.GetComponent<LivingObject>())
                {
                    LivingObject livvy = griddy.GetComponent<LivingObject>();

                    reac = myManager.CalcDamage(current, livvy, myManager.player.current.WEAPON, Reaction.none, false);
                }
                else
                {

                    reac = myManager.CalcDamage(current, griddy, myManager.player.current.WEAPON, Reaction.none, false);
                }
             

                myManager.myCamera.potentialDamage = reac.damage;
                myManager.myCamera.UpdateCamera();
                if (myManager.potential)
                {
                    myManager.potential.pulsing = true;
                }

            }
        }
        MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
        if (myMenuManager)
        {
            myMenuManager.ShowNone();
        }

        myManager.StackNewSelection(State.PlayerAttacking, currentMenu.Strikes);

    }
    public void prepareAttack(ItemScript attack)
    {
        if (!current)
        {
            return;
        }
        myManager.attackableTiles = myManager.GetItemUseableTiles(current, currentItem);
        myManager.ShowWhite();
        GridObject possibleObject = null;
        myManager.currentAttackList.Clear();
        if (myManager.attackableTiles.Count > 0)
        {
            for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
            {
                for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                {
                    if (possibleObject == null)
                    {
                        if (myManager.attackableTiles[i][j].isOccupied)
                        {
                            possibleObject = myManager.GetObjectAtTile(myManager.attackableTiles[i][j]);
                            myManager.currentAttackList = myManager.attackableTiles[i];
                            if (possibleObject)
                            {
                                if (possibleObject.FACTION == current.FACTION)
                                {
                                    possibleObject = null;
                                }
                            }
                        }
                    }

                    if (currentItem.ITYPE == ItemType.dmg || currentItem.ITYPE == ItemType.dart)
                    {
                        myManager.attackableTiles[i][j].MYCOLOR = Common.pink;// Color.red;

                    }
                    else
                    {
                        myManager.attackableTiles[i][j].MYCOLOR = Common.lime;

                    }


                }
            }
            if (myManager.currentAttackList.Count == 0)
            {
                myManager.currentAttackList = myManager.attackableTiles[0];
            }

            if (possibleObject == null)
            {
                myManager.tempObject.transform.position = myManager.currentAttackList[0].transform.position;
                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];
            }
            else
            {
                myManager.tempObject.transform.position = possibleObject.transform.position;
                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                myManager.tempObject.GetComponent<GridObject>().currentTile = possibleObject.currentTile;
            }

            for (int i = 0; i < myManager.currentAttackList.Count; i++)
            {

                if (currentItem.ITYPE == ItemType.dmg || currentItem.ITYPE == ItemType.dart)
                {
                    myManager.currentAttackList[i].MYCOLOR = Common.red;

                }
                else
                {
                    myManager.currentAttackList[i].MYCOLOR = Common.green;

                }


            }



        }

        else
        {
            currentSkill = null;
            myManager.attackableTiles.Clear();
            myManager.tempObject.transform.position = myManager.currentObject.transform.position;
            myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;
        }
        myManager.myCamera.potentialDamage = 0;
        myManager.myCamera.UpdateCamera();
        myManager.anchorHpBar();

        GridObject griddy = myManager.GetObjectAtTile(myManager.tempObject.GetComponent<GridObject>().currentTile);
        if (griddy)
        {
            DmgReaction reac;
            if (griddy.FACTION != myManager.player.current.FACTION)
            {
                if (griddy.GetComponent<LivingObject>())
                {
                    LivingObject livvy = griddy.GetComponent<LivingObject>();

                    reac = myManager.CalcDamage(current, livvy, currentSkill, Reaction.none, false);
                }
                else
                {

                    reac = myManager.CalcDamage(current, griddy, currentSkill, Reaction.none, false);
                }
           
                myManager.myCamera.potentialDamage = reac.damage;
                myManager.myCamera.UpdateCamera();
                if (myManager.potential)
                {
                    myManager.potential.pulsing = true;
                }

            }
        }
        MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
        if (myMenuManager)
        {
            myMenuManager.ShowNone();
        }

        myManager.StackNewSelection(State.PlayerUsingItems, currentMenu.CmdItems);
    }

    public void BuffereduseOrEquip()
    {

        useOrEquip();
    }


    public bool ShowCmd(Object data)
    {
        if (myManager.oppEvent.caller == null)
        {
            if (current)
            {
                SpriteRenderer sr = current.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    sr.color = Color.white;
                }
                current.TakeAction();

            }
        }


        if (current != null)
        {


            if (!current.DEAD && current)
            {
                if (current.ACTIONS > 1)
                {
                    myManager.CleanMenuStack();
                    myManager.ShowGridObjectAffectArea(current);
                }
                else
                {
                    myManager.CleanMenuStack(true);
                }
            }
            else
            {
                myManager.CleanMenuStack(true);
            }
            // myManager.MoveCameraAndShow(current);

        }
        else
        {
            myManager.CleanMenuStack(true);
            LivingObject lastCurrentObject = data as LivingObject;
            if (lastCurrentObject != null)
            {
                Debug.Log("call for " + lastCurrentObject.FullName);
                myManager.MoveCameraAndShow(lastCurrentObject);
            }
            else
            {
                Debug.Log("last was null");
            }
        }

        return true;
    }
    public bool BeforeOpp(Object data)
    {
        myManager.returnState();
        myManager.returnState();
        myManager.returnState();
        return true;
    }
}
