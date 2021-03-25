using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CutsceneManager : MonoBehaviour
{
    public List<Sprite> backgrounds;
    public Image mainBackground;

    public void ChangeBackground(int newBack)
    {
        if(backgrounds.Count > newBack && mainBackground != null)
        {
            mainBackground.sprite = backgrounds[newBack];
            mainBackground.transform.localScale = new Vector3(1.5f, 1.5f, 1);

            if(newBack == 0)
            {
                mainBackground.color = new Color(0.8f, 0.74f, 0.74f);
            }
            if (newBack == 1)
            {
                mainBackground.color = new Color(0.49f, 0.41f, 0.41f);

            }

        }    
    }    

    public (State, string) CheckForMapChangeEvent(MapDetail checkMap, ManagerScript someManager, int defaultSceneEntry, TalkPanel talkPanel, SceneContainer currentScene,  string currentObjectiveString)
    {


   
        switch (defaultSceneEntry)
        {
            case 27:
                {
                    if (checkMap.mapIndex == 27)
                    {
                    ChangeBackground(1);
                        someManager.myCamera.PlaySoundTrack(18);
                        someManager.myCamera.previousClip = someManager.myCamera.musicClips[13];
                        if (talkPanel)
                        {
                            Common.summonedZeffron = true;
                            Common.enemiesCompletedPhase = true;
                            Common.haveBeenCrippled = true;
                            Common.hasAllocated = true;
                            Common.hasGainedSkill = true;
                            Common.hasLearnedFromEnemy = true;

                            talkPanel.gameObject.SetActive(true);
                            currentScene = Common.GetDatabase().GetSceneData("ZeffronPrologue");
                            currentObjectiveString = "Investigate the fairies";
                            someManager.currentState = State.SceneRunning;
                            talkPanel.scene = currentScene;
                            currentScene.index = 0;
                            someManager.SetScene(currentScene);

                        }

                    }

                    if (checkMap.mapIndex == 16)
                    {
                        ChangeBackground(0);
                        currentObjectiveString = "Look for the faries";
                    }

                }
                break;
            case 26:
                {
                    ChangeBackground(0);
                    if (checkMap.mapIndex == 26)
                    {
                        someManager.myCamera.PlaySoundTrack(10);
                        someManager.myCamera.previousClip = someManager.myCamera.musicClips[13];
                        if (talkPanel)
                        {
                            List<tutorialStep> tSteps = new List<tutorialStep>();
                            List<int> tClar = new List<int>();



                            tSteps.Add(tutorialStep.showTutorial);
                            tClar.Add(27);
                            tSteps.Add(tutorialStep.moveToPosition);
                            tClar.Add(20);
                            tSteps.Add(tutorialStep.showTutorial);
                            tClar.Add(12);
                            // tClar.Add(23);
                            tSteps.Add(tutorialStep.useStrike);
                            tClar.Add(-1);
                            tSteps.Add(tutorialStep.showTutorial);
                            tClar.Add(17);
                            tSteps.Add(tutorialStep.useSpell);
                            tClar.Add(-1);

                            someManager.PrepareTutorial(tSteps, tClar);
                            talkPanel.gameObject.SetActive(true);
                            currentScene = Common.GetDatabase().GetSceneData("JaxPrologue");
                            someManager.currentState = State.SceneRunning;
                            talkPanel.scene = currentScene;
                            currentScene.index = 0;
                            someManager.SetScene(currentScene);

                        }




                    }
                    if (checkMap.mapIndex == 15)
                    {
                        someManager.myCamera.PlaySoundTrack(3);
                        someManager.myCamera.previousClip = someManager.myCamera.musicClips[13];
                        if (talkPanel)
                        {

                            talkPanel.gameObject.SetActive(true);
                            currentScene = Common.GetDatabase().GetSceneData("Scene1");
                            currentObjectiveString = "Get rid of the Glyph";
                            someManager.currentState = State.SceneRunning;
                            talkPanel.scene = currentScene;
                            currentScene.index = 0;
                            someManager.SetScene(currentScene);
                        }
                    }
                    if (checkMap.mapIndex == 17)
                    {

                        if (Common.summonedZeffron == false)
                        {


                            someManager.myCamera.PlaySoundTrack(6);
                            someManager.myCamera.previousClip = someManager.myCamera.musicClips[16];

                            if (talkPanel)
                            {
                                //MoveCameraAndShow(liveZeff);
                                talkPanel.gameObject.SetActive(true);
                                currentScene = Common.GetDatabase().GetSceneData("JaxFindZeff");
                                currentObjectiveString = "Work with Zeffron";
                                someManager.currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                someManager.SetScene(currentScene);

                            }

                        }
                    }
                }
                break;
            case 17:
                {
                    ChangeBackground(0);
                  
                    if (checkMap.mapIndex == 17)
                    {

                        if (Common.summonedZeffron == false)
                        {
                            List<tutorialStep> tSteps = new List<tutorialStep>();
                            List<int> tClar = new List<int>();

                            tSteps.Add(tutorialStep.showTutorial);
                            tClar.Add(29);
                            someManager.PrepareTutorial(tSteps, tClar);

                            someManager.myCamera.PlaySoundTrack(6);
                            someManager.myCamera.previousClip = someManager.myCamera.musicClips[16];

                            if (talkPanel)
                            {
                                //MoveCameraAndShow(liveZeff);
                                talkPanel.gameObject.SetActive(true);
                                currentScene = Common.GetDatabase().GetSceneData("JaxTalkToZeffOne");
                                currentObjectiveString = "Work with Zeffron";
                                someManager.currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                someManager.SetScene(currentScene);
                                //currentScene.isRunning = true;
                                //someManager.menuManager.ShowNone();
                                //(this, null, "scene0 event", someManager.CheckSceneRunning, null, 0);
                            }
                        }
                    }
                    if (checkMap.mapIndex == 8)
                    {
                        someManager.myCamera.PlaySoundTrack(4);
                        someManager.myCamera.previousClip = someManager.myCamera.musicClips[4];
                        if (talkPanel)
                        {

                            talkPanel.gameObject.SetActive(true);
                            currentScene = Common.GetDatabase().GetSceneData("JaxEncounterGlyph");
                            currentObjectiveString = "Head to the Library";
                            someManager.currentState = State.SceneRunning;
                            talkPanel.scene = currentScene;
                            currentScene.index = 0;
                            someManager.SetScene(currentScene);
                        }
                    }
                    if (checkMap.mapIndex == 14)
                    {
                        someManager.myCamera.PlaySoundTrack(4);
                        someManager.myCamera.previousClip = someManager.myCamera.musicClips[10];
                        if (talkPanel)
                        {

                            talkPanel.gameObject.SetActive(true);
                            currentScene = Common.GetDatabase().GetSceneData("Scene4");
                            someManager.currentState = State.SceneRunning;
                            talkPanel.scene = currentScene;
                            currentScene.index = 0;
                            someManager.SetScene(currentScene);
                        }
                    }

                    if (Common.hasSeenJaxAndZeffCatchUp == false)
                    {
                        if (checkMap.mapIndex == 16 || checkMap.mapIndex == 0)
                        {
                            someManager.myCamera.PlaySoundTrack(2);
                            someManager.myCamera.previousClip = someManager.myCamera.musicClips[10];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = Common.GetDatabase().GetSceneData("JaxAndZeffronChapter1");
                                currentObjectiveString = "Head to the Library";
                                someManager.currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                someManager.SetScene(currentScene);
                            }
                            Common.hasSeenJaxAndZeffCatchUp = true;
                        }

                    }

                }
                break;
        }

        return ( someManager.currentState, currentObjectiveString);
    }


}
