using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

public class PongGameManager : MonoBehaviourPun
{
    [SerializeField] float ballStartSpeed = 0.01f;

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
    Vector3 ball_pos;
    Vector3 right_pos;
    Vector3 left_pos;

    private Vector3 player_pos;


    private void Awake()
    {
        ball_pos = ball.transform.position;
        right_pos = rightPlayer.transform.position;
        left_pos = leftPlayer.transform.position;
    }

    private void Start()
    {
        Invoke(nameof(SetPlayersPosition), 1);

    }

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

    #region "Score"

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

    [PunRPC]
    private void RightScoreUpdateRPC(int counter, Vector3 direction)
    {
        SetRightCounter(counter);
        ResetBallPosition();
        ball.SetBallVelocity(direction * ballStartSpeed);

    }

    [PunRPC]
    private void LeftScoreUpdateRPC(int counter, Vector3 direction)
    {
        SetLeftCounter(counter);
        ResetBallPosition();
        ball.SetBallVelocity(direction * ballStartSpeed);

    }


    #endregion

    private void ResetPlayerPositions()
    {
        rightPlayer.transform.position = right_pos;
        leftPlayer.transform.position = left_pos;
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

    private void ResetBallPosition()
    {
        ball.transform.position = ball_pos;
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
    private void EndGameRPC()
    {
        ResetBallPosition();
        ball.SetBallVelocity(Vector3.zero);
        SetLeftCounter(0);
        SetRightCounter(0);
        ResetPlayerPositions();

        IsPlaying = false;
    }


    [PunRPC]
    public void ResetBallPositionRPC()
    {
        ResetBallPosition();
        ball.SetBallVelocity(Vector3.zero);

    }

    [PunRPC]
    public void ResetPlayerPositionRPC()
    {
        ResetPlayerPositions();
    }

    public void SetPlayersPosition()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC(nameof(SetPlayersPositionRPC), RpcTarget.All, leftPlayer.transform.position, rightPlayer.transform.position);
        Invoke(nameof(SetPlayersPosition), 1);

    }

    [PunRPC]
    public void SetPlayersPositionRPC(Vector3 leftPosition, Vector3 rightPosition)
    {
        leftPlayer.transform.position = leftPosition;
        rightPlayer.transform.position = rightPosition;

    }

    [PunRPC]
    private void StartGameRPC(Vector3 direction)
    {
        ResetBallPosition();
        ball.SetBallVelocity(direction * ballStartSpeed);
        IsPlaying = true;
    }
}
