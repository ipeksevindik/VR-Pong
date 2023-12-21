using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StartButton : XRSimpleInteractable
{
    public Ball ball;

    private void Start()
    {
        ball = FindObjectOfType<Ball>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("select entered");

        if (!ball.isPlaying)
        {
            Debug.Log("ball stopped");

            ball.photonView.RPC("AddStartingForce", RpcTarget.AllBuffered);
        }

    }
}
