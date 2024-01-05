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
using System.Collections;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public PhotonView photonView;

    PongGameManager gameManager;

    PongAudioManager audioManager;

    private Vector3 ballVelocity;
    private Coroutine lerpCoroutine;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        gameManager = transform.parent.GetComponentInChildren<PongGameManager>();
        audioManager = transform.parent.GetComponentInChildren<PongAudioManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        audioManager.PlayBallHit();

        ContactPoint contactPoint = other.contacts[0];
        Vector3 normal = Perpendiculate(contactPoint.normal);
        ballVelocity = Vector3.Reflect(ballVelocity, normal);

        rb.velocity = ballVelocity;
        // Debug.DrawRay(contactPoint.point, normal, Color.cyan, 20f);
        // Debug.DrawRay(contactPoint.point, ballVelocity, Color.yellow, 20f);

        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC(nameof(BallNewVelocity), RpcTarget.Others, ballVelocity, rb.position, true);

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
    public void BallNewVelocity(Vector3 newVelocity, Vector3 pos, bool lerp = false)
    {
        if (lerpCoroutine is not null)
            StopCoroutine(lerpCoroutine);

        ballVelocity = newVelocity;
        Vector3 newPosition = pos + newVelocity * PhotonNetwork.GetPing() * .0005f;
        rb.velocity = newVelocity;

        Debug.DrawLine(newPosition, newPosition + newVelocity * .25f, Color.blue, 2f);
        if (PhotonNetwork.IsMasterClient)
        {
            rb.position = pos;
            return;
        }
        if (lerp)
            lerpCoroutine = StartCoroutine(LerpNewPosition(rb.position, newPosition, newVelocity));
        else
            rb.position = pos;
    }

    IEnumerator LerpNewPosition(Vector3 a, Vector3 b, Vector3 newVelocity)
    {
        float t = 0f;
        rb.velocity = newVelocity;
        while (t <= .25f)
        {
            Vector3 projectedOld = a + t * newVelocity;
            Vector3 projectedNew = b + t * newVelocity;
            Debug.DrawLine(b, projectedNew, Color.red, .1f);
            transform.position = Vector3.Lerp(projectedOld, projectedNew, t);
            t += Time.deltaTime;
            rb.velocity = newVelocity;
            yield return new WaitForEndOfFrame();
        }
        transform.position = b + t * newVelocity;
        rb.velocity = newVelocity;
    }


    public void SetBallVelocity(Vector3 newVelocity)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC(nameof(BallNewVelocity), RpcTarget.All, newVelocity, transform.position, false);
    }

}


