using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun;


public class RightPlayer : MonoBehaviour
{
    public Action RightOnCollidedBall;
    public TextMeshProUGUI score;
    public int counter;
    public Ball ball;
    PongGameManager gameManager;

    private void OnEnable()
    {
        RightOnCollidedBall += gameManager.RightScoreIncrease;
    }

    private void OnDisable()
    {
        RightOnCollidedBall -= gameManager.RightScoreIncrease;

    }

    void Awake()
    {
        gameManager = transform.parent.GetComponentInChildren<PongGameManager>();
        ball = transform.parent.GetComponentInChildren<Ball>();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ball.gameObject)
        {
            Debug.Log("right player");
            RightOnCollidedBall?.Invoke();
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            ball.photonView.RPC("AddStartingForce", RpcTarget.AllBuffered);

        }
    }
}
