using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet_From_Weapon : MonoBehaviour
{
    float damage = 20;
    [SerializeField] float velocity;
    [SerializeField] ParticleSystem blood_vfx;

    [SerializeField] PhotonView pv;

    private void Start()
    {
        if (!pv.IsMine) return;

        Destroy(transform.gameObject, 1.5f);
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        transform.Translate(Vector3.forward * Time.fixedDeltaTime * velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pv.IsMine) return;

        if (other.CompareTag("NPC_Enemy_Simple"))
        {
            // other.transform.GetComponent<Zombie_Controller>().Deduct_Health(damage);

            blood_vfx.transform.parent = null;
            blood_vfx.Play();

            Destroy(transform.gameObject);
        }else
        {
            Destroy(transform.gameObject);
        }
    }
}