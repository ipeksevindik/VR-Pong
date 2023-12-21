using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json.Schema;

using UnityEngine;

public class LeftJoystick : MonoBehaviour, IPlayerMovement
{
    public GameObject TopWall;
    public GameObject BottomWall;

    public GameObject Player;
    private HingeJoint hj;

    float threshold = 20;
    float maxY;
    float minY;
    public Ball ball;
    public Rigidbody rb;
    private float objectHeight;

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
        }
        if (hj.angle > hj.limits.max - threshold)
        {
            Vector3 fVelocity = new Vector3(0, -0.3f, 0);
            rb.velocity = fVelocity;
        }
        if (hj.angle > hj.limits.min + threshold && hj.angle < hj.limits.max - threshold)
        {
            Vector3 fVelocity = new Vector3(0, 0, 0);
            rb.velocity = fVelocity;
        }

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
