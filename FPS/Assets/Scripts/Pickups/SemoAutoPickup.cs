using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemoAutoPickup : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        Inventory inv = FindObjectOfType<Inventory>();
        inv.isSemiAutoEnabled = true;
        inv.SwitchWeapon(1);

        Destroy(gameObject);
    }


}
