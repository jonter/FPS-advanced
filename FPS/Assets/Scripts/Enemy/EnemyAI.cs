using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    GameObject target;
    [SerializeField] float rageRadius = 20f;
    [SerializeField] float loseRadius = 40f;
    [SerializeField] float attackRadius = 2;

    [SerializeField] float enemyDamage = 20;

    NavMeshAgent agent;

    float distance;

    bool isSeen = false;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerHealth>().gameObject;
    }

    // Update is called once per frame
    void Update()
    { 

        CheckDistance();

        WalkForTarget();

        AttackTarget();
       
    }

    void WalkForTarget()
    {
        if (isSeen == true)
        {
            anim.SetBool("isWalk", true);
            agent.SetDestination(target.transform.position);
        }
        else if(agent.desiredVelocity.magnitude <= 0.1f)
        {
            anim.SetBool("isWalk", false);
        }
    }

    void AttackTarget()
    {
        if (distance <= attackRadius)
        {
            anim.SetBool("isAttack", true);
            LookAtPlayer();
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
    }

    void LookAtPlayer()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir = dir.normalized;
        dir = new Vector3(dir.x, 0, dir.z);
        Quaternion lookRotaion = Quaternion.LookRotation(dir, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotaion, 0.1f);
    }

    void CheckDistance()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= rageRadius) isSeen = true;
        else if (distance > loseRadius) isSeen = false;
    }

    public void Hit()
    {
        if (distance > attackRadius) return;
        target.GetComponent<PlayerHealth>().GetDamage(enemyDamage);
    }

    public void SetTarget()
    {
        isSeen = true;
    }




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, loseRadius);
    }

    private void OnDisable()
    {
        agent.SetDestination(transform.position);
    }

}
