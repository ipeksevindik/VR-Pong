using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongGameManager : MonoBehaviour
{
    RightPlayer rightPlayer;
    LeftPlayer leftPlayer;

    private void OnEnable()
    {
        rightPlayer.OnCollidedBall += RightScoreIncrease;
    }

    private void OnDisable()
    {

    }

    public void RightScoreIncrease()
    {
        rightPlayer.counter++;
    }
    public void RightScoreIncrease()
    {
        rightPlayer.counter++;
    }

}
