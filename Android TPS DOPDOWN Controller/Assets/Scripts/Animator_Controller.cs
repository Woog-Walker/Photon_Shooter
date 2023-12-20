using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Controller : MonoBehaviour
{
    Animator animator;

    float temp_smooth = 10;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Set_Aimation_Float(float hor_, float ver_)
    {
        animator.SetFloat("move_hor", hor_, 1, Time.fixedDeltaTime * temp_smooth);
        animator.SetFloat("move_ver", ver_, 1, Time.fixedDeltaTime * temp_smooth);
    }

    public void Animation_Death()
    {
        animator.SetTrigger("Anim_Death");
    }
}