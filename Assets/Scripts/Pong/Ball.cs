using Photon.Realtime;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public PhotonView photonView;

    PongGameManager gameManager;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        gameManager = transform.parent.GetComponentInChildren<PongGameManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        ContactPoint contactPoint = other.contacts[0];
        Vector3 normal = Perpendiculate(contactPoint.normal);
        Vector3 newVelocity = Vector3.Reflect(rb.velocity, normal);

        rb.velocity = newVelocity;

        photonView.RPC(nameof(BallNewVelocity), RpcTarget.Others, newVelocity, rb.position);

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
        rb.position = pos;
        rb.position += newVelocity * PhotonNetwork.GetPing() * .001f;
        rb.velocity = newVelocity;
        Debug.DrawRay(pos, newVelocity, Color.blue, 20f);
    }
}


