using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    Weapon currentWeapon;
    int weaponIndex = 0;

    List<Weapon> weapons;
    bool isBusy = false;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = GetComponentInChildren<Pistol>();
        weapons = new List<Weapon>();
        weapons.Add(currentWeapon);
        StartCoroutine(currentWeapon.PullWeapon());
        
    }


    void Update()
    {
        
        if (isBusy) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(SwitchWeapon(0));
        else if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(SwitchWeapon(1));
        else if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(SwitchWeapon(2));
        else
            CheckMouseScrollWheel();

    }

    void CheckMouseScrollWheel()
    {
        float mouseMovement = Input.GetAxis("Mouse ScrollWheel");
        int newIndex = weaponIndex;
        if (mouseMovement > 0.0001f) newIndex++;
        if (mouseMovement < -0.0001f) newIndex--;

        if (newIndex > weapons.Count - 1) newIndex = 0;
        if (newIndex < 0) newIndex = weapons.Count - 1;

        StartCoroutine(SwitchWeapon(newIndex));
    }

    IEnumerator SwitchWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex == weaponIndex) yield break;
        if (newWeaponIndex < 0) yield break;
        if (newWeaponIndex > weapons.Count - 1) yield break;
        print("Switched");
        isBusy = true;
        yield return StartCoroutine(currentWeapon.HideWeapon());
        currentWeapon = weapons[newWeaponIndex];
        weaponIndex = newWeaponIndex;
        yield return StartCoroutine(currentWeapon.PullWeapon());
        isBusy = false;
    }


    public void UnlockWeapon(WeaponType wType)
    {
        if (wType == WeaponType.SemiAuto)
            weapons.Add(GetComponentInChildren<SemiAuto>());
        else if (wType == WeaponType.Shotgun)
            weapons.Add(GetComponentInChildren<Shotgun>());

        StartCoroutine(SwitchWeapon(weapons.Count - 1));
    }


}
