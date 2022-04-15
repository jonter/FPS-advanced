using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponType
{
    Pistol,
    SemiAuto,
    Shotgun
}

public class WeaponPickup : PickupObject
{
    [SerializeField] WeaponType wType;

    public override void PickUp()
    {
        FindObjectOfType<WeaponController>().UnlockWeapon(wType);

        Destroy(gameObject);
    }
}
