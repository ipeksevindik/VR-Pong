using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.PlayerSettings;

public class Player2_Score : MonoBehaviour
{
    public Ball ball;
    public int Counter = 0;
    public TextMeshProUGUI score;

    private void Start()
    {
        ball = transform.parent.gameObject.GetComponentInChildren<Ball>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ball.gameObject)
        {
            Debug.Log("2++");
            Counter++;
            score.text = Counter.ToString();
        }
    }


}
