using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SemiAuto : MonoBehaviour
{
    [SerializeField] ParticleSystem fireVFX;

    [SerializeField] int ammoInStock = 30;

    [SerializeField] Text ammoInStockText;
    [SerializeField] Text ammoAllText;

    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] GameObject hitEnemyEffectPrefab;

    public bool isInAction = false;

    Inventory inv;

    // Start is called before the first frame update
    void Start()
    {
        inv = FindObjectOfType<Inventory>();
    }



    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (isInAction == true) return;

            if (ammoInStock >= 1)
            {
                ammoInStock--;
                Shoot();
                StartCoroutine(BeginAction(0.1f));
            }
            else
            {
                print("Перезаряди оружие");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isInAction == true) return;
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("reload");
            StartCoroutine(BeginAction(2));
        }

        ammoAllText.text = "(" + inv.semiautoAmmo + ")";
        ammoInStockText.text = ammoInStock + "/30";
    }


    IEnumerator BeginAction(float delayTime)
    {
        isInAction = true;
        yield return new WaitForSeconds(delayTime);
        isInAction = false;
    }


    public void Reload()
    {
        //Отнять патроны из общего инвентаря
        int addBullets = 30 - ammoInStock;

        if (addBullets >= inv.semiautoAmmo)
        {
            ammoInStock = ammoInStock + inv.semiautoAmmo;
            inv.semiautoAmmo = 0;
        }
        else
        {
            ammoInStock = 30;
            inv.semiautoAmmo = inv.semiautoAmmo - addBullets;
        }

    }

    void Shoot()
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
                GameObject clone = Instantiate(hitEnemyEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));

                Destroy(clone, 0.3f);
            }
            else
            {
                GameObject clone = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(clone, 0.3f);
            }
        }


    }


}
