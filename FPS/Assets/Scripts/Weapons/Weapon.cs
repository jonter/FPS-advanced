using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public abstract class Weapon : MonoBehaviour
{

    protected float reloadTime = 1;
    [SerializeField] protected float fireRate = 6;
    [SerializeField] protected float damage = 10;
    [SerializeField] protected ParticleSystem fireVFX;

    protected int maxAmmoInMagazine;
    protected int ammoAll;
    protected int ammoAllMax;

    protected int currentAmmo;

    [SerializeField] protected ParticleSystem hitEffectGround;
    [SerializeField] protected ParticleSystem hitEffectEnemy;

    [SerializeField] TMP_Text magazineText;
    [SerializeField] TMP_Text allAmmoText;

    [SerializeField] protected GameObject weaponInner;
    protected Animator anim;

    protected CameraEffects camera;

    protected bool isInAction = false;
    protected bool isActive = false;

    public IEnumerator PullWeapon() // вызывается из скрипта переключения оружия
    {
        camera = GetComponentInParent<CameraEffects>();
        anim = GetComponent<Animator>();
        isInAction = true;
        weaponInner.SetActive(true);
        PullWeaponAnim();
        yield return new WaitForSeconds(0.5f);
        isInAction = false;
        isActive = true;
    }

    public IEnumerator HideWeapon()
    {
        HideWeaponAnim();
        isInAction = true;
        yield return new WaitForSeconds(0.5f);
        isActive = false;
        weaponInner.SetActive(false);
    }


    protected abstract void PullWeaponAnim();
    protected abstract void HideWeaponAnim();

    protected void UpdateUITexts()
    {
        allAmmoText.text = $"({ammoAll})";
        magazineText.text = $"{currentAmmo}/{maxAmmoInMagazine}";
    }

    protected IEnumerator SetAction(float duration)
    {
        isInAction = true;
        yield return new WaitForSeconds(duration);
        isInAction = false;
    }

    protected abstract void Shoot();

    protected void ReloadMagazine()
    {
        int addBullets = maxAmmoInMagazine - currentAmmo;

        if (addBullets >= ammoAll)
        {
            currentAmmo = currentAmmo + ammoAll;
            ammoAll = 0;
        }
        else
        {
            currentAmmo = maxAmmoInMagazine;
            ammoAll = ammoAll - addBullets;
        }
    }

    
}
