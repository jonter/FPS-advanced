using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : Weapon
{
    bool isReloading = false;

    private void Start()
    {
        ammoAllMax = 100;
        ammoAll = 10;
        maxAmmoInMagazine = 8;
        currentAmmo = maxAmmoInMagazine;
        StartCoroutine(PullWeapon());
    }

    protected override void HideWeaponAnim()
    {
        anim.SetTrigger("hide");
    }

    protected override void ShowWeaponAnim()
    {
        anim.SetTrigger("show");
    }

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PrepareShoot();
        }

        if (isInAction) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        UpdateUITexts();
    }

    void PrepareShoot()
    {
        if (isReloading)
            StopReloading();

        if (isInAction) return;

        if (currentAmmo > 0)
        {
            
            Shoot();
            StartCoroutine(SetAction(1 / fireRate));
        }
        else
        {
            StartCoroutine(Reload());
        }

    }

    public IEnumerator Reload()
    {
        if (ammoAll <= 0) yield break;
        if (currentAmmo == maxAmmoInMagazine) yield break;
        anim.SetTrigger("reload");
        isInAction = true;
        yield return new WaitForSeconds(0.5f);
        isReloading = true;
        
    }

    void ReloadOneBullet()
    {
        currentAmmo++;
        ammoAll--;
        UpdateUITexts();

        if(ammoAll <= 0 || currentAmmo == maxAmmoInMagazine)
        {
            StopReloading();
        }
    }

    void StopReloading()
    {
        anim.SetTrigger("reload_end");
        isReloading = false;
        StartCoroutine(SetAction(0.5f));
    }


    protected override void Shoot()
    {
        base.Shoot();
        currentAmmo--;
        UpdateUITexts();
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("fire");

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        fireVFX.Play();

        for(int i = 0; i < 12; i++)
        {
            CreateRaycast();
        }


    }

    void CreateRaycast()
    {
        RaycastHit hit;
        Vector3 startPos = transform.parent.position;

        float randomX = Random.Range(-0.2f, 0.2f);
        float randomY = Random.Range(-0.2f, 0.2f);
        float randomZ = Random.Range(-0.2f, 0.2f);

        Vector3 randomVector = new Vector3(randomX, randomY, randomZ);
        Vector3 direction = transform.parent.forward + randomVector;

        bool isHit = Physics.Raycast(startPos, direction, out hit, 100);

        Debug.DrawRay(startPos, direction * 100, Color.red, 5);

        if (isHit)
        {
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

            if (enemy)
            {
                enemy.GetDamage(10);
            }
        }
    }

}
