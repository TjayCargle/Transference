using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Toggle dispalyExp;



    public bool hoverSelect = false;
    public bool fixedCamera = true;





    public bool isSetup = false;
    public void Setup()
    {
        if (!isSetup)
        {
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

            if (dispalyExp)
            {
                dispalyExp.isOn = showExp;
                dispalyExp.onValueChanged.AddListener(delegate { ChangeDisplayExp(dispalyExp); });
            }
            isSetup = true;
        }
    }
    private void Start()
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
    void ChangeDisplayExp(Toggle change)
    {
        showExp = !showExp;
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
}
