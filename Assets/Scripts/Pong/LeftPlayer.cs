using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LeftPlayer : MonoBehaviour
{
    public Action LeftOnCollidedBall;
    public TextMeshProUGUI score;
    public int counter;
    public Ball ball;
    PongGameManager gameManager;


    private void OnEnable()
    {
        LeftOnCollidedBall += gameManager.RightScoreIncrease;
    }

    private void OnDisable()
    {
        LeftOnCollidedBall -= gameManager.RightScoreIncrease;

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
            LeftOnCollidedBall?.Invoke();
        }
    }
}
