using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] sfxSounds;
    public AudioSource sfxSource;
    public AudioSource bgm;

 
    public void PlaySFX(string name, bool loop)
    {
        sfxSource.loop = loop;
        Sound s = Array.Find(sfxSounds, x => x.Name == name);

        if(s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.clip = s.Clip;
            sfxSource.Play();
        }
    }


    public void PlayPullHandle()
    {
        PlaySFX("pull_handle", false);
    }

    public void PlayRowStoped()
    {
        PlaySFX("prize", false);
    }

    public void PlayRowMove()
    {
        PlaySFX("rows", true);
    }

    public void PlayJackpot()
    {
        PlaySFX("jackpot", false);
    }
}
