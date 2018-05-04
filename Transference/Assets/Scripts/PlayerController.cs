using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : LivingObject
{
    [SerializeField]
    private SkillScript currentSkill;
    private enum Facing
    {
        North,
        East,
        South,
        West,
    }
    //public bool canMove = false;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        // MoveDist = 1;
        // MAX_ATK_DIST = 2;
        // SkillManager.CreateSkill(1);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        switch (myManager.currentState)
        {
            case State.PlayerInput:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    myManager.currentMenuitem--;
                    if (myManager.currentMenuitem < 0)
                    {
                        myManager.currentMenuitem = 5;
                    }
                    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    myManager.currentMenuitem++;
                    if (myManager.currentMenuitem > 5)
                    {
                        myManager.currentMenuitem = 0;
                    }
                    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    myManager.SelectMenuItem(this);
                }
                break;
            case State.PlayerMove:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    myManager.MoveGridObject(this, new Vector3(0, 0, 1));
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    myManager.MoveGridObject(this, new Vector3(-1, 0, 0));
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    myManager.MoveGridObject(this, new Vector3(0, 0, -1));
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    myManager.MoveGridObject(this, new Vector3(1, 0, 0));
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    myManager.ComfirmMenuAction(this);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                break;
            case State.PlayerAttacking:

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (myManager.currentAttackList.Count > 0)
                    {
                     
                        List<int> targetIndicies = myManager.GetTargetList();
                        
                        if (targetIndicies != null)
                        {
                            for (int i = 0; i < targetIndicies.Count; i++)
                            {
                                GridObject potentialTarget = myManager.GetObjectAtTile(myManager.currentAttackList[targetIndicies[i]]);
                                if (potentialTarget.GetComponent<LivingObject>())
                                {
                                    LivingObject target = potentialTarget.GetComponent<LivingObject>();

                                    DmgReaction react;
                                    if(currentSkill != null)
                                    {
                                        react = myManager.CalcDamage(this, target,currentSkill.ELEMENT, currentSkill.ETYPE, currentSkill.DMG);
                                    }
                                    else
                                    {
                                        react = myManager.CalcDamage(this, target, WEAPON.AFINITY, WEAPON.ATTACK_TYPE, WEAPON.ATTACK);
                                    }

                                    switch (react.reaction)
                                    {
                                        case Reaction.none:
                                            myManager.DamageLivingObject(target, react.damage);
                            
                                            break;
                                        case Reaction.statDrop:
                                            break;
                                        case Reaction.nulled:
                                            myManager.DamageLivingObject(target, react.damage);

                                            break;
                                        case Reaction.reflected:
                                            myManager.DamageLivingObject(this, react.damage);
                                            break;
                                        case Reaction.knockback:
                                            myManager.DamageLivingObject(target, react.damage);
                                            Vector3 direction = transform.position - target.transform.position;
                                            myManager.MoveGridObject(target, (-1 * direction));
                                            myManager.ComfirmMoveGridObject(target, myManager.GetTileIndex(target));
                                            
                                            break;
                                        case Reaction.snatched:
                                            myManager.DamageLivingObject(target, react.damage);
                                            break;
                                        default:
                                            break;
                                    }

                                    if (currentSkill != null)
                                        currentSkill = null;
                                }
                            }


                        }
                        if (myManager.GetComponent<MenuManager>())
                        {
                            myManager.GetComponent<MenuManager>().ShowCommandCanvas();
                        }
                        myManager.prevState = myManager.currentState;
                        myManager.currentState = State.PlayerInput;
                    }

                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                    currentSkill = null;
                }
                break;
            case State.PlayerEquippingMenu:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    myManager.currentMenuitem--;
                    if (myManager.currentMenuitem < 6)
                    {
                        myManager.currentMenuitem = 8;
                    }
                    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    myManager.currentMenuitem++;
                    if (myManager.currentMenuitem > 8)
                    {
                        myManager.currentMenuitem = 6;
                    }
                    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    myManager.SelectMenuItem(this);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                break;
            case State.PlayerEquipping:
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        InventoryMangager invm = Object.FindObjectOfType<InventoryMangager>();

                        switch (invm.selectedMenuItem.refItem.TYPE)
                        {
                            case 0:
                                if (invm.selectedMenuItem.refItem.GetType() == typeof(WeaponScript))
                                {
                                    WeaponScript newWeapon = (WeaponScript)invm.selectedMenuItem.refItem;
                                    WEAPON.Equip(newWeapon);
                                }
                                break;
                            case 1:
                                if (invm.selectedMenuItem.refItem.GetType() == typeof(ArmorScript))
                                {
                                    ArmorScript newArmor = (ArmorScript)invm.selectedMenuItem.refItem;
                                    ARMOR.Equip(newArmor);
                                }
                                break;
                            case 2:
                                if (invm.selectedMenuItem.refItem.GetType() == typeof(AccessoryScript))
                                {
                                    AccessoryScript newAcc = (AccessoryScript)invm.selectedMenuItem.refItem;
                                    ACCESSORY.Equip(newAcc);
                                }
                                break;
                            case 3:
                                if (invm.selectedMenuItem.refItem.GetType() == ACCESSORY.GetType())
                                {
                                    AccessoryScript newAcc = (AccessoryScript)invm.selectedMenuItem.refItem;
                                    ACCESSORY.Equip(newAcc);

                                }
                                break;
                            case 4:
                                {

                                    SkillScript selectedSkil = (SkillScript)invm.selectedMenuItem.refItem;
                                    currentSkill = selectedSkil;
                                    myManager.attackableTiles = myManager.GetSkillsAttackableTiles(this, selectedSkil);
                                    myManager.ShowWhite();
                                    if (myManager.attackableTiles.Count > 0)
                                    {
                                        for (int i = 0; i < myManager.attackableTiles.Count; i++) //list of lists
                                        {
                                            for (int j = 0; j < myManager.attackableTiles[i].Count; j++) //indivisual list
                                            {

                                                myManager.attackableTiles[i][j].myColor = Color.red;
                                            }
                                        }
                                        myManager.currentAttackList = myManager.attackableTiles[0];

                                        for (int i = 0; i < myManager.currentAttackList.Count; i++)
                                        {
                                            myManager.currentAttackList[i].myColor = Color.green;
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
                                    myManager.prevState = myManager.currentState;
                                    myManager.currentState = State.PlayerAttacking;
                                }
                                break;
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        myManager.CancelMenuAction(this);
                        currentSkill = null;
                    }
                }
                break;
            case State.PlayerWait:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                break;
            case State.FreeCamera:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    myManager.ComfirmMenuAction(this);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                break;
            case State.EnemyTurn:
                break;
            default:
                break;
        }


    }
}
