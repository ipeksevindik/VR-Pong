using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PongGameManager : MonoBehaviourPun
{
    [SerializeField]
    RightPlayer rightPlayer;

    [SerializeField]
    LeftPlayer leftPlayer;

    [SerializeField]
    Ball ball;

    public void RightScoreIncrease()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        rightPlayer.counter++;

        photonView.RPC(nameof(RightScoreUpdateRPC), RpcTarget.All, rightPlayer.counter);
    }

    [PunRPC]
    private void RightScoreUpdateRPC(int counter)
    {
        rightPlayer.counter = counter;
        rightPlayer.score.text = rightPlayer.counter.ToString();
    }

    public void LeftScoreIncrease()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        leftPlayer.counter++;

        photonView.RPC(nameof(LeftScoreUpdateRPC), RpcTarget.All, leftPlayer.counter);

    }

    private void LeftScoreUpdateRPC(int counter)
    {
        leftPlayer.counter = counter;
        leftPlayer.score.text = leftPlayer.counter.ToString();
    }


    public void ResetScore()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        leftPlayer.counter = 0;
        rightPlayer.counter = 0;


    }
}
