using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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
        Vector3 normal = Perpendiculate(contactPoint.normal);
        Vector3 newVelocity = Vector3.Reflect(ball.rb.velocity, normal);
        ball.rb.velocity = newVelocity;
        //Debug.Break();

        if (!PhotonNetwork.IsMasterClient)
            return;

        ball.photonView.RPC(nameof(BallNewVelocity), RpcTarget.Others, newVelocity, ball.rb.position);
    }

    private Vector3 Perpendiculate(Vector3 normal)
    {
        float max = Mathf.Max(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));
        if (Mathf.Abs(normal.x) == max)
            return (Vector3.right * normal.x).normalized;
        if (Mathf.Abs(normal.y) == max)
            return (Vector3.up * normal.y).normalized;
        return (Vector3.forward * normal.z).normalized;
    }

    [PunRPC]
    public void BallNewVelocity(Vector3 newVelocity, Vector3 pos)
    {
        ball.rb.position = pos;
        ball.rb.position += newVelocity * PhotonNetwork.GetPing();
        ball.rb.velocity = newVelocity;
    }
}
