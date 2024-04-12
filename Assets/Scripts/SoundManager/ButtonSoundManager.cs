using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip soundMenuInicio;
    public AudioClip soundBrownBtn;
    public AudioClip soundIconBtn;
    public AudioClip soundExit;

    public void soundButton(int soundClipId)
    {
        switch (soundClipId)
        {
            case 1:
            sound.clip = soundMenuInicio;
            break;
            case 2:
            sound.clip = soundBrownBtn;
            break;
            case 3:
            sound.clip = soundIconBtn;
            break;
            case 4:
            sound.clip = soundExit;
            break;
        }
        
        sound.enabled = false;
        sound.enabled = true;
    }
}
