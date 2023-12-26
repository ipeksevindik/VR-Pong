using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RightPlayer : MonoBehaviour
{
    public Action OnCollidedBall;
    public TextMeshProUGUI core;
    public float counter;
    public Ball ball;

    void Start()
    {
        ball = transform.parent.GetComponentInChildren<Ball>();
        //leftPaddle.OnCollidedBall += LeftScoreIncrease();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ball.gameObject)
            OnCollidedBall.Invoke();
    }
}
