using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class OptionsManager : MonoBehaviour
{

    public AudioSource master;
    public AudioSource music;
    public AudioSource sfx;
    public AudioSource voices;

    public bool battleAnims = true;
    public bool dmgAnims = true;
    public bool displayMessages = true;
    public bool showExp = true;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;

    public Toggle battleAnimToggle;
    public Toggle dmgAnimToggle;
    public Toggle displayToggle;
    public Toggle hoverToggle;



    public bool hoverSelect = false;
    public bool fixedCamera = true;


    public float mastervolume;
    public float musicvolume;
    public float sfxvolume;
    public float voicevolume;

    public float displayMsgSpeed = 0.2f;
    public float cameraSpeed = 0.8f;

    public Slider displaySlider;
    public Slider cameraSlider;

    public bool isSetup = false;
    private CameraScript myCamera;
    public void Setup()
    {
        if (!isSetup)
        {
            myCamera = GameObject.FindObjectOfType<CameraScript>();
            if (masterSlider)
            {

                masterSlider.value = master.volume;

            }
            if (musicSlider)
            {

                musicSlider.value = music.volume;

            }

            if (sfxSlider)
            {
                sfxSlider.value = sfx.volume;
            }


            if (voiceSlider)
            {
                voiceSlider.value = voices.volume;
            }

            if(displaySlider)
            {
                displaySlider.value = displayMsgSpeed;
            }

            if(cameraSlider)
            {
                cameraSlider.value = cameraSpeed;
            }

            if (battleAnimToggle)
            {
                battleAnimToggle.isOn = battleAnims;
                battleAnimToggle.onValueChanged.AddListener(delegate { ChangeBattleAnim(battleAnimToggle); });
            }

            if (dmgAnimToggle)
            {
                dmgAnimToggle.isOn = dmgAnims;
                dmgAnimToggle.onValueChanged.AddListener(delegate { ChangeDmgAnim(dmgAnimToggle); });
            }

            if (displayToggle)
            {
                displayToggle.isOn = displayMessages;
                displayToggle.onValueChanged.AddListener(delegate { ChangeDisplayMessages(displayToggle); });
            }

            if (hoverToggle)
            {
                hoverToggle.isOn = hoverSelect;
                hoverToggle.onValueChanged.AddListener(delegate { ChangeHoverSelect(hoverToggle); });
            }
            loadSettings();
            isSetup = true;
        }
    }
    private void Start()
    {
        Setup();
    }
    private void Awake()
    {
        Setup();
    }
    void ChangeBattleAnim(Toggle change)
    {
        battleAnims = !battleAnims;
    }

    void ChangeDmgAnim(Toggle change)
    {
        dmgAnims = !dmgAnims;
    }

    void ChangeDisplayMessages(Toggle change)
    {
        displayMessages = !displayMessages;
    }
    void ChangeHoverSelect(Toggle change)
    {
        hoverSelect = !hoverSelect;
    }
    private void Update()
    {
        ForceUpdate();
    }
    public void ForceUpdate()
    {
        if (!isSetup)
        {
            Setup();
        }
        if (masterSlider)
        {
            if (masterSlider.value != master.volume)
            {
                master.volume = masterSlider.value;
            }
            if (musicSlider)
            {
                if (musicSlider.value != music.volume)
                {
                    music.volume = musicSlider.value;
                }

                if (musicSlider.value > masterSlider.value)
                {
                    musicSlider.value = masterSlider.value;
                }

                if (music.volume > master.volume)
                {
                    music.volume = master.volume;
                }


            }

            if (sfxSlider)
            {

                if (sfxSlider.value != sfx.volume)
                {
                    sfx.volume = sfxSlider.value;
                }

                if (sfxSlider.value > masterSlider.value)
                {
                    sfxSlider.value = masterSlider.value;
                }

                if (sfx.volume > master.volume)
                {
                    sfx.volume = master.volume;
                }

            }

            if (voiceSlider)
            {

                if (voiceSlider.value != voices.volume)
                {
                    voices.volume = voiceSlider.value;
                }

                if (voiceSlider.value > masterSlider.value)
                {
                    voiceSlider.value = masterSlider.value;
                }
                if (voices.volume > master.volume)
                {
                    voices.volume = master.volume;
                }
            }






        }
    }

    public void loadSettings()
    {
        if (PlayerPrefs.HasKey("gameSettings") == true)
        {
            string gameSettings = PlayerPrefs.GetString("gameSettings");
            string[] optionLines = gameSettings.Split('|');
            for (int i = 0; i < optionLines.Length; i++)
            {
                string line = optionLines[i];
                if (line != String.Empty)
                {
                    if (line[0] != '-')
                    {
                        string[] parsed = line.Split(',');
                        switch (parsed[0])
                        {
                            case "ba":
                                {
                                    if (Boolean.TryParse(parsed[1], out battleAnims))
                                    {
                                        battleAnimToggle.isOn = battleAnims;
                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;

                            case "da":
                                {
                                    if (Boolean.TryParse(parsed[1], out dmgAnims))
                                    {
                                        dmgAnimToggle.isOn = dmgAnims;
                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;

                            case "dm":
                                {
                                    if (Boolean.TryParse(parsed[1], out displayMessages))
                                    {
                                        displayToggle.isOn = displayMessages;
                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;

                            case "hs":
                                {
                                    if (Boolean.TryParse(parsed[1], out hoverSelect))
                                    {
                                        hoverToggle.isOn = hoverSelect;
                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;
                            case "mv":
                                {
                                    if (float.TryParse(parsed[1], out mastervolume))
                                    {
                                        masterSlider.value = mastervolume;
                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;

                            case "av":
                                {
                                    if (float.TryParse(parsed[1], out musicvolume))
                                    {
                                        musicSlider.value = musicvolume;
                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;
                            case "sv":
                                {
                                    if (float.TryParse(parsed[1], out sfxvolume))
                                    {
                                        sfxSlider.value = sfxvolume;
                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;

                            case "vv":
                                {
                                    if (float.TryParse(parsed[1], out voicevolume))
                                    {
                                        voiceSlider.value = voicevolume;
                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;

                            case "dms":
                                {
                                    if (float.TryParse(parsed[1], out displayMsgSpeed))
                                    {

                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;

                            case "cs":
                                {
                                    if (float.TryParse(parsed[1], out cameraSpeed))
                                    {
                                        if(myCamera)
                                        {
                                            myCamera.smoothSpd = cameraSpeed;
                                        }
                                            cameraSlider.value = cameraSpeed;
                                    }
                                    else
                                    {
                                        Debug.Log("failed to parse settings");
                                    }
                                }
                                break;

                        }
                    }
                }
            }
        }
    }

    public void SaveSettings()
    {
        string newGameSettings = "";

        newGameSettings += "ba," + battleAnims + "|";
        newGameSettings += "da," + dmgAnims + "|";
        newGameSettings += "dm," + displayMessages + "|";
        newGameSettings += "hs," + hoverSelect + "|";
        newGameSettings += "mv," + masterSlider.value+ "|";
        newGameSettings += "av," + musicSlider.value+ "|";
        newGameSettings += "sv," + sfxSlider.value+ "|";
        newGameSettings += "vv," + voiceSlider.value + "|";
        newGameSettings += "dms," + displaySlider.value + "|";
        newGameSettings += "cs," + cameraSlider.value+ "|";
        PlayerPrefs.SetString("gameSettings", newGameSettings);


        loadSettings();
    }

    public void Cancel()
    {
        loadSettings();
        ForceUpdate();
    }

  

}
