using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float speedModifier = 2; // make this < 1 if you want enemy slower or > 1 if you want enemy faster

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnAnimatorMove()
    {
        if (navMeshAgent == null)
        {
            return;
        }

        navMeshAgent.speed = (animator.deltaPosition / Time.fixedDeltaTime).magnitude * speedModifier;
    }

    public bool SetFollowTarget(Vector3 position)
    {
        return navMeshAgent.SetDestination(position);
    }
}
