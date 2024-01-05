using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RightJoystick : MonoBehaviour, IPlayerMovement
{
    public PhotonView JoystickPhotonView;
    public GameObject TopWall;
    public GameObject BottomWall;

    public GameObject Player;
    public Rigidbody rb;
    private HingeJoint hj;
    public Ball ball;


    float threshold = 20;
    float maxY;
    float minY;

    private float objectHeight;
    private bool isTriggeringPrevious;
    private bool isTriggering;
    public Action RightPlayerLerp;

    private void Start()
    {
        hj = GetComponent<HingeJoint>();
        rb = Player.transform.GetComponent<Rigidbody>();
        objectHeight = Player.transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

    }

    private void Update()
    {
        PlayerMove();

    }
    private void FixedUpdate()
    {
        Boundaries();
    }


    public void PlayerMove()
    {
        if (hj.angle < hj.limits.min + threshold)
        {
            Vector3 fVelocity = new Vector3(0, 0.3f, 0);
            rb.velocity = fVelocity;
            isTriggering = true;

        }
        if (hj.angle > hj.limits.max - threshold)
        {
            Vector3 fVelocity = new Vector3(0, -0.3f, 0);
            rb.velocity = fVelocity;
            isTriggering = true;

        }
        if (hj.angle > hj.limits.min + threshold && hj.angle < hj.limits.max - threshold)
        {
            Vector3 fVelocity = new Vector3(0, 0, 0);
            rb.velocity = fVelocity;
            isTriggering = false;
        }
        if (isTriggeringPrevious != isTriggering)
        {
            RightPlayerLerp?.Invoke();
        }

        isTriggeringPrevious = isTriggering;

    }

    public void Boundaries()
    {
        maxY = TopWall.transform.position.y;
        minY = BottomWall.transform.position.y;

        Vector3 pos = Player.transform.position;
        pos.y = Mathf.Clamp(pos.y, minY + objectHeight, maxY - objectHeight);
        Player.transform.position = pos;

    }
}
