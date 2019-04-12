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
                    }
                    break;

                case State.PlayerSelectItem:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {

                            UseItem();
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

    public void UseItem()
    {
        if (invm.selectedMenuItem)
        {
            if (invm.selectedMenuItem.refItem)
            {
                if (invm.selectedMenuItem.refItem.TYPE == 5)
                {
                    ItemScript selectedItem = (ItemScript)invm.selectedMenuItem.refItem;
                    bool useditem = selectedItem.useItem(current);
                    if (useditem == true || selectedItem.ITYPE == ItemType.actionBoost)
                    {
                        current.INVENTORY.ITEMS.Remove(selectedItem);
                        myManager.menuManager.ShowItemCanvas(11, current);
                        current.TakeAction();
                    }
                    if(selectedItem.ITYPE == ItemType.actionBoost && useditem == false)
                    {
                        myManager.CreateTextEvent(this, "Chance of gaining action points failed", "action item", myManager.CheckText, myManager.TextStart);
                    }
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
                                      //  menuStackEntry entry = new menuStackEntry();
                                      //  entry.state = State.PlayerAttacking;
                                      //  entry.index = invm.currentIndex;
                                      //  entry.menu = currentMenu.CmdSkills;
                                      //  myManager.enterState(entry);

                                        myManager.StackNewSelection(State.PlayerAttacking, currentMenu.CmdSkills);
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
                                UseItem();
                               // myManager.attackableTiles = myManager.GetAdjecentTiles(current);
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
                if(currentSkill.COST > 0)
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
                            check = myManager.AttackTargets(invoker, tempSKill);
                        }
                    }
                    else
                    {
                        check = myManager.AttackTargets(invoker, tempSKill);

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

    public bool ShowCmd(Object data)
    {
        if(myManager.oppEvent.caller == null)
        {
            if(current)
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
