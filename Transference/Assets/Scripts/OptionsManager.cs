using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{

    public AudioSource master;
    public AudioSource music;
    public AudioSource sfx;

    public bool battleAnims = true;
    public bool dmgAnims = true;
    public bool displayMessages = true;
    public bool showExp = false;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public Toggle battleAnimToggle;
    public Toggle dmgAnimToggle;
    public Toggle displayToggle;

    private void Start()
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

        if(battleAnimToggle)
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
    private void Update()
    {
        if (masterSlider)
        {
            if (masterSlider.value != master.volume)
            {
                master.volume = masterSlider.value;
            }
        }
        if (musicSlider)
        {
            if (musicSlider.value != music.volume)
            {
                music.volume = musicSlider.value;
            }
        }

        if (sfxSlider)
        {

            if (sfxSlider.value != sfx.volume)
            {
                sfx.volume = sfxSlider.value;
            }
        }
        if (musicSlider.value > masterSlider.value)
        {
            musicSlider.value = masterSlider.value;
        }
        if (sfxSlider.value > masterSlider.value)
        {
            sfxSlider.value = masterSlider.value;
        }


        if (music.volume > master.volume)
        {
            music.volume = master.volume;
        }

        if (sfx.volume > master.volume)
        {
            sfx.volume = master.volume;
        }


    }
}
