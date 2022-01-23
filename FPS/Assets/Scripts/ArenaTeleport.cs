using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaTeleport : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.GetComponent<PlayerHealth>())
        {
            SceneManager.LoadScene(1);
        }


    }

}
