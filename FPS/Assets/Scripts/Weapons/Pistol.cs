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

    protected override void ShowWeaponAnim()
    {
        weaponTitleText.ShowTitle("Pistol");
        anim.SetTrigger("show");
    }

    override protected void Shoot()
    {
        base.Shoot();
        DisplayShoot();

        RaycastHit hit;
        Vector3 startPos = transform.parent.position;
        float randomSpread = aimSpread.GetSpread();
        float randomY = Random.Range(-randomSpread, randomSpread);
        float randomX = Random.Range(-randomSpread, randomSpread);
        
        Vector3 randomVec = transform.parent.up * randomY
            + transform.parent.right * randomX;
        Vector3 direction = transform.parent.forward + randomVec;
        bool isHit = Physics.Raycast(startPos, direction, out hit, 100);

        aimSpread.MakeShoot();
        if (isHit) ProcessHit(hit);
    }

    private void ProcessHit(RaycastHit hit)
    {
        DisplayBulletImpact(hit.point, hit.normal, hit.transform.gameObject.layer);
        EnemyHitbox enemy = hit.transform.GetComponent<EnemyHitbox>();
        enemy?.GetDamage(damage);
    }

    void DisplayShoot()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("fire");
        StartCoroutine(camera.Coil());

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        fireVFX.Play();
        StartCoroutine(ShowLight());
    }

    IEnumerator ShowLight()
    {
        fireVFX.GetComponentInChildren<Light>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        fireVFX.GetComponentInChildren<Light>().enabled = false;
    }


}
