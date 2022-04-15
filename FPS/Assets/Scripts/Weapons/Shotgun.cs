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
    }

    protected override void HideWeaponAnim()
    {
        anim.SetTrigger("hide");
    }

    protected override void ShowWeaponAnim()
    {
        weaponTitleText.ShowTitle("Shotgun");
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

        float randomSpread = aimSpread.GetSpread();
        float randomY = Random.Range(-randomSpread, randomSpread);
        float randomX = Random.Range(-randomSpread, randomSpread);

        Vector3 randomVec = transform.parent.up * randomY
            + transform.parent.right * randomX;
        aimSpread.MakeShoot();

        for (int i = 0; i < 12; i++)
        {
            CreateRaycast(randomVec);
        }
    }

    void CreateRaycast(Vector3 globalRandom)
    {
        RaycastHit hit;
        Vector3 startPos = transform.parent.position;

        float randomX = Random.Range(-0.2f, 0.2f);
        float randomY = Random.Range(-0.2f, 0.2f);
        float randomZ = Random.Range(-0.2f, 0.2f);

        Vector3 randomVector = new Vector3(randomX, randomY, randomZ);
        Vector3 direction = transform.parent.forward + randomVector + globalRandom;

        bool isHit = Physics.Raycast(startPos, direction, out hit, 100);

        if (isHit)
            ProcessHit(hit);
    }

    void ProcessHit(RaycastHit hit)
    {
        EnemyHitbox enemy = hit.transform.GetComponent<EnemyHitbox>();
        DisplayImpactShootgun(hit.point, hit.normal, hit.transform.gameObject.layer);
        if (enemy)
        {
            enemy.GetDamage(damage);
        }
    }

    void DisplayImpactShootgun(Vector3 point, Vector3 dir, LayerMask mask)
    {
        LayerMask enemyLayer = 10;
        ParticleSystem impactVFX;
        print($"enemy: {enemyLayer.value}, mask: {mask.value}");
        if (enemyLayer.value == mask.value) impactVFX = hitEffectEnemy;
        else impactVFX = hitEffectGround;

        impactVFX.transform.position = point;
        impactVFX.transform.LookAt(dir + point);
        impactVFX.Emit(5);
        ParticleSystem[] innerVFXs = impactVFX.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem vfx in innerVFXs)
        {
            vfx.Emit(10);
        }
    }

}
