using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int ammoType = 0;
    [SerializeField] int ammoCount = 20;

    private void OnTriggerEnter(Collider other)
    {
        Inventory inv = FindObjectOfType<Inventory>();

        if(ammoType == 0)
        {
            //inv.pistolAmmo = inv.pistolAmmo + ammoCount;
            inv.pistolAmmo += ammoCount;
        }
        else if (ammoType == 1)
        {
            inv.semiautoAmmo += ammoCount;
        }
        else if(ammoType == 2)
        {
            inv.shotgunAmmo += ammoCount;
        }

        Destroy(gameObject);

    }
}
