using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public Animator anim;

    public float sightRange = 15f;
    public float attackRange = 2f;

    [SerializeField] private float _attackDash = 5f;

    private bool alreadyAttacked = false;
    public float attackCooldown = 1.5f;
    private bool _stopMovement = false;
    public bool _isDeing = false;
    private bool inAttack;
    private bool inSight;
    private float dist;

    [SerializeField] private Transform _rotatable;

    public static Action onPlayerHit;
    public static Action onPlayerHeal;

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponentInChildren<Animator>();

        agent.updateRotation = false; // manual rotation
        agent.stoppingDistance = attackRange * 0.9f; // avoid sliding close

    }
    private void Update()
    {
        dist = Vector3.Distance(transform.position, player.position);

        inSight = dist <= sightRange;


        inAttack = dist <= attackRange;
    }
    private void FixedUpdate()
    {

        if (_isDeing == true) 
        {
            HealPlayer();
            anim.SetBool("isDeing", true);
        }
        if (player == null) return;

        

        
        
        

        if (inAttack)
        {
            AttackPlayer();
        }
        else if (inSight)
        {
            ChasePlayer();
        }
        else
        {
            StopMoving();
        }
        RotateTowardsTarget();
        // Smooth rotation only when allowed & not inside attack range
        /*if (!_stopMovement && dist > attackRange)
        {
            RotateTowardsTarget();
        }
        */
        // animation speed matches movement (optional but fixes skating)
        //anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    void ChasePlayer()
    {
        anim.SetBool("isRunning", true);
        anim.SetBool("isBiting", false);

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void StopMoving()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("isBiting", false);

        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;  // prevents sliding
    }

    void AttackPlayer()
    {
        _stopMovement = true;
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;


        anim.SetBool("isRunning", false);

        if (!alreadyAttacked)
        {
            anim.SetBool("isBiting", true);
            DamagePlayerDuringAttack();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack()
    {
        _stopMovement = false;
        anim.SetBool("isBiting", false);
        alreadyAttacked = false;

        agent.isStopped = false; // allow movement again
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = player.position - transform.position;

        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion rotated = Quaternion.Euler(0f, targetRotation.eulerAngles.y + 180, 0f);

        _rotatable.rotation = Quaternion.Slerp(
            _rotatable.rotation,
            rotated,
            100f * Time.deltaTime  // rotation smoothness
        );
    }


    private void DamagePlayerDuringAttack()
    {
        float _Playerdistance = Vector3.Distance(transform.position, player.transform.position);
        
        if (_Playerdistance <= attackRange)
        {
            onPlayerHit?.Invoke();
        }
    }

    private void HealPlayer()
    {
        float _Playerdistance = Vector3.Distance(transform.position, player.transform.position);
        
        if (_Playerdistance <= attackRange)
        {
            onPlayerHeal?.Invoke();
        }
    }
}