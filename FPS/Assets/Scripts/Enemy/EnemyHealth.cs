using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] float hp = 100;
    public bool isAlive = true;
    
    public void GetDamage(float damage)
    {
        if(isAlive == false) return;

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
        capsule.enabled = false;
        GetComponent<EnemyAI>().enabled = false;
        
        StartCoroutine(FadeOut(0.5f));
    }

    IEnumerator FadeOut(float duration)
    {
        yield return new WaitForSeconds(2);
        SkinnedMeshRenderer[] skins = GetComponentsInChildren<SkinnedMeshRenderer>();
        float timer = 0;
        while(timer < 1)
        {
            timer += Time.deltaTime / duration;
            float opacity = Mathf.Lerp(2, -1, timer);
            for (int i = 0; i < skins.Length; i++)
                skins[i].material.SetFloat("_opacity", opacity);
            yield return null;
        }
        Destroy(gameObject);
    }


}
