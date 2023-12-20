using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_Weapon_1 : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float fire_rate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shot_point;
    [SerializeField] ParticleSystem vfx_shoot;
    [SerializeField] ParticleSystem vfx_trace;

    PhotonView pv;
    Transform player_gfx;
    float ray_distance = 15;
    bool can_shot = true;

    [SerializeField]Player_Controller player_controller;

    private void Awake()
    {
        player_gfx = GetComponentInParent<Animator_Controller>().transform;
        pv = GetComponent<PhotonView>();

        player_controller = GetComponentInParent<Player_Controller>();
    }

    public void Perfom_Shot()
    {
        if (!pv.IsMine) return;
        if (!can_shot) return;

        if (can_shot)
        {
            pv.RPC(nameof(Perf_Shot), RpcTarget.All);
            StartCoroutine(Fire_With_Delay());
        }
    }

    [PunRPC]
    void Perf_Shot()
    {
        Vector3 rayOrigin = shot_point.transform.position;
        Vector3 rayDirection = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, ray_distance))
        {
            if (hit.transform.GetComponent<Player_Controller>() != null)
                hit.transform.GetComponent<Player_Controller>().TakeDamage(damage);
        }

        vfx_shoot.Play();
        vfx_trace.Play();
        player_controller.Camera_Shake();
    }

    IEnumerator Fire_With_Delay()
    {
        can_shot = false;
        yield return new WaitForSeconds(fire_rate);
        can_shot = true;
    }
}