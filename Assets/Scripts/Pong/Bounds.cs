using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    public GameObject Wall;

    public Ball ball;

    private float objectHeight;

    private void Start()
    {
        ball = transform.parent.parent.gameObject.GetComponentInChildren<Ball>();

        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

  
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
        Vector3 newVelocity = Vector3.Reflect(ball.rb.velocity, contactPoint.normal);
        ball.rb.velocity = newVelocity;
    }

}
