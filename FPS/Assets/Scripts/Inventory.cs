using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    int currentWeapon = 0;

    public bool isSemiAutoEnabled = false;
    public bool isShotgunEnabled = false;

    public int pistolAmmo = 60;
    public int semiautoAmmo = 0;
    public int shotgunAmmo = 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentWeapon == 0) return;
            SwitchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentWeapon == 1) return;
            if (isSemiAutoEnabled == false) return;
            SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentWeapon == 2) return ;
            if (isShotgunEnabled == false) return;
            SwitchWeapon(2);
        }


    }

    public void SwitchWeapon(int newWeapon)
    {
        if(currentWeapon == 0)
        {
            Pistol pistol = transform.GetComponentInChildren<Pistol>();
            if (pistol.isInAction == true) return;
            pistol.gameObject.SetActive(false);
        }
        if (currentWeapon == 1)
        {
            SemiAuto auto = transform.GetComponentInChildren<SemiAuto>();
            if (auto.isInAction == true) return;
            auto.gameObject.SetActive(false);
        }
        if (currentWeapon == 2)
        {
            Shotgun shotgun = transform.GetComponentInChildren<Shotgun>();
            if (shotgun.isInAction == true) return;
            shotgun.gameObject.SetActive(false);
        }



        transform.GetChild(newWeapon).gameObject.SetActive(true);
        currentWeapon = newWeapon;
    }

}
