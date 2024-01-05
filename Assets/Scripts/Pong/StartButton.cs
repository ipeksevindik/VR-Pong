using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StartButton : XRSimpleInteractable
{
    public Ball ball;
    public PongGameManager pongGameManager = null;

    private void Start()
    {
        ball = transform.parent.parent.GetComponentInChildren<Ball>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("select entered");

        if (!pongGameManager.IsPlaying)
        {
            Debug.Log("ball stopped");
            pongGameManager.StartGame();
            // ball.photonView.RPC("AddStartingForce", RpcTarget.AllBuffered);
        }

    }

    [ContextMenu("StartGame")]
    public void StartGame()
    {
        if (!pongGameManager.IsPlaying)
        {
            pongGameManager.StartGame();
            // ball.photonView.RPC("AddStartingForce", RpcTarget.AllBuffered);
        }
    }
}
