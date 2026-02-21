using System.Collections;
using System.Collections.Generic;
using DAUEscape;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IAttackAnimListener
{
    public Animator Animator { get { return animator; } }

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float speedModifier = 2; // make this < 1 if you want enemy slower or > 1 if you want enemy faster

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }// Awake


    public void MeleeAttackStart()
    {

    }// MeleeAttackStart


    public void MeleeAttackEnd()
    {

    }// MeleeAttackEnd


    private void OnAnimatorMove()
    {
        if (navMeshAgent.enabled)
        {
            navMeshAgent.speed = (animator.deltaPosition / Time.fixedDeltaTime).magnitude * speedModifier;
        }

    }// OnAnimatorMove


    public bool FollowTarget(Vector3 position)
    {
        if (!navMeshAgent.enabled)
        {
            navMeshAgent.enabled = true;
        }
        return navMeshAgent.SetDestination(position);
    }// FollowTarget


    public void StopFollowTarget()
    {
        navMeshAgent.enabled = false;
    }// StopFollowTarget
}
