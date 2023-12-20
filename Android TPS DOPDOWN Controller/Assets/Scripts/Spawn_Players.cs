using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Photon.Pun;

public class Spawn_Players : MonoBehaviour
{
    [SerializeField] GameObject player_prefab;
    [SerializeField] Transform[] spawn_points;

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            int currentPlayerIndex = GetCurrentPlayerIndex();

            PhotonNetwork.Instantiate(player_prefab.name, spawn_points[currentPlayerIndex].position, Quaternion.identity);
        }
    }

    private int GetCurrentPlayerIndex()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        Photon.Realtime.Player localPlayer = PhotonNetwork.LocalPlayer;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == localPlayer)
                return i;
        }

        return -1; // Local player not found in the player list (should not happen in a room).
    }
}