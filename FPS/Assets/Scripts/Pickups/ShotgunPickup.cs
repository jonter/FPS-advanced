using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Inventory inv = FindObjectOfType<Inventory>();
        inv.isShotgunEnabled = true;
        inv.SwitchWeapon(2);

        Destroy(gameObject);
    }
}
