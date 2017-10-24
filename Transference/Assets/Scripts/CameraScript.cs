using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public TileScript currentTile;
    public Canvas infoCanvas;
    public Text infoText;
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
            if(infoCanvas)
            {
                if(infoText)
                {
                    if(currentTile.isOccupied)
                    {
                        infoCanvas.gameObject.SetActive(true);
                        if (infoObject)
                        {
                            infoText.text = infoObject.FullName;
                            if(infoObject.GetComponent<StatScript>())
                            {

                                infoText.text = infoText.text + " \n LV:" + infoObject.GetComponent<StatScript>().LEVEL.ToString();
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
            }
            Vector3 tilePos = currentTile.transform.position;
            Vector3 camPos = transform.position;
            tilePos.y = 0.0f;
            camPos.y = 0.0f;
            distance = Mathf.Abs(tilePos.sqrMagnitude - camPos.sqrMagnitude);
            distance = Mathf.Sqrt(distance);
            if (distance > 2.0f)
            {
                Vector3 directionVector = (currentTile.transform.position - new Vector3(0, -5, 7)) - transform.position;    
                transform.Translate(directionVector * Time.deltaTime);
            }
          
        }
    }
}
