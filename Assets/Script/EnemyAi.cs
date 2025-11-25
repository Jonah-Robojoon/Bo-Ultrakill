using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public GameObject projectile;


    [Header("Ranges")]
    public float sightRange = 15f;
   

  
    

    [Header("Close-Range Movement")]
    public float closeMoveSpeed = 1.5f;      // speed of the fwd/back motion
    public float closeMoveAmount = 0.5f;     // how far it moves forward/back


    private bool playerInSightRange = false;
    private bool playerInAttackRange = false;

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (agent == null) agent = GetComponent<NavMeshAgent>();

  
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        playerInSightRange = dist <= sightRange;
      

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
      
        else
        {
            if (agent.hasPath) agent.ResetPath();
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);


    }
}