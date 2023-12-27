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

    void Awake()
    {
        ball = transform.parent.GetComponentInChildren<Ball>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ball.gameObject)
        {
            Debug.Log("left player");
            LeftOnCollidedBall?.Invoke();
        }
    }
}
