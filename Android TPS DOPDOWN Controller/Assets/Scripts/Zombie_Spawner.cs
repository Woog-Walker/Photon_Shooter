using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Zombie_Spawner : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject zombie_prefab;
    [SerializeField] float spawn_cd;
    [SerializeField] Transform[] spawn_points;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Spawner_Delay());
            Debug.Log("start spawner at master client");
        }
    }

    IEnumerator Spawner_Delay()
    {
        yield return new WaitForSeconds(spawn_cd);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("ALL LOADED AND SPAWN ZOMBIE");
            int rnd_index = Random.Range(0, spawn_points.Length);
            PhotonNetwork.InstantiateSceneObject(zombie_prefab.name, spawn_points[rnd_index].position, Quaternion.identity);

            StartCoroutine(Spawner_Delay());
        }
    }
}