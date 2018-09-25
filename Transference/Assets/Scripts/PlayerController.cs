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
                            if (invm.selectedMenuItem)
                            {
                                if (invm.selectedMenuItem.refItem)
                                {
                                    if (invm.selectedMenuItem.refItem.TYPE == 5)
                                    {
                                        ItemScript selectedItem = (ItemScript)invm.selectedMenuItem.refItem;
                                        if (selectedItem.useItem(current) == true)
                                        {
                                            Debug.Log("Health: " + current.HEALTH);
                                            Debug.Log("Stats health " + current.STATS.HEALTH);
                                            current.TakeAction();
                                            current.INVENTORY.ITEMS.Remove(selectedItem);
                                            invm.Validate("Player controller used item");
                                        }
                                    }
                                }
                            }

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
    public void useOrEquip(bool takeAction = true)
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
                            WeaponScript newWeapon = (WeaponScript)invm.selectedMenuItem.refItem;
                            current.WEAPON.Equip(newWeapon);
                            myManager.invManager.Validate("player controller for equipping");
                        }
                        break;
                    case 1:
                        if (invm.selectedMenuItem.refItem.GetType() == typeof(ArmorScript))
                        {
                            ArmorScript newArmor = (ArmorScript)invm.selectedMenuItem.refItem;
                            current.ARMOR.Equip(newArmor);
                            myManager.invManager.Validate("player controller for equipping");
                        }
                        break;
                    case 2:
                        if (invm.selectedMenuItem.refItem.GetType() == typeof(AccessoryScript))
                        {
                            AccessoryScript newAcc = (AccessoryScript)invm.selectedMenuItem.refItem;
                            current.ACCESSORY.Equip(newAcc);
                            myManager.invManager.Validate("player controller for equipping");
                        }
                        break;
                    case 3:
                        if (invm.selectedMenuItem.refItem.GetType() == current.ACCESSORY.GetType())
                        {

                        }
                        break;
                    case 4:
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

                            if (myManager.currentState == State.PlayerEquippingSkills)
                            {
                                Debug.Log("made it to use or equip");
                                if (myManager.invManager.menuSide == -1)
                                {
                                    Debug.Log("  equipin");

                                    myManager.invManager.EquipSkill();
                                }
                                else
                                {
                                    Debug.Log("  eun quipin");

                                    myManager.invManager.UnequipSkill();
                                }
                            }
                            else
                            {

                                if (selectedSkil.CanUse())
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

                                                myManager.attackableTiles[i][j].myColor = myManager.pink;// Color.red;
                                            }
                                        }
                                        myManager.currentAttackList = myManager.attackableTiles[0];

                                        for (int i = 0; i < myManager.currentAttackList.Count; i++)
                                        {
                                            myManager.currentAttackList[i].myColor = Color.red;
                                        }


                                    }

                                    else
                                    {
                                        currentSkill = null;
                                        myManager.attackableTiles.Clear();

                                    }
                                    myManager.tempObject.transform.position = myManager.currentObject.transform.position;
                                    myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;
                                    MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
                                    if (myMenuManager)
                                    {
                                        myMenuManager.ShowNone();
                                    }
                                    menuStackEntry entry = new menuStackEntry();
                                    entry.state = State.PlayerAttacking;
                                    entry.index = invm.currentIndex;
                                    entry.menu = currentMenu.CmdSkills;
                                    myManager.enterState(entry);
                                }
                                else
                                {
                                    myManager.PlayExitSnd();
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
                            Debug.Log("use or atk");
                            if (invm.selectedMenuItem.refItem.GetType() != typeof(UsableScript))
                            {
                                CommandSkill selectedSkill = (CommandSkill)invm.selectedMenuItem.refItem;
                                currentSkill = selectedSkill;
                            }
                            OppUseOrAttack(oppTarget);
                        }
                        break;
                    default:

                        Debug.Log("default  use");
                        Debug.Log(invm.selectedMenuItem.refItem.NAME + " " + invm.selectedMenuItem.refItem.TYPE);
                        if (invm.selectedMenuItem.refItem.NAME.Equals("ATTACK"))
                        {
                            OppUseOrAttack(oppTarget);
                        }
                        else if (invm.selectedMenuItem.refItem.NAME.Equals("MOVE"))
                        {
                            myManager.ShowGridObjectMoveArea(oppTarget);
                            menuStackEntry entry = new menuStackEntry();
                            entry.state = State.PlayerOppMove;
                            entry.index = invm.currentIndex;
                            entry.menu = currentMenu.OppOptions;
                            myManager.enterState(entry);
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
                modification = invoker.STATS.FTCHANGE;
            if (invoker.SSTATUS != SecondaryStatus.seal)
            {
                if (currentSkill.CanUse(modification))
                {
                    if (invoker.SSTATUS == SecondaryStatus.confusion)
                    {
                        int chance = Random.Range(0, 2);
                        if (chance <= 0)
                        {
                            Debug.Log("They hit themselves");
                            newTarget.Clear();
                            newTarget.Add(invoker.currentTile);
                            myManager.SetTargetList(newTarget);
                            check = myManager.AttackTargets(invoker, currentSkill);
                        }
                        else
                        {
                            check = myManager.AttackTargets(invoker, currentSkill);
                        }
                    }
                    else
                    {
                        check = myManager.AttackTargets(invoker, currentSkill);

                    }

                    if (check == true)
                    {

                        myManager.oppEvent.caller = null;
                        myManager.CreateEvent(this, null, "Show Command", BeforeOpp);
                    }

                    if (currentSkill != null)
                        currentSkill = null;

                }
            }
        }
        else
        {
            if (invoker.SSTATUS == SecondaryStatus.confusion)
            {
                int chance = Random.Range(0, 2);

                if (chance <= 0)
                {
                    Debug.Log("They hit themselves");
                    newTarget.Clear();
                    newTarget.Add(invoker.currentTile);
                    myManager.SetTargetList(newTarget);
                    check = myManager.AttackTargets(invoker, currentSkill);
                }
            }
            else
            {
                check = myManager.AttackTargets(invoker, invoker.WEAPON);

            }
        }

        myManager.currentState = State.PlayerTransition;

        if (check == true)
        {

            myManager.oppEvent.caller = null;
            myManager.CreateEvent(this, null, "Show Command", BeforeOpp);

        }
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
                modification = current.STATS.FTCHANGE;
            if (current.SSTATUS != SecondaryStatus.seal)
            {
                if (currentSkill.CanUse(modification))
                {
                    if (current.SSTATUS == SecondaryStatus.confusion)
                    {
                        int chance = Random.Range(0, 2);
                        if (chance <= 0)
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
                    }
                    else
                    {
                        myManager.invManager.currentIndex = 0;

                    }
                    if (currentSkill != null)
                        currentSkill = null;
                }
            }
        }
        else
        {
            if (current.SSTATUS == SecondaryStatus.confusion)
            {
                int chance = Random.Range(0, 2);

                if (chance <= 0)
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



        if (check == true)
        {
            if (takeAction)
            {

                current.TakeAction();
            }


            // myManager.menuManager.ShowNone();
        }
    }

    public bool ShowCmd(Object data)
    {
     
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
