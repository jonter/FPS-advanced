using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pistol : Weapon
{

    private void Start()
    {
        ammoAllMax = 300;
        ammoAll = 100;
        maxAmmoInMagazine = 8;
        currentAmmo = maxAmmoInMagazine;
        StartCoroutine(PullWeapon());
    }

    private void Update()
    {
        if (!isActive) return;
        if (isInAction) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PrepareShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        UpdateUITexts();
    }

    void PrepareShoot()
    {
        if(currentAmmo > 0)
        {
            currentAmmo--;
            Shoot();
            StartCoroutine(SetAction(1/fireRate));
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
        StartCoroutine(SetAction(reloadTime));
        // это время когда обойма в анимации залетает в оружие
        yield return new WaitForSeconds(45f/60f);
        ReloadMagazine();
    }

    protected override void HideWeaponAnim()
    {
        anim.SetTrigger("hide");
    }

    protected override void PullWeaponAnim()
    {
        anim.SetTrigger("show");
    }

    override protected void Shoot()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("fire");

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        fireVFX.Play();
        RaycastHit hit;
        Vector3 startPos = transform.parent.position;
        Vector3 direction = transform.parent.forward;
        bool isHit = Physics.Raycast(startPos, direction, out hit, 100);

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
