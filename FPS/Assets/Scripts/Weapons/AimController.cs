using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    PlayerMovement player;
    float currentWidth;
    float minWidth;// менять при переключении оружия

    float moveWidth, shootWidth;
    RectTransform aimWindow;

    [SerializeField] float maxMoveWidth = 120, maxShootWidth = 80;
    [SerializeField] float moveSensivity = 400;
    [SerializeField] float shootSensivity = 1000;
    float widthCrouch, widthWalk, widthSprint, widthJump;
    float shootDuration = 0.2f;// менять при переключении оружия
    

    Coroutine spreadCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        widthCrouch = 0.05f * maxMoveWidth;
        widthWalk = 0.3f * maxMoveWidth;
        widthSprint = 0.8f * maxMoveWidth;
        widthJump = maxMoveWidth;

        player = FindObjectOfType<PlayerMovement>();
        aimWindow = GetComponent<RectTransform>();
        minWidth = aimWindow.sizeDelta.x;
    }

    public void MakeShoot()
    {
        if(spreadCoroutine == null)
            spreadCoroutine = StartCoroutine(ShootSpread());
        else
        {
            StopCoroutine(spreadCoroutine);
            spreadCoroutine = StartCoroutine(ShootSpread());
        }
    }

    IEnumerator ShootSpread()
    {
        float timer = 0;
        while(timer < shootDuration / 2)
        {
            timer += Time.deltaTime;
            shootWidth += shootSensivity * Time.deltaTime;
            if (shootWidth > maxShootWidth) shootWidth = maxShootWidth;
            yield return null;
        }
        while (shootWidth > 0)
        {
            shootWidth -= shootSensivity * Time.deltaTime;
            yield return null;
        }
        shootWidth = 0;

        spreadCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MakeShoot();
        }
        PlayerState state = player.GetPlayerState();
        bool isGrounded = player.GetGroundState();
        SetMoveWidth(state, isGrounded);

        SetAimWindow();
    }

    void SetMoveWidth(PlayerState state, bool isGrounded)
    {
        float desiredWidth = 0;
        if (!isGrounded) desiredWidth = widthJump;
        else if (state == PlayerState.SPRINT || state == PlayerState.SLIDE)
            desiredWidth = widthSprint;
        else if (state == PlayerState.WALK) desiredWidth = widthWalk;
        else if (state == PlayerState.CROUCH) desiredWidth = widthCrouch;

        if (moveWidth < desiredWidth)
        {
            moveWidth += moveSensivity * Time.deltaTime;
            if (moveWidth > desiredWidth) moveWidth = desiredWidth;
        }
        else if(moveWidth > desiredWidth)
        {
            moveWidth -= moveSensivity * Time.deltaTime;
            if (moveWidth < desiredWidth) moveWidth = desiredWidth;
        }
    }

    void SetAimWindow()
    {
        currentWidth = minWidth + moveWidth + shootWidth;
        aimWindow.sizeDelta = new Vector2(currentWidth, currentWidth);
    }
}
