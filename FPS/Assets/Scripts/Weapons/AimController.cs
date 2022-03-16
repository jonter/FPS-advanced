using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    PlayerMovement player;
    float currentWidth;
    float minWidth;// менять при переключении оружия

    float moveWidth, shootWidth, speedWidth;
    RectTransform aimWindow;

    [SerializeField] float maxMoveWidth = 120, maxShootWidth = 80;
    [SerializeField] float moveSensivity = 400;
    [SerializeField] float shootSensivity = 1000;
    float widthCrouch, widthWalk, widthSprint, widthJump;
    float shootDuration = 0.2f;// менять при переключении оружия

    Coroutine spreadCoroutine;

    public float GetSpread()
    {
        if (currentWidth <= widthCrouch + minWidth) return 0;
        float maxWidth = maxMoveWidth + maxShootWidth + minWidth + speedWidth;
        float pecentage = currentWidth / maxWidth;
        return pecentage * 0.35f;
    }

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
        PlayerState state = player.GetPlayerState();
        bool isGrounded = player.GetGroundState();
        SetMoveWidth(state, isGrounded);
        SetSpeedWidth(player.GetSpeed());

        SetAimWindow();
        
    }

    void SetSpeedWidth(float playerSpeed)
    {
        float desiredWidth = 0;
        if (playerSpeed > 0.5f) desiredWidth = 30;

        if (speedWidth < desiredWidth)
        {
            speedWidth += moveSensivity * Time.deltaTime/2;
            if (speedWidth > desiredWidth) speedWidth = desiredWidth;
        }
        else if (speedWidth > desiredWidth)
        {
            speedWidth -= moveSensivity * Time.deltaTime/2;
            if (speedWidth < desiredWidth) speedWidth = desiredWidth;
        }
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
        currentWidth = minWidth + moveWidth + shootWidth + speedWidth;
        aimWindow.sizeDelta = new Vector2(currentWidth, currentWidth);
    }
}
