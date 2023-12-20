using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    public Ball ball;

    private void Start()
    {
        ball = transform.parent.gameObject.GetComponentInChildren<Ball>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
        Vector3 newVelocity = Vector3.Reflect(ball.rb.velocity, contactPoint.normal);
        ball.rb.velocity = newVelocity;
    }
}
