using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 200;
    [SerializeField] Slider healthbar;

    [SerializeField] GameObject splatterImage;

    public event Action onDeath;

    private void Update()
    {
        healthbar.value = health;
    }


    public void GetDamage(float damage)
    {
        health = health - damage;

        if (health <= 0)
        {
            if(onDeath != null) onDeath();
        }
        else
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(DisplaySplatter());
        }

    }

    IEnumerator DisplaySplatter()
    {
        splatterImage.SetActive(true);
        float randomZ = UnityEngine.Random.Range(0, 360);
        splatterImage.transform.rotation = Quaternion.Euler(0, 0, randomZ);
        yield return new WaitForSeconds(0.25f);
        splatterImage.SetActive(false);
    }


    public void RestoreHealth(float extraHealth)
    {
        health = health + extraHealth;

        if(health > 200)
        {
            health = 200;
        }
    }

}
