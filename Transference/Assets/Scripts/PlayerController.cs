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
        myManager = GameObject.FindObjectOfType<ManagerScript>();
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
                            current.TakeAction();
                            // myManager.ComfirmMoveGridObject(current,myManager.GetTileIndex(current));
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        myManager.CancelMenuAction(current);
                    }
                    break;
                case State.PlayerAttacking:

                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        UseOrAttack();

                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        // myManager.invManager.currentIndex = myManager.invManager.prevIndex;
                        // myManager.returnState();
                        myManager.CancelMenuAction(current);
                        currentSkill = null;
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
                            useOrEquip();
                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            // myManager.invManager.currentIndex = myManager.invManager.prevIndex;
                            myManager.CancelMenuAction(current);
                            currentSkill = null;

                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            currentSkill = null;
                            myManager.CancelMenuAction(current);
                        }
                    }
                    break;
                case State.playerUsingSkills:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            //  useOrEquip();
                            myManager.CreateEvent(this, null, "buffered use", myManager.ReturnTrue, BuffereduseOrEquip);

                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            // myManager.invManager.currentIndex = myManager.invManager.prevIndex;
                            myManager.CancelMenuAction(current);
                            currentSkill = null;

                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            currentSkill = null;
                            myManager.CancelMenuAction(current);
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

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {

                        myManager.CancelMenuAction(current);
                    }
                    if (Input.GetMouseButtonDown(1))
                    {

                        myManager.CancelMenuAction(current);
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

                            //  UseItem();
                            myManager.CreateEvent(this, null, "buffered use", myManager.ReturnTrue, BuffereduseOrEquip);
                            //   useOrEquip();
                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            myManager.CancelMenuAction(current);
                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            myManager.CancelMenuAction(current);
                        }
                    }
                    break;


                default:
                    break;
            }


        }
    }

    public void UseItem(LivingObject target, TileScript TargetTile)
    {
        if (currentItem)
        {
            bool useditem = false;
            if (!target && TargetTile)
            {
                if (currentItem.TTYPE != TargetType.adjecent)
                {
                    return;
                }
                useditem = currentItem.useItem(current, TargetTile);
                if (useditem == true)
                {
                    GridAnimationObj gao = null;
                    gao = myManager.PrepareGridAnimation(null, current);
                    gao.type = -2;
                    gao.magnitute = 0;
                    gao.LoadGridAnimation();

                  myManager.  CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

                    myManager.CreateTextEvent(this, current.NAME + " used " + currentItem.NAME, "item used", myManager.CheckText, myManager.TextStart);
                    current.INVENTORY.ITEMS.Remove(currentItem);

                    current.TakeAction();
                    if (current.ACTIONS > 0)
                        myManager.returnState();
                    currentItem = null;
                }
            }
            if (target)
            {

                if (target.FACTION == Faction.ally && currentItem.TTYPE != TargetType.ally)
                {
                    myManager.CreateTextEvent(this, "Cannot use " + currentItem.NAME + " on " + target.NAME, "item invalid target", myManager.CheckText, myManager.TextStart);
                    return;
                }
                if (target.FACTION == Faction.enemy && currentItem.TTYPE == TargetType.ally)
                {
                    myManager.CreateTextEvent(this, "Cannot use " + currentItem.NAME + " on " + target.NAME, "item invalid target", myManager.CheckText, myManager.TextStart);
                    return;
                }

                useditem = currentItem.useItem(target);

                if (useditem == false)
                {
                    switch (currentItem.ITYPE)
                    {
                        case ItemType.healthPotion:
                            myManager.CreateTextEvent(this, "Target health is full", "action item", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.manaPotion:
                            myManager.CreateTextEvent(this, "Target mana is full", "action item", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.fatiguePotion:
                            myManager.CreateTextEvent(this, "Target FT couldn't change. ", "item invalid target", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.cure:
                            myManager.CreateTextEvent(this, "Target has no ailments. ", "item invalid target", myManager.CheckText, myManager.TextStart);
                            break;
                        case ItemType.buff:
                            break;
                        case ItemType.dmg:
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
                    myManager.CreateTextEvent(this, current.NAME + " used " + currentItem.NAME + " on " + target.NAME, "item used", myManager.CheckText, myManager.TextStart);
                    current.INVENTORY.ITEMS.Remove(currentItem);
                    //   myManager.menuManager.ShowItemCanvas(11, current);
                    current.TakeAction();
                    if (current.ACTIONS > 0)
                        myManager.returnState();
                    currentItem = null;
                }

            }
        }
    }

    public void useOrEquip(bool takeAction = true)
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
                                if (myManager.invManager.menuSide == -1)
                                {
                                    WeaponScript newWeapon = (WeaponScript)invm.selectedMenuItem.refItem;
                                    current.WEAPON.Equip(newWeapon);
                                    myManager.ShowGridObjectAffectArea(current);
                                    myManager.menuManager.ShowExtraCanvas(4, current);
                                }
                                else
                                {
                                    current.WEAPON.unEquip();
                                    myManager.menuManager.ShowExtraCanvas(4, current);
                                    myManager.ShowGridObjectAffectArea(current);

                                }
                            }
                            break;
                        case 1:
                            if (invm.selectedMenuItem.refItem.GetType() == typeof(ArmorScript))
                            {
                                if (myManager.invManager.menuSide == -1)
                                {
                                    ArmorScript newArmor = (ArmorScript)invm.selectedMenuItem.refItem;
                                    current.ARMOR.Equip(newArmor);
                                    myManager.menuManager.ShowExtraCanvas(5, current);
                                }
                                else
                                {
                                    current.ARMOR.unEquip();
                                    //current.ARMOR.NAME = "none";
                                    //current.ARMOR.DEFENSE = 0;
                                    //current.ARMOR.RESISTANCE = 0;
                                    //current.ARMOR.HITLIST = Common.noAmor;
                                    //for (int i = 0; i < 7; i++)
                                    //{
                                    //    current.ARMOR.HITLIST.Add(EHitType.normal);
                                    //}
                                }
                                myManager.menuManager.ShowExtraCanvas(5, current);

                            }
                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                        case 4:
                            {
                                if (myManager.currentState == State.PlayerEquippingSkills)
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
                                        modification = current.STATS.SPCHANGE;
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
                                        myManager.attackableTiles = myManager.GetSkillsAttackableTiles(current, selectedSkil);
                                        myManager.ShowWhite();
                                        if (myManager.attackableTiles.Count > 0)
                                        {
                                            for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
                                            {
                                                for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                                                {
                                                    if (currentSkill.SUBTYPE == SubSkillType.Buff)
                                                    {
                                                        myManager.attackableTiles[i][j].myColor = Common.lime;

                                                    }
                                                    else
                                                    {
                                                        myManager.attackableTiles[i][j].myColor = Common.pink;// Color.red;

                                                    }
                                                }
                                            }
                                            myManager.currentAttackList = myManager.attackableTiles[0];

                                            for (int i = 0; i < myManager.currentAttackList.Count; i++)
                                            {
                                                if (currentSkill.SUBTYPE == SubSkillType.Buff)
                                                {
                                                    myManager.currentAttackList[i].myColor = Common.green;

                                                }
                                                else
                                                {
                                                    myManager.currentAttackList[i].myColor = Common.red;
                                                }
                                            }

                                            myManager.tempObject.transform.position = myManager.currentAttackList[0].transform.position;
                                            myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                                            myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];


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
                                            if (griddy.GetComponent<LivingObject>())
                                            {
                                                LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                if (livvy.FACTION != myManager.player.current.FACTION)
                                                {
                                                    DmgReaction reac;
                                                    if (myManager.player.currentSkill)
                                                        reac = myManager.CalcDamage(myManager.player.current, livvy, myManager.player.currentSkill, Reaction.none, false);
                                                    else
                                                        reac = myManager.CalcDamage(myManager.player.current, livvy, myManager.player.current.WEAPON, Reaction.none, false);
                                                    if (reac.reaction > Reaction.weak)
                                                        reac.damage = 0;
                                                    myManager.myCamera.potentialDamage = reac.damage;
                                                    myManager.myCamera.UpdateCamera();
                                                    if (myManager.potential)
                                                    {
                                                        myManager.potential.pulsing = true;
                                                    }

                                                }
                                            }
                                        }
                                        MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
                                        if (myMenuManager)
                                        {
                                            myMenuManager.ShowNone();
                                        }
                                        myManager.tempObject.transform.position = myManager.currentObject.transform.position;
                                        myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;
                                        if (currentSkill.ETYPE == EType.physical)
                                            myManager.StackNewSelection(State.PlayerAttacking, currentMenu.CmdSkills);
                                        else
                                            myManager.StackNewSelection(State.PlayerAttacking, currentMenu.CmdSpells);
                                        //  menuStackEntry entry = new menuStackEntry();
                                        //  entry.state = State.PlayerAttacking;
                                        //  entry.index = invm.currentIndex;
                                        //  entry.menu = currentMenu.CmdSkills;
                                        //  myManager.enterState(entry);

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
                                            myManager.attackableTiles = myManager.GetItemUseableTiles(current, currentItem);

                                            myManager.ShowWhite();
                                            if (myManager.attackableTiles.Count > 0)
                                            {
                                                for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
                                                {
                                                    for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                                                    {
                                                        if (currentItem.TTYPE == TargetType.ally)
                                                        {
                                                            myManager.attackableTiles[i][j].myColor = Common.lime;
                                                        }
                                                        else
                                                        {
                                                            myManager.attackableTiles[i][j].myColor = Common.pink;// Color.red;
                                                        }
                                                    }
                                                }
                                                myManager.currentAttackList = myManager.attackableTiles[0];

                                                for (int i = 0; i < myManager.currentAttackList.Count; i++)
                                                {
                                                    if (currentItem.TTYPE == TargetType.ally)
                                                    {
                                                        myManager.currentAttackList[i].myColor = Common.green;

                                                    }
                                                    else
                                                    {
                                                        myManager.currentAttackList[i].myColor = Common.red;
                                                    }
                                                }

                                                myManager.tempObject.transform.position = myManager.currentAttackList[0].transform.position;
                                                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                                                myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];

                                                MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
                                                if (myMenuManager)
                                                {
                                                    myMenuManager.ShowNone();
                                                }
                                                myManager.StackNewSelection(State.PlayerUsingItems, currentMenu.CmdItems);
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
    }
    public void useOrEquip(LivingObject living, bool takeAction = true)
    {
        if (living)
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
                                if (myManager.invManager.menuSide == -1)
                                {
                                    WeaponScript newWeapon = (WeaponScript)invm.selectedMenuItem.refItem;
                                    living.WEAPON.Equip(newWeapon);
                                    myManager.ShowGridObjectAffectArea(living);
                                    myManager.menuManager.ShowExtraCanvas(4, living);
                                }
                                else
                                {
                                    living.WEAPON.unEquip();
                                    myManager.menuManager.ShowExtraCanvas(4, living);
                                    myManager.ShowGridObjectAffectArea(living);

                                }
                            }
                            break;
                        case 1:
                            if (invm.selectedMenuItem.refItem.GetType() == typeof(ArmorScript))
                            {
                                if (myManager.invManager.menuSide == -1)
                                {
                                    ArmorScript newArmor = (ArmorScript)invm.selectedMenuItem.refItem;
                                    living.ARMOR.Equip(newArmor);
                                    myManager.menuManager.ShowExtraCanvas(5, current);
                                }
                                else
                                {
                                    living.ARMOR.unEquip();
                                    //current.ARMOR.NAME = "none";
                                    //current.ARMOR.DEFENSE = 0;
                                    //current.ARMOR.RESISTANCE = 0;
                                    //current.ARMOR.HITLIST = Common.noAmor;
                                    //for (int i = 0; i < 7; i++)
                                    //{
                                    //    current.ARMOR.HITLIST.Add(EHitType.normal);
                                    //}
                                }
                                myManager.menuManager.ShowExtraCanvas(5, current);

                            }
                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                        case 4:
                            {
                                if (myManager.currentState == State.PlayerEquippingSkills)
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
                                        modification = living.STATS.SPCHANGE;
                                    if (selectedSkil.ETYPE == EType.physical)
                                    {
                                        if (selectedSkil.COST > 0)
                                        {
                                            modification = living.STATS.FTCHARGECHANGE;
                                        }
                                        else
                                        {
                                            modification = living.STATS.FTCOSTCHANGE;
                                        }
                                    }
                                    if (selectedSkil.CanUse(modification))
                                    {

                                        currentSkill = selectedSkil;
                                        myManager.attackableTiles = myManager.GetSkillsAttackableTiles(living, selectedSkil);
                                        myManager.ShowWhite();
                                        if (myManager.attackableTiles.Count > 0)
                                        {
                                            for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
                                            {
                                                for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                                                {
                                                    if (currentSkill.SUBTYPE == SubSkillType.Buff)
                                                    {
                                                        myManager.attackableTiles[i][j].myColor = Common.lime;

                                                    }
                                                    else
                                                    {
                                                        myManager.attackableTiles[i][j].myColor = Common.pink;// Color.red;

                                                    }
                                                }
                                            }
                                            myManager.currentAttackList = myManager.attackableTiles[0];

                                            for (int i = 0; i < myManager.currentAttackList.Count; i++)
                                            {
                                                if (currentSkill.SUBTYPE == SubSkillType.Buff)
                                                {
                                                    myManager.currentAttackList[i].myColor = Common.green;

                                                }
                                                else
                                                {
                                                    myManager.currentAttackList[i].myColor = Common.red;
                                                }
                                            }

                                            myManager.tempObject.transform.position = myManager.currentAttackList[0].transform.position;
                                            myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                                            myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];


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
                                            if (griddy.GetComponent<LivingObject>())
                                            {
                                                LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                if (livvy.FACTION != myManager.player.current.FACTION)
                                                {
                                                    DmgReaction reac;
                                                    if (myManager.player.currentSkill)
                                                        reac = myManager.CalcDamage(living, livvy, myManager.player.currentSkill, Reaction.none, false);
                                                    else
                                                        reac = myManager.CalcDamage(living, livvy, myManager.player.current.WEAPON, Reaction.none, false);
                                                    if (reac.reaction > Reaction.weak)
                                                        reac.damage = 0;
                                                    myManager.myCamera.potentialDamage = reac.damage;
                                                    myManager.myCamera.UpdateCamera();
                                                    if (myManager.potential)
                                                    {
                                                        myManager.potential.pulsing = true;
                                                    }

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
                                        //  menuStackEntry entry = new menuStackEntry();
                                        //  entry.state = State.PlayerAttacking;
                                        //  entry.index = invm.currentIndex;
                                        //  entry.menu = currentMenu.CmdSkills;
                                        //  myManager.enterState(entry);

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
                                            myManager.attackableTiles = myManager.GetItemUseableTiles(living, currentItem);

                                            myManager.ShowWhite();
                                            if (myManager.attackableTiles.Count > 0)
                                            {
                                                for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
                                                {
                                                    for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                                                    {
                                                        if (currentItem.TTYPE == TargetType.ally)
                                                        {
                                                            myManager.attackableTiles[i][j].myColor = Common.lime;
                                                        }
                                                        else
                                                        {
                                                            myManager.attackableTiles[i][j].myColor = Common.pink;// Color.red;
                                                        }
                                                    }
                                                }
                                                myManager.currentAttackList = myManager.attackableTiles[0];

                                                for (int i = 0; i < myManager.currentAttackList.Count; i++)
                                                {
                                                    if (currentItem.TTYPE == TargetType.ally)
                                                    {
                                                        myManager.currentAttackList[i].myColor = Common.green;

                                                    }
                                                    else
                                                    {
                                                        myManager.currentAttackList[i].myColor = Common.red;
                                                    }
                                                }

                                                myManager.tempObject.transform.position = myManager.currentAttackList[0].transform.position;
                                                myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));
                                                myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentAttackList[0];

                                                MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
                                                if (myMenuManager)
                                                {
                                                    myMenuManager.ShowNone();
                                                }
                                                myManager.StackNewSelection(State.PlayerUsingItems, currentMenu.CmdItems);
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
    }
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
                            if (invm.selectedMenuItem.refItem.GetType() != typeof(UsableScript))
                            {
                                CommandSkill selectedSkill = (CommandSkill)invm.selectedMenuItem.refItem;
                                currentSkill = selectedSkill;
                            }
                            OppUseOrAttack(oppTarget);
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
    public void OppUseOrAttack(LivingObject invoker)
    {
        bool check = false;
        if (currentSkill != null)
        {
            float modification = 1.0f;
            if (currentSkill.ETYPE == EType.magical)
                modification = invoker.STATS.SPCHANGE;
            if (currentSkill.ETYPE == EType.physical)
            {
                if (currentSkill.COST > 0)
                {
                    modification = invoker.STATS.FTCHARGECHANGE;
                }
                else
                {
                    modification = invoker.STATS.FTCOSTCHANGE;
                }
            }

            if (invoker.SSTATUS != SecondaryStatus.seal)
            {
                if (currentSkill.CanUse(modification))
                {
                    CommandSkill tempSKill = currentSkill;
                    tempSKill.ACCURACY = 100;
                    if (invoker.SSTATUS == SecondaryStatus.confusion)
                    {
                        int chance = Random.Range(0, 2);
                        if (chance <= 0)
                        {
                            Debug.Log("They hit themselves");
                            newTarget.Clear();
                            newTarget.Add(invoker.currentTile);
                            myManager.SetTargetList(newTarget);
                            check = myManager.AttackTargets(invoker, tempSKill);
                        }
                        else
                        {
                            //check = myManager.AttackTargets(invoker, tempSKill);
                            for (int i = 0; i < myManager.opptargets.Count; i++)
                            {
                                bool testcheck = myManager.AttackTarget(invoker, myManager.opptargets[i], tempSKill);
                                if(testcheck == true)
                                {
                                    check = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < myManager.opptargets.Count; i++)
                        {
                            bool testcheck = myManager.AttackTarget(invoker, myManager.opptargets[i], tempSKill);
                            if (testcheck == true)
                            {
                                check = true;
                            }
                        }
                    }

                    if (check == true)
                    {
                        myManager.oppEvent.caller = null;
                        myManager.oppObj = null;
                    }

                    if (currentSkill != null)
                        currentSkill = null;

                }
                else
                {
                    myManager.ShowCantUseText(currentSkill);
                }
            }
        }
        else
        {
            WeaponEquip tempWeapon = invoker.WEAPON;

            if (invoker.SSTATUS == SecondaryStatus.confusion)
            {
                int chance = Random.Range(0, 2);

                if (chance <= 0)
                {
                    Debug.Log("They hit themselves");
                    newTarget.Clear();
                    newTarget.Add(invoker.currentTile);
                    myManager.SetTargetList(newTarget);
                    check = myManager.AttackTargets(invoker, tempWeapon);
                }
            }
            else
            {
                check = myManager.AttackTargets(invoker, tempWeapon);

            }
        }


        if (check == true)
        {
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
                modification = current.STATS.SPCHANGE;
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
                            check = myManager.AttackTargets(current, currentSkill);
                        }
                    }
                    else
                    {
                        check = myManager.AttackTargets(current, currentSkill);

                    }

                    if (check == true)
                    {
                        myManager.invManager.currentIndex = 2;
                        //current.TakeAction();
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
            }
            else
            {
                check = myManager.AttackTargets(current, current.WEAPON);
            }
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
                current.TakeAction();

            }
        }
        myManager.CleanMenuStack();
        myManager.ShowGridObjectAffectArea(current);
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
