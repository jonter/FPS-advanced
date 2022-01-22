using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    bool isStated = false;

    [SerializeField] GameObject skullButton;

    private void OnTriggerEnter(Collider other)
    {
        if (isStated == true) return;

        skullButton.GetComponent<Animator>().SetTrigger("isPressed");

        isStated = true;
        EnemySpawner[] enemySpawners = FindObjectsOfType<EnemySpawner>();

        for (int i = 0; i < enemySpawners.Length; i++)
        {
            StartCoroutine(enemySpawners[i].SpawnZombie());
        }

    }

}
