using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    GameObject target;
    [SerializeField] float rageRadius = 20f;

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
        EnemyHealth enemy = GetComponent<EnemyHealth>();
        if(enemy.isAlive == false)
        {
            agent.SetDestination(transform.position);
            return;
        }


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
        else
        {
            anim.SetBool("isWalk", false);
        }
    }

    void AttackTarget()
    {
        if (distance <= 2.6f)
        {
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
    }

    void CheckDistance()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= rageRadius)
        {
            isSeen = true;
        }
    }

    public void Hit()
    {
        target.GetComponent<PlayerHealth>().GetDamage(enemyDamage);
        print("Я тебя ударил!");
    }

    public void SetTarget()
    {
        isSeen = true;
    }




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rageRadius);
    }

}
