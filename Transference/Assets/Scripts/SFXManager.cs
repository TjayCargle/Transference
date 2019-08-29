using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

    public bool isSetup = false;
    protected AudioSource source;

    public AudioSource SOURCE
    {
        get { return source; }
        set { source = value; }
    }
    public void Setup()
    {
        if(!isSetup)
        {
            source = GetComponent<AudioSource>();
        }
        isSetup = true;
    }
	void Start () {
        Setup();
	}
	
    public void loadAudio(AudioClip clip)
    {
        source.clip = clip;
    }

    public bool isPlaying()
    {
        return source.isPlaying;
    }
    public void playSound()
    {
       // if(!source.isPlaying)
        {
          //  source.Stop();
            source.Play();
        }
    }

}
