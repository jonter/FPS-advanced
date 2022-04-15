using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] float damageMult = 1;

    public void GetDamage(float damage)
    {
        GetComponentInParent<EnemyHealth>().GetDamage(damage * damageMult);
    }

    
}
