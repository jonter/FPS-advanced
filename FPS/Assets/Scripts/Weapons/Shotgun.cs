using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : MonoBehaviour
{

    void Shoot()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("reload");

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        //fireVFX.Play();

        for(int i = 0; i < 12; i++)
        {
            CreateRaycast();
        }


    }

    void CreateRaycast()
    {
        RaycastHit hit;
        Vector3 startPos = transform.parent.position;

        float randomX = Random.Range(-0.2f, 0.2f);
        float randomY = Random.Range(-0.2f, 0.2f);
        float randomZ = Random.Range(-0.2f, 0.2f);

        Vector3 randomVector = new Vector3(randomX, randomY, randomZ);
        Vector3 direction = transform.parent.forward + randomVector;

        bool isHit = Physics.Raycast(startPos, direction, out hit, 100);

        Debug.DrawRay(startPos, direction * 100, Color.red, 5);

        if (isHit)
        {
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

            if (enemy)
            {
                enemy.GetDamage(10);
            }
        }
    }

}
