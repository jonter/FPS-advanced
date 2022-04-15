using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PickupChecker : MonoBehaviour
{
    [SerializeField] TMP_Text pickupInfoText;
    [SerializeField] float distance = 2;

    PickupObject currentPickup = null;

    private void Start()
    {
        pickupInfoText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        CheckWithRaycast();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentPickup == null) return;
            currentPickup.PickUp();
        }
    }

    void CheckWithRaycast()
    {
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(origin, dir, out hitInfo, distance);
        if (isHit)
        {
            PickupObject pickup = hitInfo.transform.GetComponent<PickupObject>();
            if (pickup)
            {
                currentPickup = pickup;
                pickupInfoText.text = "Press E to pick up: " + pickup.pickupName;
            }
            else DisablePickup();
        }
        else DisablePickup();
    }

    void DisablePickup()
    {
        currentPickup = null;
        pickupInfoText.text = "";
    }
}
