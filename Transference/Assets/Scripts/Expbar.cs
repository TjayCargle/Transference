﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Expbar : MonoBehaviour
{

    public LivingObject currentUser;
    public bool updating = false;
    public Slider slider;
    public Text text;
   public ManagerScript manager;
    private float timer = 0.0f;
    private float startValue;
    private float finalValue;
    private void Start()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<Text>();
        gameObject.SetActive(false);

    }
    void Update()
    {
        if (updating)
        {
            if (currentUser)
            {
                if (timer >= 0)
                {
                    if (slider.value < currentUser.BASE_STATS.EXP)
                    {
                        slider.value += Mathf.Lerp(startValue, finalValue, Time.deltaTime);//Mathf.Max(0.18f, Mathf.Abs(slider.value - currentUser.BASE_STATS.EXP) * 0.03f);
                        text.text = ((int)slider.value).ToString() + "/100";
                    }
                    if (slider.value >= 100)
                    {

                        if (currentUser.BASE_STATS.EXP >= 100)
                        {
                            currentUser.LevelUp();
                            slider.value = 0;
                            if(manager)
                            {
                                manager.CreateTextEvent(this, currentUser.NAME + " leveled up!", "level up event", manager.CheckText, manager.TextStart);
                            }
                            StartUpdating(false);
                        }
                    }
                    if (slider.value >= currentUser.BASE_STATS.EXP)
                    {
                        slider.value = currentUser.BASE_STATS.EXP;
                        text.text = ((int)slider.value).ToString() + "/100";
                  
                      
                    }
                    timer -= 0.01f;
                }
                else
                {
                    gameObject.SetActive(false);
                    updating = false;
                }
            }
        }
    }
    public void StartUpdating(bool resetTime = true)
    {
        if(resetTime)
        {
        timer = 0.5f;

        }
        startValue = slider.value;
        if (currentUser)
            finalValue = slider.value + (float)currentUser.BASE_STATS.EXP;
        else
            finalValue = startValue + 2;
        if (finalValue > 100)
            finalValue = 100;
        updating = true;
    }
}
