using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvent : MonoBehaviour
{
    public AudioSource Sound;
    public AudioClip SoundEvento;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundButton()
    {
        Sound.clip = SoundEvento;
        Sound.enabled = false;
        Sound.enabled = true;
    }
}
