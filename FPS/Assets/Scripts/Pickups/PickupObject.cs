using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupObject : MonoBehaviour
{
    public string pickupName = "ABOBA";
    public abstract void PickUp();

}
