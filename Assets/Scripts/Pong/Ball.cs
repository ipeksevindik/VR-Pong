using Photon.Realtime;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;
using Photon.Pun;
using System.Threading.Tasks;

public class Ball : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    Vector3 pos;
    public TextMeshProUGUI Player1_score;
    public TextMeshProUGUI Player2_score;
    public int Player1_counter = 0;
    public int Player2_counter = 0;
    public GameObject Player1_Goal;
    public GameObject Player2_Goal;
    private PhotonView photonView;


    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        pos = transform.position;
    }

    [ContextMenu("AddStartingForce")]
    [PunRPC]
    public void AddStartingForce()
    {
        ResetPosition();
        float x = UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
        float y = UnityEngine.Random.value < 0.5f ? UnityEngine.Random.Range(-1.0f, -0.5f) :
                                                    UnityEngine.Random.Range(0.5f, 1.0f);

        Vector3 direction = new Vector3(0, x, y);
        rb.AddForce(direction * this.speed);
    }

    private async void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == Player1_Goal.gameObject)
        {
            Player1_counter++;
            Player1_score.text = Player1_counter.ToString();
        }

        if (other.gameObject == Player2_Goal.gameObject)
        {
            Player2_counter++;
            Player2_score.text = Player2_counter.ToString();
        }

        if (other.gameObject == Player1_Goal.gameObject || other.gameObject == Player2_Goal.gameObject)
        {
            if (Player1_counter + Player2_counter >= 7)
            {
                await WaitScore();
                ResetPosition();
                Debug.Log("Reset");
                Player1_counter = 0;
                Player1_score.text = Player1_counter.ToString();

                Player2_counter = 0;
                Player2_score.text = Player2_counter.ToString();
            }
            if (Player1_counter + Player2_counter < 7)
            {
                photonView.RPC(nameof(AddStartingForce), RpcTarget.AllBuffered);
                //AddStartingForce();
            }
        }
    }

    async Task WaitScore()
    {
        await Task.Delay(2000);
    }


    public void ResetPosition()
    {
        transform.position = pos;
        rb.velocity = Vector3.zero;
    }

}
