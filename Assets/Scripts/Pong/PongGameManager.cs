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

    [SerializeField]
    LeftJoystick LeftJoystick;

    [SerializeField]
    RightJoystick RightJoystick;

    public bool IsPlaying { get; protected set; }

    public int TotalScore => rightPlayerGoal.counter + leftPlayerGoal.counter;
    Vector3 ball_pos;
    Vector3 right_pos;
    Vector3 left_pos;

    private Vector3 player_pos;
    private Coroutine rigthLerpCoroutine;
    private Coroutine leftLerpCoroutine;
    public Rigidbody right_rb;
    public Rigidbody left_rb;



    private void Awake()
    {
        right_rb = rightPlayer.transform.GetComponent<Rigidbody>();
        left_rb = leftPlayer.transform.GetComponent<Rigidbody>();
        ball_pos = ball.transform.position;
        right_pos = rightPlayer.transform.position;
        left_pos = leftPlayer.transform.position;
    }

    private void OnEnable()
    {
        LeftJoystick.LeftPlayerLerp += SetLeftPlayerPosition;
        RightJoystick.RightPlayerLerp += SetRightPlayerPosition;
        rightPlayerGoal.RightOnCollidedBall += RightScoreIncrease;
        leftPlayerGoal.LeftOnCollidedBall += LeftScoreIncrease;

    }

    private void OnDisable()
    {
        LeftJoystick.LeftPlayerLerp -= SetLeftPlayerPosition;
        RightJoystick.RightPlayerLerp -= SetRightPlayerPosition;
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

    #region lerp player positions

    public void SetRightPlayerPosition()
    {
        if (RightJoystick.JoystickPhotonView.IsMine)
            photonView.RPC(nameof(SetRightPlayerPositionRPC), RpcTarget.Others, PhotonNetwork.GetPing(), right_rb.position, right_rb.velocity, true);

    }

    public void SetLeftPlayerPosition()
    {
        if (LeftJoystick.JoystickPhotonView.IsMine)
            photonView.RPC(nameof(SetLeftPlayerPositionRPC), RpcTarget.Others, PhotonNetwork.GetPing(), left_rb.position, left_rb.velocity, true);

    }

    [PunRPC]
    public void SetLeftPlayerPositionRPC(int clientPing, Vector3 leftPosition, Vector3 leftNewVelocity, bool lerp = false)
    {
        if (leftLerpCoroutine is not null)
        {
            StopCoroutine(leftLerpCoroutine);
        }
        left_rb.velocity = leftNewVelocity;
        Vector3 left_newPosition = leftPosition + leftNewVelocity * clientPing * .001f;

        if (lerp)
        {
            leftLerpCoroutine = StartCoroutine(LeftLerpNewPosition(left_rb.position, left_newPosition, leftNewVelocity));
        }

        else
        {
            left_rb.position = leftPosition;
        }
    }


    [PunRPC]
    public void SetRightPlayerPositionRPC(int clientPing, Vector3 rightPosition, Vector3 rigtNewVelocity, bool lerp = false)
    {
        if (rigthLerpCoroutine is not null)
        {
            StopCoroutine(rigthLerpCoroutine);
        }

        right_rb.velocity = rigtNewVelocity;
        Vector3 right_newPosition = rightPosition + rigtNewVelocity * clientPing * .001f;

        if (lerp)
        {
            rigthLerpCoroutine = StartCoroutine(RightLerpNewPosition(right_rb.position, right_newPosition, rigtNewVelocity));
        }
        else
        {
            right_rb.position = rightPosition;
        }
    }

    public IEnumerator RightLerpNewPosition(Vector3 a, Vector3 b, Vector3 rightNewVelocity)
    {
        float t = 0f;
        right_rb.velocity = rightNewVelocity;
        while (t <= .25f)
        {
            Vector3 rightProjectedOld = a + t * rightNewVelocity;
            Vector3 rightProjectedNew = b + t * rightNewVelocity;
            transform.position = Vector3.Lerp(rightProjectedOld, rightProjectedNew, t);
            t += Time.deltaTime;
            right_rb.velocity = rightNewVelocity;
            yield return new WaitForEndOfFrame();
        }
        transform.position = b + t * rightNewVelocity;
        right_rb.velocity = rightNewVelocity;
    }


    public IEnumerator LeftLerpNewPosition(Vector3 a, Vector3 b, Vector3 leftNewVelocity)
    {
        float t = 0f;
        left_rb.velocity = leftNewVelocity;
        while (t <= .25f)
        {
            Vector3 leftProjectedOld = a + t * leftNewVelocity;
            Vector3 leftProjectedNew = b + t * leftNewVelocity;
            transform.position = Vector3.Lerp(leftProjectedOld, leftProjectedNew, t);
            t += Time.deltaTime;
            left_rb.velocity = leftNewVelocity;
            yield return new WaitForEndOfFrame();
        }
        transform.position = b + t * leftNewVelocity;
        left_rb.velocity = leftNewVelocity;
    }

    #endregion

    [PunRPC]
    private void StartGameRPC(Vector3 direction)
    {
        ResetBallPosition();
        ball.SetBallVelocity(direction * ballStartSpeed);
        //SetPlayersPosition();
        IsPlaying = true;
    }


}