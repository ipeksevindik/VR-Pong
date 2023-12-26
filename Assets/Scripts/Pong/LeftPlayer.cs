using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeftPlayer : MonoBehaviour
{
    public TextMeshProUGUI score;
    public float counter;
    public Ball ball;

    void Start()
    {
        ball = transform.parent.GetComponentInChildren<Ball>();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ball.gameObject)
        {
            counter++;
            score.text = counter.ToString();
        }
    }
}
