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
        ball = transform.parent.parent.GetComponentInChildren<Ball>();
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

    [ContextMenu("StartGame")]
    public void StartGame()
    {
        if (!ball.isPlaying)
        {
            Debug.Log("neden çalışmıyosun????");
            ball.photonView.RPC("AddStartingForce", RpcTarget.AllBuffered);
        }
    }
}
