using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun;


public class LeftPlayer : MonoBehaviour
{
    public Action LeftOnCollidedBall;
    public TextMeshProUGUI score;
    public int counter;
    public Ball ball;
    PongGameManager gameManager;


    private void OnEnable()
    {
        LeftOnCollidedBall += gameManager.LeftScoreIncrease;
    }

    private void OnDisable()
    {
        LeftOnCollidedBall -= gameManager.LeftScoreIncrease;
    }

    void Awake()
    {
        ball = transform.parent.GetComponentInChildren<Ball>();
        gameManager = transform.parent.GetComponentInChildren<PongGameManager>();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ball.gameObject)
        {
            Debug.Log("left player");
            LeftOnCollidedBall?.Invoke();
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            ball.photonView.RPC("AddStartingForce", RpcTarget.AllBuffered);

        }
    }
}
