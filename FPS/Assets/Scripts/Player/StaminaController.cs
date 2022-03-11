using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    PlayerMovement player;
    public float currentStam { get; private set; }
    float maxStam = 100;

    [SerializeField] float stamDecreaseSpeed = 20;
    [SerializeField] float stamRestoreSpeed = 20;
    [SerializeField] float restoreDuration = 2;

    Slider displayStam;

    bool isTired = false;

    // Start is called before the first frame update
    void Start()
    {
        currentStam = maxStam;
        displayStam = GetComponent<Slider>();
        player = FindObjectOfType<PlayerMovement>();   
    }

    public void SpendStamina(float decrease)
    {
        currentStam -= decrease;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTired) return;
        PlayerState state = player.GetPlayerState();
        if(state == PlayerState.SPRINT)
            currentStam -= stamDecreaseSpeed * Time.deltaTime;
        else if(state == PlayerState.WALK || state == PlayerState.CROUCH)
            currentStam += stamRestoreSpeed * Time.deltaTime;

        if (currentStam >= maxStam) currentStam = maxStam;
        if (currentStam <= 0) StartCoroutine(LoseStamina());

        displayStam.value = currentStam / maxStam;
    }

    IEnumerator LoseStamina()
    {
        currentStam = 0;
        isTired = true;
        yield return new WaitForSeconds(restoreDuration);
        isTired = false;
        currentStam = 1;
    }

}
