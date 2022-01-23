using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : MonoBehaviour
{
    [SerializeField] ParticleSystem fireVFX;

    [SerializeField] Text ammoInStockText;
    [SerializeField] Text ammoAllText;

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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isInAction == true) return;

            if (inv.shotgunAmmo >= 1)
            {
                inv.shotgunAmmo--;
                Shoot();
                StartCoroutine(BeginAction(1));
            }
            else
            {
                print("Перезаряди оружие");
            }
        }


        ammoAllText.text = "(" + inv.shotgunAmmo + ")";
        ammoInStockText.text = "Shotgun";
    }


    IEnumerator BeginAction(float delayTime)
    {
        isInAction = true;
        yield return new WaitForSeconds(delayTime);
        isInAction = false;
    }


    void Shoot()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("reload");

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
