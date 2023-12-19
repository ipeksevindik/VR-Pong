using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleTrigger : MonoBehaviour
{
    public GameManager gameManager;
    private void Start()
    {
        //gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (other.transform.CompareTag("Pull") && gameManager.isStoped)
        {
            gameManager.OnHandlePulled();
        }
    }

}
