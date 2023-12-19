using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Ball : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    public Action StartGame;

    Vector3 pos;

    private void Start()
    {
        pos = transform.position;

    }

    private void OnEnable()
    {
        StartGame += AddStartingForce;
    }

    private void OnDisable()
    {
        StartGame -= AddStartingForce;
    }


    [ContextMenu("AddStartingForce")]
    public void AddStartingForce()
    {
        Debug.Log("deneme");
        ResetPosition();
        float x = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
        float y = UnityEngine.Random.value < 0.5f ? UnityEngine.Random.Range(-1.0f, -0.5f) :
                                                    UnityEngine.Random.Range(0.5f, 1.0f);

        Vector3 direction = new Vector3(0, x, y);
        rb.AddForce(direction * this.speed);
    }


    public void ResetPosition()
    {
        transform.position = pos;
        rb.velocity = Vector3.zero;
    }
  
}
