using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Photon_Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField input_field;
    [SerializeField] int room_size;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("CONNECTED TO MASTERAS");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("CONNECTED TO LOBY");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 10000).ToString("0000");
    }

    public void Create_Room()
    {
        RoomOptions room_settings =
        new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = room_size,
        };

        Hashtable room_custom_props = new Hashtable();

        PhotonNetwork.CreateRoom(input_field.text.ToString(), room_settings);
    }

    public override void OnJoinedRoom()
    {
        Count_And_Start();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Count_And_Start();
    }

    void Count_And_Start()
    {
        Debug.Log("IN ROOM: " + PhotonNetwork.PlayerList.Length);

        if (PhotonNetwork.PlayerList.Length >= room_size)        
            PhotonNetwork.LoadLevel(1);
    }

    public void Connect_Random_Room()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}