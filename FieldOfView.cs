using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public List<Transform> visibleTargets = new List<Transform>();  //list of visible targets

    Transform player;               // Reference to the player's position.
    PlayerHealth playerHealth;      // Reference to the player's health.
    //EnemyHealth enemyHealth;        // Reference to this enemy's health.
    public NavMeshAgent nav;               // Reference to the nav mesh agent.
    Animator anim;                      // Reference to the animator component.

    public float playerAlpha;
    public bool playerInRange = false;

    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }




    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .1f);

        //StartCoroutine("Wander", 5f);
    }

    //coroutine to make detecting targets not instantaneous
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    //coroutine to make wandering not instant - gets rid of character shaking constantly
  /*  IEnumerator Wander(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            NewDirection();
        }
    }*/




    //find targets in view radius
    void FindVisibleTargets()
    {
        visibleTargets.Clear();  //reset target list

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius);
     //   targetsInViewRadius = ;  //get array of all colliders in view radius

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            if(targetsInViewRadius[i].tag == "Player")
            {
                //Get the location of the target
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = target.position - transform.position;

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    //get the distance to the target
                    float dstToTarget = Vector3.Distance(transform.position, target.position);

                    //if not blocked, add to target list
                    visibleTargets.Add(target);
                }
            }
        }
    }

    //called every frame - currently controls actor movement
    void FixedUpdate()
    {
        nav.enabled = true;
        playerAlpha = player.GetComponentInChildren<SkinnedMeshRenderer>().material.color.a;

        if (playerAlpha == 0)
        {
            nav.SetDestination(transform.position);
            anim.SetBool("IsWalking", false);
        }

        //originally used array of colliders, but it would try and move towards trees that are blocked
        //Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        Transform closest;
        Vector3 tgt;

        //check if we have any targets
        if (visibleTargets.Count > 0)
        {
            //get the first target i.e. the closest
            closest = visibleTargets[0];

            //for each target...
            foreach (Transform obj in visibleTargets)
            {
                if (obj == null)
                {
                    //small check incase target is destroyed before character reaches it
                    return;
                }

                transform.LookAt(obj.transform.position, transform.up);

            }

            if (Vector3.Distance(player.position, this.transform.position) < 1.0f)
            {
                anim.SetBool("IsAttacking", true);

                // Stop following player.
                nav.SetDestination(this.transform.position);

                anim.SetBool("IsWalking", false);
            }
            else
            {
                tgt = closest.transform.position;
                anim.SetBool("IsAttacking", false);

                // ... set the destination of the nav mesh agent to the player.
                nav.SetDestination(tgt);

                anim.SetBool("IsWalking", true);
            }

            if(playerInRange == true)
            {
                Attack();
            }

        
        }
        else if (visibleTargets.Count == 0)
        {
            nav.enabled = false;
            anim.SetBool("IsWalking", false);
            //if no targets, wander forever
            //transform.position = Vector3.MoveTowards(transform.position, randomWayPoint, speed);
        }

        if(playerHealth.currentHealth <= 0)
        {
            anim.SetBool("IsAttacking", false);
        }
    }

    


    void OnTriggerEnter(Collider coll)
    {
        //if found player
        if (coll.gameObject.tag == "Player")
        {
            //attack
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void Attack()
    {
        // If the player has health to lose...
        if (playerHealth.currentHealth >= 0)
        {
            // ... damage the player.
            playerHealth.TakeDamage(5);
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        //Generates the field of view angles for each spawned actor - can only be seen in scene view
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
