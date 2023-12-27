using Photon.Realtime;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Ball : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    Vector3 pos;
    Vector3 player1_pos;
    Vector3 player2_pos;

    public int Player1_counter = 0;
    public int Player2_counter = 0;
    public PhotonView photonView;

    public GameObject player_1;
    public GameObject player_2;

    public bool isPlaying;


    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        pos = transform.position;
        player1_pos = player_1.transform.position;
        player2_pos = player_2.transform.position;
        isPlaying = false;
    }

    [PunRPC]
    public void AddStartingForce()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        isPlaying = true;
        photonView.RPC(nameof(ResetBallPosition), RpcTarget.All);

        Vector3 direction = BallMove();

        photonView.RPC(nameof(SetVelocityRPC), RpcTarget.All, direction);
    }

    [PunRPC]
    private void SetVelocityRPC(Vector3 direction)
    {
        rb.AddForce(direction * this.speed);
    }

    public Vector3 BallMove()
    {
        float x = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
        float y = UnityEngine.Random.value < 0.5f ? UnityEngine.Random.Range(-1.0f, -0.5f) :
                                                    UnityEngine.Random.Range(0.5f, 1.0f);

        Vector3 dir = new Vector3(0, x, y).normalized;

        return dir;
    }


    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contactPoint = other.contacts[0];
        Vector3 normal = Perpendiculate(contactPoint.normal);
        Vector3 newVelocity = Vector3.Reflect(rb.velocity, normal);
        rb.velocity = newVelocity;
        //Debug.Break();

        if (!PhotonNetwork.IsMasterClient)
            return;

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

    [PunRPC]
    public void ResetBallPosition()
    {
        transform.position = pos;
        rb.velocity = Vector3.zero;
    }

    // public void ResetPlayerPosition()
    // {
    //     player_1.transform.position = player1_pos;
    //     player_2.transform.position = player2_pos;
    // }


}


