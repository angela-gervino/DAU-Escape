using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    private Animator monkey;
    CharacterController characterController;
    public float gravity = 2.0f;
    private Vector3 moveDirection = Vector3.zero;
    private bool Speed1 = true;
    private bool Speed2 = false;
    private bool Speed3 = false;
    // Start is called before the first frame update
    void Start()
    {
        monkey = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(moveDirection * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Speed1 = !Speed1;
            Speed2 = false;
            Speed3 = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Speed2 = !Speed2;
            Speed1 = false;
            Speed3 = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Speed3 = !Speed3;
            Speed1 = false;
            Speed2 = false;
        }
        if (monkey.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            monkey.SetBool("sitting", false);
            monkey.SetBool("idletosit", false);
            monkey.SetBool("sittoidle", false);
            monkey.SetBool("jump", false);
            monkey.SetBool("attack", false);
            monkey.SetBool("jumpinplace", false);
            monkey.SetBool("threat",false);
            monkey.SetBool("hit", false);
        }
        if (monkey.GetCurrentAnimatorStateInfo(0).IsName("sitting"))
        {
            monkey.SetBool("idle", false);
            monkey.SetBool("idletosit", false);
            monkey.SetBool("sittoidle", false);
            monkey.SetBool("jump", false);
            monkey.SetBool("jumprun", false);
            monkey.SetBool("eat", false);
            monkey.SetBool("ungry", false);
            monkey.SetBool("smile", false);
            monkey.SetBool("walk", false);
            monkey.SetBool("walkleft", false);
            monkey.SetBool("walkright", false);
            monkey.SetBool("turnleft", false);
            monkey.SetBool("turnright", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("attack", false);
        }
        if (monkey.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            monkey.SetBool("jumprun", false);
        }
        if (monkey.GetCurrentAnimatorStateInfo(0).IsName("climbing"))
        {
            monkey.SetBool("sittoclimb", false);
            monkey.SetBool("climbtosit", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            monkey.SetBool("idle", false);
            monkey.SetBool("idletosit", true);
            monkey.SetBool("sittoidle", true);

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            monkey.SetBool("turnleft", true);
            monkey.SetBool("sitting", false);
            monkey.SetBool("runleft", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            monkey.SetBool("turnright", true);
            monkey.SetBool("sitting", false);
            monkey.SetBool("runleft", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            monkey.SetBool("idle", false);
            monkey.SetBool("backward", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            monkey.SetBool("backward", false);
            monkey.SetBool("idle", true);
        }
        if (Input.GetKeyDown(KeyCode.W) && (Speed1 == true))
        {
            monkey.SetBool("idle", false);
            monkey.SetBool("walk", true);
            monkey.SetBool("sitting", false);
        }
        if (Input.GetKeyUp(KeyCode.W) && (Speed1 == true))
        {
            monkey.SetBool("idle", true);
            monkey.SetBool("walk", false);
        }
        if (Input.GetKeyDown(KeyCode.A) && (Speed1 == true))
        {
            monkey.SetBool("walk", false);
            monkey.SetBool("walkleft", true);
        }
        if (Input.GetKeyUp(KeyCode.A) && (Speed1 == true))
        {
            monkey.SetBool("walk", true);
            monkey.SetBool("walkleft", false);
        }
        if (Input.GetKeyDown(KeyCode.D) && (Speed1 == true))
        {
            monkey.SetBool("walk", false);
            monkey.SetBool("walkright", true);
        }
        if (Input.GetKeyUp(KeyCode.D) && (Speed1 == true))
        {
            monkey.SetBool("walk", true);
            monkey.SetBool("walkright", false);
        }
        if (Input.GetKeyDown(KeyCode.W) && (Speed2 == true))
        {
            monkey.SetBool("idle", false);
            monkey.SetBool("run", true);
            monkey.SetBool("sitting", false);
        }
        if (Input.GetKeyUp(KeyCode.W) && (Speed2 == true))
        {
            monkey.SetBool("idle", true);
            monkey.SetBool("run", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && (Speed2 == true))
        {
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", true);
        }
        if (Input.GetKeyUp(KeyCode.A) && (Speed2 == true))
        {
            monkey.SetBool("run", true);
            monkey.SetBool("runleft", false);
        }
        if (Input.GetKeyDown(KeyCode.D) && (Speed2 == true))
        {
            monkey.SetBool("run", false);
            monkey.SetBool("runright", true);
        }
        if (Input.GetKeyUp(KeyCode.D) && (Speed2 == true))
        {
            monkey.SetBool("run", true);
            monkey.SetBool("runright", false);
        }

        if (Input.GetKeyDown(KeyCode.W) && (Speed3 == true))
        {
            monkey.SetBool("climbing", false);
            monkey.SetBool("climb", true);
        }
        if (Input.GetKeyUp(KeyCode.W) && (Speed3 == true))
        {
            monkey.SetBool("climbing", true);
            monkey.SetBool("climb", false);
        }
        if (Input.GetKeyDown(KeyCode.S) && (Speed3 == true))
        {
            monkey.SetBool("climbing", false);
            monkey.SetBool("climbdown", true);
        }
        if (Input.GetKeyUp(KeyCode.S) && (Speed3 == true))
        {
            monkey.SetBool("climbing", true);
            monkey.SetBool("climbdown", false);
        }
        if (Input.GetKeyDown(KeyCode.A) && (Speed3 == true))
        {
            monkey.SetBool("climbing", false);
            monkey.SetBool("climbleft", true);
        }
        if (Input.GetKeyUp(KeyCode.A) && (Speed3 == true))
        {
            monkey.SetBool("climbing", true);
            monkey.SetBool("climbleft", false);
        }
        if (Input.GetKeyDown(KeyCode.D) && (Speed3 == true))
        {
            monkey.SetBool("climbing", false);
            monkey.SetBool("climbright", true);
        }
        if (Input.GetKeyUp(KeyCode.D) && (Speed3 == true))
        {
            monkey.SetBool("climbing", true);
            monkey.SetBool("climbright", false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && (Speed3 == true))
        {
            monkey.SetBool("sitting", false);
            monkey.SetBool("sittoclimb", true);
            monkey.SetBool("climbtosit", true);
            monkey.SetBool("climbing", false);
            monkey.SetBool("sittoidle", false);
            monkey.SetBool("idletosit", false);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            monkey.SetBool("idle", false);
            monkey.SetBool("jump", true);
            monkey.SetBool("sitting", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            monkey.SetBool("eat", true);
            monkey.SetBool("sitting", false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            monkey.SetBool("ungry", true);
            monkey.SetBool("sitting", false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            monkey.SetBool("smile", true);
            monkey.SetBool("sitting", false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            monkey.SetBool("attack", true);
            monkey.SetBool("idle", false);
            monkey.SetBool("sitting", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("walk", false);
            monkey.SetBool("walkleft", false);
            monkey.SetBool("walkright", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            monkey.SetBool("jumpinplace", true);
            monkey.SetBool("idle", false);
            monkey.SetBool("jumprun", true);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            monkey.SetBool("threat", true);
            monkey.SetBool("idle", false);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            monkey.SetBool("hit", true);
            monkey.SetBool("idle", false);
            monkey.SetBool("sitting", false);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            monkey.SetBool("die", true);
            monkey.SetBool("idle", false);
            monkey.SetBool("sitting", false);
        }
    }
}
