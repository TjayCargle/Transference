using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public TileScript currentTile;
    public Canvas infoCanvas;
    public Text infoText;
    public Slider healthSlider;
    public Slider mansSlider;
    public Slider fatigueSlider;
    public Text healthText;
    public Text manaText;
    public Text fatigueText;
    public GridObject infoObject;
    float distance = 0;

    // Use this for initialization
    void Start()
    {
        transform.Rotate(new Vector3(35, 5, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTile)
        {
            if (infoCanvas)
            {
                if (infoText)
                {
                    if (currentTile.isOccupied)
                    {
                        infoCanvas.gameObject.SetActive(true);
                        if (infoObject)
                        {
                            infoText.text = infoObject.FullName;
                            if (infoObject.GetComponent<LivingObject>())
                            {

                                infoText.text = infoText.text + " \n LV:" + infoObject.GetComponent<StatScript>().LEVEL.ToString();

                                if (healthSlider)
                                {
                                    healthSlider.value = (float)infoObject.GetComponent<LivingObject>().HEALTH / (float)infoObject.GetComponent<LivingObject>().MAX_HEALTH;
                                    healthText.text = infoObject.GetComponent<LivingObject>().HEALTH.ToString() + "/" + infoObject.GetComponent<LivingObject>().MAX_HEALTH.ToString();
                                }
                                if (mansSlider)
                                {
                                    mansSlider.value = (float)infoObject.GetComponent<LivingObject>().MANA / (float)infoObject.GetComponent<LivingObject>().MAX_MANA;
                                   manaText.text = infoObject.GetComponent<LivingObject>().MANA.ToString() + "/" + infoObject.GetComponent<LivingObject>().MAX_MANA.ToString();
                                }
                                if (fatigueSlider)
                                {
                                    fatigueSlider.value = (float)infoObject.GetComponent<LivingObject>().FATIGUE / (float)infoObject.GetComponent<LivingObject>().MAX_FATIGUE;
                                    fatigueText.text = infoObject.GetComponent<LivingObject>().FATIGUE.ToString() + "/" + infoObject.GetComponent<LivingObject>().MAX_FATIGUE.ToString();
                                }

                            }
                        }
                        else if (infoObject.GetComponent<StatScript>())
                        {
                            if (healthSlider)
                            {
                                healthSlider.value = infoObject.GetComponent<StatScript>().HEALTH / infoObject.GetComponent<StatScript>().MAX_HEALTH;
                                healthText.text = infoObject.GetComponent<StatScript>().HEALTH.ToString() + "/" + infoObject.GetComponent<StatScript>().MAX_HEALTH.ToString();
                            }

                        }
                    }
                }
            }
            else
            {
                infoObject = null;
                infoText.text = "";
                infoCanvas.gameObject.SetActive(false);

            }
        }

        Vector3 tilePos = currentTile.transform.position;
        Vector3 camPos = transform.position;
        tilePos.y = 0.0f;
        camPos.y = 0.0f;
        distance = Mathf.Abs(tilePos.sqrMagnitude - camPos.sqrMagnitude);
        distance = Mathf.Sqrt(distance);
        if (distance > 1.0f)
        {
            Vector3 directionVector = (currentTile.transform.position - new Vector3(0, -5, 7)) - transform.position;
            transform.Translate(directionVector * Time.deltaTime);
        }

    }
}

