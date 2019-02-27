using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpectreAI : MonoBehaviour
{
    // The NavMeshAgent for use with the Game's NavMesh
    NavMeshAgent agent;
    // The variables related to the aggro system
    GameObject aggroTarget;
    float aggroCheckTime = 4.0f;
    float aggroCheckTimer = 0.0f;
    // The variables related to animating the spectre's head
    public Animator spectreHeadAnimator;
    Vector3 oldPosition = Vector3.zero;
    // The starting position to return to if it has nothing to attack
    Vector3 startPosition;

    [Space(10)]
    // The variables related to the scythe attack
    // - The Scythe has a Projectile component where you set the lifespan and movement vector
    // - The Scythe has an EnemyWeapon component where you set the damage and knockback
    public GameObject scythePrefab;
    public float scytheAttackCooldown = 2.0f;
    float scytheAttackTimer = 0.0f;
    [Space(5)]
    public float scytheLifespan = 5.0f;
    public float scytheSpeed = 3.0f;
    [Space(5)]
    public float scytheAttackDamage = 1.0f;
    public bool scytheHasKnockback = false;
    public float scytheKnockbackValue = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Acquires the NavMeshAgent component and tells it to stay still.
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(transform.position);

        // Initializes the aggro target as null
        aggroTarget = null;
        // Initialize the starting position
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(aggroTarget != null)
        {
            PursueAggroTarget();
        } else
        {
            aggroCheckTimer += Time.deltaTime;
            if(aggroCheckTimer > aggroCheckTime)
            {
                AggroCheck();
                aggroCheckTimer -= aggroCheckTime;
            }
        }
    }

    void AggroCheck()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 10);

        foreach(Collider c in nearbyObjects)
        {
            // If the sphere caught a player
            if(c.tag == "Player")
            {
                // If the player is alive
                if (c.gameObject.GetComponent<PlayerController>().GetPlayerState() != PlayerState.Dead)
                {
                    // Go after the player
                    BeginPursuit(c.gameObject);
                }
            }
        }
    }

    void PursueAggroTarget()
    {
        if (Vector3.Distance(transform.position, aggroTarget.transform.position) < 4.0f)
        {
            agent.destination = transform.position;
            Vector3 directionVector = aggroTarget.transform.position - transform.position;
            directionVector.Normalize();
            spectreHeadAnimator.SetFloat("Horizontal Speed", directionVector.x);
            spectreHeadAnimator.SetFloat("Vertical Speed", directionVector.z);

            scytheAttackTimer += Time.deltaTime;
            if(scytheAttackTimer > scytheAttackCooldown){
                if (aggroTarget.GetComponent<PlayerController>().GetPlayerState() == PlayerState.Dead)
                {
                    EndPursuit();
                }
                else
                {
                    scytheAttackTimer -= scytheAttackCooldown;

                    GameObject newScythe = (GameObject)Instantiate(scythePrefab);
                    newScythe.transform.position = transform.position;
                    Vector3 attackVector = aggroTarget.transform.position - transform.position;
                    attackVector.Normalize();
                    newScythe.GetComponent<Projectile>().InitializeProjectile(scytheLifespan, directionVector, scytheSpeed);
                    newScythe.GetComponent<EnemyWeapon>().InitializeEnemyWeapon(scytheAttackDamage, scytheHasKnockback, scytheKnockbackValue);
                }
            }
        }
        else
        {
            agent.destination = aggroTarget.transform.position;

            Vector3 directionVector = transform.position - oldPosition;
            directionVector.Normalize();
            spectreHeadAnimator.SetFloat("Horizontal Speed", directionVector.x);
            spectreHeadAnimator.SetFloat("Vertical Speed", directionVector.z);

            oldPosition = transform.position;
            
        }

        // With the timer counting here, the countdown is guaranteed whether or not the spectre is within attacking range.
        if (scytheAttackTimer < scytheAttackCooldown) scytheAttackTimer += Time.deltaTime;
    }

    public void BeginPursuit(GameObject target)
    {
        aggroTarget = target;

        oldPosition = transform.position;
    }

    public void EndPursuit()
    {
        aggroTarget = null;

        Vector3 directionVector = startPosition - transform.position;
        directionVector.Normalize();
        spectreHeadAnimator.SetFloat("Horizontal Speed", directionVector.x);
        spectreHeadAnimator.SetFloat("Vertical Speed", directionVector.z);

        agent.destination = startPosition;
    }
}
