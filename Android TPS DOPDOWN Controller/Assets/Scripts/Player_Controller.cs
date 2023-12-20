using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class Player_Controller : MonoBehaviour
{
    [Header("MAIN STUFF")]
    [SerializeField] float health;
    bool is_rotating = false;
    bool is_dead = false;
    float max_health;
    float targetAngle;

    [Space]
    [Header("MOVEMENT")]
    [SerializeField] Joystick joysitck_move;
    [SerializeField] float move_speed;
    float move_angle = 0;
    float hoirontal_move;
    float vertical_move;

    [Space]
    [Header("ROTATION")]
    [SerializeField] Joystick joysitck_rotation;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform player_gfx;
    float hoirontal_rotation;
    float vertical_rotation;

    [Space]
    [Header("CAMERA SETTINGS")]
    [SerializeField] float offset_x;
    [SerializeField] float offset_y;
    [SerializeField] float offset_z;
    [SerializeField] Camera camera;
    [SerializeField] GameObject camera_holder;

    [Space]
    [Header("OTHER STUFF")]
    [SerializeField] GameObject event_system;
    [SerializeField] GameObject canvas_object;
    [SerializeField] Collider player_collider;
    [SerializeField] Canvas_Manager canvas_Manager;
    [SerializeField] ParticleSystem blood_vfx;

    Animator_Controller animator_controller;
    Player_Weapon_1 player_Weapon_1;
    CharacterController controller;
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            controller = GetComponent<CharacterController>();
            animator_controller = GetComponentInChildren<Animator_Controller>();
            player_Weapon_1 = GetComponentInChildren<Player_Weapon_1>();

            max_health = health;
        }
    }

    private void Start()
    {
        if (!pv.IsMine)
        {
            Destroy(camera.gameObject);
            Destroy(controller);
            Destroy(event_system);
            Destroy(canvas_object);
            Destroy(player_collider);
        }
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;
        if (is_dead) return;

        Movement();
        Rotation();
        Camera_Follower();
    }

    void Camera_Follower()
    {
        camera_holder.transform.position = new Vector3(transform.position.x + offset_x, transform.position.y + offset_y, transform.position.z + offset_z);
    }

    void Movement()
    {
        hoirontal_move = joysitck_move.Horizontal;
        vertical_move = joysitck_move.Vertical;

        Vector3 move_direction = new Vector3(hoirontal_move, 0f, vertical_move);

        if (move_direction != Vector3.zero)
        {
            if (is_rotating == false)
            {
                Forward();
                player_gfx.transform.rotation = Quaternion.LookRotation(move_direction);

            }

            controller.Move(move_direction * move_speed * Time.deltaTime);
        }
        else Idle();
    }

    void Rotation()
    {
        hoirontal_rotation = joysitck_rotation.Horizontal;
        vertical_rotation = joysitck_rotation.Vertical;

        Vector3 inputDirection = new Vector3(hoirontal_rotation, 0f, vertical_rotation);

        if (inputDirection != Vector3.zero && is_rotating)
        {
            targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

            Vector3 targetDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            player_gfx.rotation = Quaternion.Slerp(player_gfx.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Check_Rotation_Direction();
            player_Weapon_1.Perfom_Shot();
        }

        if (inputDirection != Vector3.zero) is_rotating = true;
        if (inputDirection == Vector3.zero) is_rotating = false;
    }

    public void TakeDamage(float damage)
    {
        pv.RPC(nameof(RPC_Take_Damage), pv.Owner, damage);        
    }

    [PunRPC]
    void RPC_Take_Damage(float damage, PhotonMessageInfo info)
    {
        health -= 5;

        canvas_Manager.Repaint_Health_Value(health, max_health);
        pv.RPC(nameof(RPC_Show_Blood_VFX), RpcTarget.All);

        if (health <= 0 && !is_dead)
        {
            is_dead = true;
            Death();
        }
    }

    [PunRPC] public void RPC_Show_Blood_VFX() { blood_vfx.Play(); }

    void Death()
    {
        if (is_dead) return;

        animator_controller.Animation_Death();
        is_dead = true;

        pv.RPC(nameof(Death_Case), RpcTarget.All);
    }

    [PunRPC]
    void Death_Case()
    {
        animator_controller.Animation_Death();
        transform.tag = "Untagged";

        Destroy(player_collider);
    }

    public void Camera_Shake()
    {
        camera.transform.DOShakePosition(0.2f, 0.15f);
    }

    #region movement rotation


    void Check_Rotation_Direction()
    {
        move_angle = Mathf.Atan2(hoirontal_move, vertical_move) * Mathf.Rad2Deg;

        if (move_angle > -45 && move_angle < 45) Look_Movement_UP();
        else if (move_angle >= 45 && move_angle < 135) Look_Movement_Right();
        else if (move_angle >= -180 && move_angle < -135 || move_angle >= 135 && move_angle <= 180) Look_Movement_Down();
        else Look_Movement_Left();
    }

    void Look_Movement_UP()
    {
        if (targetAngle >= -45 && targetAngle < 45) Forward();
        if (targetAngle >= 45 && targetAngle < 135) Left();
        if (targetAngle >= 135 && targetAngle < 180 || targetAngle >= -180 && targetAngle < -135) Back();
        if (targetAngle >= -135 && targetAngle < -45) Right();
    }

    void Look_Movement_Right()
    {
        if (targetAngle >= -45 && targetAngle < 45) Right();
        if (targetAngle >= 45 && targetAngle < 135) Forward();
        if (targetAngle >= 135 && targetAngle < 180 || targetAngle >= -180 && targetAngle < -135) Left();
        if (targetAngle >= -135 && targetAngle < -45) Back();
    }

    void Look_Movement_Down()
    {
        if (targetAngle >= -45 && targetAngle < 45) Back();
        if (targetAngle >= 45 && targetAngle < 135) Right();
        if (targetAngle >= 135 && targetAngle < 180 || targetAngle >= -180 && targetAngle < -135) Forward();
        if (targetAngle >= -135 && targetAngle < -45) Left();
    }

    void Look_Movement_Left()
    {
        if (targetAngle >= -45 && targetAngle < 45) Left();
        if (targetAngle >= 45 && targetAngle < 135) Back();
        if (targetAngle >= 135 && targetAngle < 180 || targetAngle >= -180 && targetAngle < -135) Right();
        if (targetAngle >= -135 && targetAngle < -45) Forward();
    }

    void Idle()
    {
        animator_controller.Set_Aimation_Float(0, 0);
    }

    void Forward()
    {
        if (hoirontal_move == 0 || vertical_move == 0) return;

        animator_controller.Set_Aimation_Float(0, 1);
        FindObjectOfType<Canvas_Manager>().Move_Forward();
    }

    void Right()
    {
        if (hoirontal_move == 0 || vertical_move == 0) return;

        animator_controller.Set_Aimation_Float(1, 0);
        FindObjectOfType<Canvas_Manager>().Move_Right();
    }

    void Back()
    {
        if (hoirontal_move == 0 || vertical_move == 0) return;

        animator_controller.Set_Aimation_Float(0, -1);
        FindObjectOfType<Canvas_Manager>().Move_Back();
    }

    void Left()
    {
        if (hoirontal_move == 0 || vertical_move == 0) return;

        animator_controller.Set_Aimation_Float(-1, 0);
        FindObjectOfType<Canvas_Manager>().Move_Left();
    }
    #endregion

}