using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PongGameManager : MonoBehaviourPun
{
    [SerializeField] float ballStartSpeed = .5f;
    [SerializeField] Transform ballInitialTransform = null;
    [SerializeField] Transform leftInitialTransform = null;
    [SerializeField] Transform rightInitialTransform = null;
    [SerializeField]
    RightPlayer rightPlayerGoal;

    [SerializeField]
    LeftPlayer leftPlayerGoal;
    [SerializeField]
    Transform leftPlayer;
    [SerializeField]
    Transform rightPlayer;

    [SerializeField]
    Ball ball;
    public bool IsPlaying { get; protected set; }

    public int TotalScore => rightPlayerGoal.counter + leftPlayerGoal.counter;

    private void OnEnable()
    {
        rightPlayerGoal.RightOnCollidedBall += RightScoreIncrease;
        leftPlayerGoal.LeftOnCollidedBall += LeftScoreIncrease;
    }

    private void OnDisable()
    {
        rightPlayerGoal.RightOnCollidedBall -= RightScoreIncrease;
        leftPlayerGoal.LeftOnCollidedBall -= LeftScoreIncrease;
    }

    public void StartGame()
    {
        Vector3 startingDirection = GenerateRandomDirection();
        photonView.RPC(nameof(StartGameRPC), RpcTarget.All, startingDirection);
    }

    public void RightScoreIncrease()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        rightPlayerGoal.counter++;

        if (TotalScore == 7)
        {
            photonView.RPC(nameof(EndGameRPC), RpcTarget.All);
            return;
        }

        Vector3 randomDirection = GenerateRandomDirection();

        photonView.RPC(nameof(RightScoreUpdateRPC), RpcTarget.All, rightPlayerGoal.counter, randomDirection);
    }

    public void LeftScoreIncrease()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        leftPlayerGoal.counter++;

        if (TotalScore == 7)
        {
            photonView.RPC(nameof(EndGameRPC), RpcTarget.All);
            return;
        }

        Vector3 randomDirection = GenerateRandomDirection();

        photonView.RPC(nameof(LeftScoreUpdateRPC), RpcTarget.All, leftPlayerGoal.counter, randomDirection);
    }

    private void ResetPlayerPositions()
    {
        rightPlayer.transform.position = rightInitialTransform.position;
        leftPlayer.transform.position = leftInitialTransform.position;
    }

    private void SetRightCounter(int count)
    {
        rightPlayerGoal.counter = count;
        rightPlayerGoal.score.text = rightPlayerGoal.counter.ToString();
    }

    private void SetLeftCounter(int count)
    {
        leftPlayerGoal.counter = count;
        leftPlayerGoal.score.text = leftPlayerGoal.counter.ToString();
    }

    private void ResetBall(Vector3 velocity)
    {
        ball.transform.position = ballInitialTransform.position;
        ball.rb.velocity = velocity;
    }

    public Vector3 GenerateRandomDirection()
    {
        float x = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
        float y = UnityEngine.Random.value < 0.5f ? UnityEngine.Random.Range(-1.0f, -0.5f) :
                                                    UnityEngine.Random.Range(0.5f, 1.0f);

        Vector3 dir = new Vector3(0, x, y).normalized;

        return dir;
    }

    [PunRPC]
    private void RightScoreUpdateRPC(int counter, Vector3 direction)
    {
        SetRightCounter(counter);
        ResetBall(direction * ballStartSpeed);
    }

    [PunRPC]
    private void EndGameRPC()
    {
        ResetBall(Vector3.zero);
        SetLeftCounter(0);
        SetRightCounter(0);
        ResetPlayerPositions();

        IsPlaying = false;
    }

    [PunRPC]
    private void LeftScoreUpdateRPC(int counter, Vector3 direction)
    {
        SetLeftCounter(counter);
        ResetBall(direction * ballStartSpeed);
    }

    [PunRPC]
    public void ResetBallPositionRPC()
    {
        ResetBall(Vector3.zero);
    }

    [PunRPC]
    public void ResetPlayerPositionRPC()
    {
        ResetPlayerPositions();
    }

    [PunRPC]
    private void StartGameRPC(Vector3 direction)
    {
        ResetBall(direction * ballStartSpeed);
        IsPlaying = true;
    }
}
