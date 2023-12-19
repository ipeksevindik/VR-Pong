using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int MaxPlayer;
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject room_1;
    public GameObject room_2;
    public GameObject connect_button;
    public TextMeshPro load_txt;
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        load_txt.text = "Try Connect To Server";
        Debug.Log("Try Connect To Server");
    }

    public override void OnConnectedToMaster()
    {
        load_txt.text = "Select Room";
        Debug.Log("Connected To Server");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    { 
        base.OnJoinedLobby();
        Debug.Log("WE JOINED THE LOBBY");
        connect_button.SetActive(false);
        room_1.SetActive(true);
        room_2.SetActive(true);
    }

    public void InitiliazeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        // LOAD SCENE
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);

        // CREATE THE ROOM
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.MaxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room");

    }

}
