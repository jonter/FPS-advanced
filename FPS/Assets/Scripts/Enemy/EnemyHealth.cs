using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] float hp = 100;
    public bool isAlive = true;
    
    public void GetDamage(float damage)
    {
        if(isAlive == false)
        {
            return;
        }
        hp = hp - damage;

        GetComponent<EnemyAI>().SetTarget();

        if (hp <= 0)
        {
            isAlive = false;
            Death();
        }

    }

    void Death()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("die");
        Collider capsule = GetComponent<Collider>();
        capsule.isTrigger = true;

        Destroy(gameObject, 5);
    }


}
