using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject light;
    AudioSource audio;
    Animator anim;

    bool isInAction = false;

    bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isInAction == true) return;
            StartCoroutine(SwitchFlashlight());
        }
    }

    IEnumerator SwitchFlashlight()
    {
        isInAction = true;
        if(isOn == false)
        {
            anim.SetBool("isShown", true);
            yield return new WaitForSeconds(0.5f);
            light.SetActive(true);
            audio.Play();
            isOn = true;
        }
        else
        {
            light.SetActive(false);
            audio.Play();
            anim.SetBool("isShown", false);
            yield return new WaitForSeconds(0.5f);
            isOn = false;
        }
        isInAction = false;
    }

}
