using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelCreationCamera : MonoBehaviour
{
    public LevelCreationTiles currentTile;
    public LevelCreationTiles selectedTile;

    public int x = 0;
    public int y = -5;
    public int z = 7;


    Color transparent;
    Color opaque;
    public float smoothSpd = 0.5f;

    bool isSetup = false;
    
    void Start()
    {
        Setup();
    }
    public void Setup()
    {
        if (!isSetup)
        {
            isSetup = true;

            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.Rotate(new Vector3(90, 0, 0));
            SetCameraPosDefault();
            transparent = new Color(0, 0, 0, 0);
            opaque = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        
        }
    }

    void FixedUpdate()
    {
       
            UpdatePosition();
        
    }

    public void UpdatePosition()
    {

        if (selectedTile)
        {
            Vector3 tilePos = selectedTile.transform.position;
            Vector3 camPos = transform.position;
            tilePos.y += y;
            tilePos.z += z;
            Vector3 targetLocation = tilePos - camPos;
            Vector3 smooth = Vector3.Lerp(transform.position, tilePos, smoothSpd * Time.fixedDeltaTime);
            transform.position = smooth;

        }
   
    }

    public void SetCameraPosDefault()
    {
        x = 0;
        y = 12;
        z = 0;
    }

    public void SetCameraPosSlightZoom()
    {
        x = 0;
        y = 10;
        z = 0;
    }

    public void SetCameraPosZoom()
    {
        x = 0;
        y = 8;
        z = 0;
    }

    public void SetCameraPosOffsetZoom()
    {
        x = -2;
        y = 6;
        z = 0;
    }

    public void SetCameraPosFar()
    {
        x = 0;
        y = 13;
        z = -1;
    }

  



}
