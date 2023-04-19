using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public static SoundEffect Instance;
    public AudioSource Source,Source_walk,Source_release;
    public AudioClip ButtonSounds;
    public AudioClip GetOxygenSounds,WinSounds,LoseSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("There is probably more than one instance.", Instance);
            Object.Destroy(this);
        }
    }

    public void ButtonClicks()
    {
        Source.PlayOneShot(ButtonSounds);
    }
    public void Walk(bool state,bool iscrouch)
    {
        if(state == true && iscrouch==false)
            Source_walk.enabled = true;
        else Source_walk.enabled = false;
    }
    public void GetOxygenTank()
    {
        Source.PlayOneShot(GetOxygenSounds);
    }
    public void ReleaseOxygen(float oxygen,bool state)
    {
        if(oxygen<50f && state==true)
            Source_release.enabled = true;
        else
            Source_release.enabled = false;
    }
    public void Win()
    {
        Source.PlayOneShot(WinSounds);
    }
}
