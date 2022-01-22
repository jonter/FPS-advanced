using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject zombiePrefab;


    
    public IEnumerator SpawnZombie()
    {
        Vector3 spawnPos = transform.GetChild(0).position;
        GameObject clone = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        clone.GetComponent<EnemyAI>().SetTarget();

        yield return new WaitForSeconds(5);
        StartCoroutine(SpawnZombie());
    }

    

    
}
