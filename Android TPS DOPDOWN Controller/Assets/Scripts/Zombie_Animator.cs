using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Animator : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Animation_Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void Animation_Death()
    {
        animator.SetTrigger("Death");
    }

}