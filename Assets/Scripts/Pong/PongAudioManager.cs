using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongAudioManager : MonoBehaviour
{

    public AudioSource BallHit;

    void Start()
    {

    }

    public void PlayBallHit()
    {
        BallHit.Play();
    }

}
