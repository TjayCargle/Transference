using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalDisplay : MonoBehaviour
{

    public State[] displayStates;
    public bool requiresSelected = false;
    public bool isAffectDisplay = false;
    public bool requiresDoorTile = false;
    public bool requiresShopTile = false;
    public bool requiresSaveTile = false;
    public bool requriesSelectedPlayer = false;
    public bool requriesSelectedEnemy = false;
    public bool mustBeLiving = false;
    public bool checksForEvent = false;
    public bool requiresSelectedorSpecialTile = false;
    ManagerScript manager;
    public bool debugging = false;
    public bool checksForGlyph = false;
    public bool checksForEnemy = false;
    public bool checksForInteractable = false;
    public bool checkForHelpTile = false;

    public bool playerHasSkills = false;
    public bool playerHasSpells = false;
    public bool playerHasStrikes = false;
    public bool playerHasBarriers = false;
    public bool playerHasItems = false;
    public bool playerHasCombos = false;
    public bool playerHasAutos = false;
    public bool playerHasOpps = false;
    LivingObject livvy = null;
    public bool dontShowForMovementTutorial = false;
    public bool dontShowForStrikeTutorial = false;
    public bool dontShowForSkillTutorial = false;
    public bool dontShowForSpellTutorial = false;
    public bool dontShowForAllocateTutorial = false;
    public bool playerOrTile = false;

    public bool tweenAble = false;
    // Use this for initialization
    void Start()
    {
        manager = Common.GetManager();
    }

    //public void UpdateDisplay()
    //{
    //    gameObject.SetActive(false);
    //    if (manager)
    //    {
    //        for (int i = 0; i < displayStates.Length; i++)
    //        {
    //            if (manager. GetState() == displayStates[i])
    //            {
    //                gameObject.SetActive(true);

    //                break;
    //            }
    //        }

    //    }

    //}

    public void UpdateDisplay(ManagerScript newmanager)
    {
        manager = newmanager;
        gameObject.SetActive(false);
        bool found = false;
        if (null != newmanager)
        {
            if (null != newmanager.myCamera)
            {
                if (null != newmanager.myCamera.infoObject)
                {

                    livvy = manager.myCamera.infoObject.GetComponent<LivingObject>();
                }
            }
        }
        if (requiresSelected == true)
        {
            if (manager.myCamera.infoObject == null)
            {
                if (debugging)
                {
                    Debug.Log("no info obj");
                }
                gameObject.SetActive(false);
                return;
            }
            if (isAffectDisplay)
            {
                if (manager)
                {
                    if (livvy)
                    {
                        if (livvy.INVENTORY.DEBUFFS.Count > 0 || livvy.INVENTORY.BUFFS.Count > 0 || livvy.INVENTORY.EFFECTS.Count > 0 || livvy.GetComponent<SecondStatusScript>())
                        {
                            gameObject.SetActive(true);
                        }

                    }
                }
            }

        }

        if (playerOrTile == true)
        {
            if (manager.myCamera.infoObject == null)
            {
                if (manager.myCamera.selectedTile)
                {
                    if (manager.myCamera.selectedTile.TTYPE == TileType.regular)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            else if (manager.myCamera.infoObject.FACTION != Faction.ally)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        if (requriesSelectedPlayer == true)
        {

            if (manager.player.current == null)
            {
                if (manager.myCamera.infoObject == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                else if (manager.myCamera.infoObject.FACTION != Faction.ally)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            //if (manager.myCamera.infoObject == null)
            //{

            //    gameObject.SetActive(false);
            //    return;
            //}
            //else if (!manager.myCamera.infoObject.GetComponent<LivingObject>())
            //{

            //    gameObject.SetActive(false);
            //    return;
            //}
        }
        if (requriesSelectedEnemy)
        {
            if (manager.myCamera.infoObject == null)
            {

                gameObject.SetActive(false);
                return;
            }
            else if (manager.myCamera.infoObject.FACTION == Faction.ally)
            {
                gameObject.SetActive(false);
                return;
            }
            else if (mustBeLiving == true && !manager.myCamera.infoObject.GetComponent<LivingObject>())
            {

                gameObject.SetActive(false);
                return;
            }
        }

        if (manager)
        {
            if (manager.menuManager.isGameOverShowing() == true)
            {
                gameObject.SetActive(false);
                return;
            }


            if (playerHasSkills == true)
            {
                if (livvy)
                {

                    if (livvy.PHYSICAL_SLOTS.SKILLS.Count == 0)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            if (playerHasSpells == true)
            {
                if (livvy)
                {
                    if (livvy.MAGICAL_SLOTS.SKILLS.Count == 0)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            if (playerHasStrikes == true)
            {
                if (livvy)
                {
                    if (livvy.INVENTORY.WEAPONS.Count == 0)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            if (playerHasBarriers == true)
            {
                if (livvy)
                {
                    if (livvy.INVENTORY.ARMOR.Count == 0)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            if (playerHasItems == true)
            {
                if (livvy)
                {
                    if (livvy.INVENTORY.ITEMS.Count == 0)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }

            if (playerHasOpps == true)
            {
                if (livvy)
                {
                    if (livvy.INVENTORY.OPPS.Count == 0)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }

            if (playerHasCombos == true)
            {
                if (livvy)
                {
                    if (livvy.INVENTORY.COMBOS.Count == 0)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }

            if (playerHasAutos == true)
            {
                if (livvy)
                {
                    if (livvy.INVENTORY.AUTOS.Count == 0)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }

            if (manager.currentTutorial.steps.Count > 0)
            {
                if (manager.currentTutorial.currentStep < manager.currentTutorial.steps.Count)
                {

                    switch (manager.currentTutorial.steps[manager.currentTutorial.currentStep])
                    {
                        case tutorialStep.moveToPosition:
                            {
                                if (dontShowForMovementTutorial == true)
                                {
                                    gameObject.SetActive(false);
                                    return;
                                }
                            }
                            break;
                        case tutorialStep.useStrike:
                            {
                                if (dontShowForStrikeTutorial == true)
                                {
                                    gameObject.SetActive(false);
                                    return;
                                }
                            }
                            break;
                        case tutorialStep.useSkill:
                            {
                                if (dontShowForSkillTutorial == true)
                                {
                                    gameObject.SetActive(false);
                                    return;
                                }
                            }
                            break;
                        case tutorialStep.useSpell:
                            {
                                if (dontShowForSpellTutorial == true)
                                {
                                    gameObject.SetActive(false);
                                    return;
                                }
                            }
                            break;
                        case tutorialStep.useBarrier:
                            break;
                        case tutorialStep.useItem:
                            break;
                        case tutorialStep.allocate:
                            {
                                if (dontShowForAllocateTutorial == true)
                                {
                                    gameObject.SetActive(false);
                                    return;
                                }
                            }
                            break;
                        case tutorialStep.defeatEnemy:
                            break;
                        case tutorialStep.hackGlyph:
                            break;
                    }
                }
            }




            if (requiresDoorTile == true)
            {
                if (manager.myCamera.currentTile)
                {

                    if (manager.myCamera.currentTile.TTYPE != TileType.door)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            if (requiresSelectedorSpecialTile == true)
            {
                if (manager.myCamera.currentTile)
                {

                    if (manager.myCamera.infoObject == null && manager.myCamera.currentTile.TTYPE == TileType.regular)
                    {
                        if (debugging == true)
                        {
                            Debug.Log("found false");
                        }

                        gameObject.SetActive(false);
                        return;
                    }
                    else
                    {
                        if (debugging == true)
                        {
                            Debug.Log("found true: " + manager.myCamera.currentTile.TTYPE);
                        }
                    }
                }

            }
            if (checksForGlyph == true)
            {
                if (manager.myCamera.infoObject == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (manager.myCamera.infoObject != manager.player.current)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (!manager.CheckAdjecentTilesGlyphs(manager.player.current))
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            if (checkForHelpTile == true)
            {
                if (!manager.myCamera)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (!manager.myCamera.currentTile)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (manager.myCamera.currentTile.TTYPE != TileType.help)
                {
                    if (Common.isOverrideTile(manager.myCamera.currentTile) == false)
                    {

                        gameObject.SetActive(false);
                        return;

                    }
                }
            }
            if (checksForInteractable == true)
            {
                if (manager.myCamera.infoObject == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (manager.myCamera.infoObject != manager.player.current)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (!manager.CheckAdjecentTilesInteractable(manager.player.current))
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            if (checksForEnemy == true)
            {
                gameObject.SetActive(false);
                return;
                if (manager.myCamera.infoObject == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (manager.myCamera.infoObject != manager.player.current)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (!manager.CheckAdjecentTilesEnemy(manager.player.current))
                {

                }
            }
            if (checksForEvent == true)
            {
                if (manager.myCamera.infoObject == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (manager.myCamera.infoObject != manager.player.current)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (!manager.CheckAdjecentTilesEvent(manager.player.current))
                {
                    gameObject.SetActive(false);
                    return;
                }


            }

            if (requiresShopTile == true)
            {
                if (manager.myCamera)
                {
                    if (manager.myCamera.currentTile)
                    {
                        if (manager.myCamera.currentTile.TTYPE != TileType.shop)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
            }
            if (requiresSaveTile == true)
            {
                if (manager.myCamera)
                {
                    if (manager.myCamera.currentTile)
                    {
                        if (manager.myCamera.currentTile.TTYPE != TileType.saveLoad)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
            }
            gameObject.SetActive(true);

            for (int i = 0; i < displayStates.Length; i++)
            {
                if (manager.GetState() == displayStates[i])
                {

                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                gameObject.SetActive(false);
                if (debugging == true)
                {
                    Debug.Log("found false");
                }
            }
        }
        else
        {
            Debug.Log("no manager");
        }

    }
}
