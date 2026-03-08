using System.Collections;
using System.Collections.Generic;
using DAUEscape;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IAttackAnimListener
{
    [System.Serializable]
    public class AttackPoint
    {
        public float radius;

        // When the weapon game object moves or rotates, its own local coordinate system moves/rotates too so the weapon is always at (0,0,0).
        //The offset tracked here is according to the local coordinate grid of the weapon.
        public Vector3 offset;

        public Transform rootTransform;
    }

    public Animator Animator { get { return animator; } }
    public AttackPoint[] attackPoints = new AttackPoint[0];
    public LayerMask targetLayers;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float speedModifier = 2; // make this < 1 if you want enemy slower or > 1 if you want enemy faster
    private int damage = 10;
    private bool isAttacking = false;
    private Vector3[] originalAttackPositions;
    private RaycastHit[] rayCastHitCache = new RaycastHit[32];

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }// Awake


    private void FixedUpdate()
    {
        if (isAttacking)
        {
            for (int i = 0; i < attackPoints.Length; i++)
            {
                // want to draw in the world space but offset is tracked according to local space so convert it
                AttackPoint ap = attackPoints[i];
                Vector3 worldPos = ap.rootTransform.position + ap.rootTransform.TransformVector(ap.offset);
                Vector3 attackVector = (worldPos - originalAttackPositions[0]).normalized;

                Ray ray = new Ray(worldPos, attackVector); // vector starting from worldPos in the direction of attackVector
                Debug.DrawRay(worldPos, attackVector, Color.red, 4.0f);

                // cast sphere along the direction of the ray
                // contacts > 0 means that the weapon made contact with a collider through one of the rays
                // ~0 means enable all of the layers (~ is negation)
                int contacts = Physics.SphereCastNonAlloc(
                    ray,
                    ap.radius,
                    rayCastHitCache,
                    attackVector.magnitude,
                    ~0,
                    QueryTriggerInteraction.Ignore
                );

                for (int k = 0; k < contacts; k++)
                {
                    Collider collider = rayCastHitCache[k].collider;
                    if (collider != null)
                    {
                        // many game objects have colliders, but not all of them are damageable
                        // check if object can take damage
                        CheckDamage(collider, ap);
                    }
                }

                originalAttackPositions[0] = worldPos; // in next update, pos will be previous pos
            }
        }
    }// FixedUpdate


    private void CheckDamage(Collider other, AttackPoint ap)
    {

        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
        {
            return; // not hitting correct layer
        }

        Damageable damageable = other.GetComponent<Damageable>();

        if (damageable != null)
        {
            Damageable.DamageMessage data;
            data.amount = damage;
            data.damager = this;
            damageable.ApplyDamage(data);
        }
    }// CheckDamage


    public void MeleeAttackStart()
    {
        isAttacking = true;
        originalAttackPositions = new Vector3[attackPoints.Length];

        for (int i = 0; i < attackPoints.Length; i++)
        {
            AttackPoint ap = attackPoints[i];
            originalAttackPositions[i] = ap.rootTransform.position + ap.rootTransform.TransformDirection(ap.offset);
        }
    }// MeleeAttackStart


    public void MeleeAttackEnd()
    {
        isAttacking = false;
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


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (AttackPoint attackPoint in attackPoints)
        {
            if (attackPoint.rootTransform != null)
            {
                Vector3 worldPosition = attackPoint.rootTransform.TransformVector(attackPoint.offset);
                Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                Gizmos.DrawSphere(attackPoint.rootTransform.position + worldPosition, attackPoint.radius);
            }
        }
    }
#endif
}
