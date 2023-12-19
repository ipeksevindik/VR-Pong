using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Goal : MonoBehaviour
{
    public Ball ball;

    public int Counter;
    public TextMeshProUGUI score;

    private void Start()
    {
        ball = transform.parent.gameObject.GetComponentInChildren<Ball>();

    }
 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ball.gameObject)
        {
            Debug.Log("dlþfmdlsþfmdlsþ");

            Counter++;
            score.text = Counter.ToString();            
            ball.StartGame();
            if (Counter >= 7)
            {
                ball.ResetPosition();
                ResetCounter();

            }
        }
    }

    public void ResetCounter()
    {
        Counter = 0;
    }

}
