using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Controller : MonoBehaviour
{



    [SerializeField] float health;
    [SerializeField] Transform player;
    [SerializeField] float distance_to_attack;

    float distance = 0;                                   // distance to player
    float rotationSpeed = 10;                            // rotation speed to player in melee zone

    bool can_attack = true;
    bool is_dead = false;
    bool can_move = true;

    NavMeshAgent agent;
    Zombie_Animator animator_script;

    [SerializeField] List<GameObject> playerList = new List<GameObject>();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator_script = GetComponentInChildren<Zombie_Animator>();
    }

    private void Start()
    {
        // Find all GameObjects with a specific tag (e.g., "Player") and add them to the playerList
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        playerList.AddRange(playerObjects);
    }

    private void FixedUpdate()
    {
        
    }

}



/*
 * p

private void FixedUpdate()
{
    if (is_dead) return;

    distance = Vector3.Distance(transform.position, player.transform.position);

    if (distance > distance_to_attack && can_move && !is_dead)
        NPC_Follow_Player();

    if (distance <= distance_to_attack && can_attack && !is_dead)
        NPC_Attack_Player();
}

void NPC_Follow_Player()
{
    agent.SetDestination(player.position);
}

void NPC_Attack_Player()
{
    agent.SetDestination(transform.position);

    Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    StartCoroutine(Attack_Delay());
}

IEnumerator Attack_Delay()
{
    animator_script.Animation_Attack();

    // if (distance < 1) player.GetComponent<Player_Controller>().Deduct_Health();

    can_attack = false;
    can_move = false;

    yield return new WaitForSeconds(1);

    can_attack = true;
    can_move = true;
}

public void Deduct_Health(float amount)
{
    health -= amount;

    if (health <= 0) Death();
}

void Death()
{
    if (is_dead) return;

    is_dead = true;
    can_attack = false;
    can_move = false;

    GetComponent<Collider>().enabled = false;
    GetComponent<MeshCollider>().enabled = false;

    agent.SetDestination(transform.position);

    animator_script.Animation_Death();
    Destroy(transform.gameObject, 4f);
}*/