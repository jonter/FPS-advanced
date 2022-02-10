﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    ON_GROUND,
    IN_AIR,
    IN_SPRINT,
    IN_SLIDE,
    IN_CROUCH
}

public class PlayerMovement : MonoBehaviour
{
    PlayerState state = PlayerState.ON_GROUND;

    Vector3 velocity;
    float velocityY;

    [SerializeField] float normalSpeed = 8;
    [SerializeField] float sprintSpeed = 12;
    [SerializeField] float playerJump = 20;
    [SerializeField] float flySpeedControl = 4;
    // то насколько быстро изменяется currentSpeed от normalSpeed до sprintSpeed
    [SerializeField] float accelerationSpeed = 4; 
   
    float currentSpeed;
    CharacterController controller;
    Camera playerCamera;
    StaminaController staminaController;

    float gravity;

    bool isGrounded = false;

    [SerializeField] GameObject legs;
    [SerializeField] LayerMask groundMask;


    public PlayerState GetPlayerState()
    {
        return state;
    }

    // Start is called before the first frame update
    void Start()
    {
        staminaController = FindObjectOfType<StaminaController>();
        gravity = Physics.gravity.y;
        currentSpeed = normalSpeed;
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove();
        VerticalMovement();
        SprintControl();
        SpeedChangeControl();

    }


    void SpeedChangeControl()
    {
        float change = accelerationSpeed * Time.deltaTime;
        if (state == PlayerState.IN_SPRINT) currentSpeed += change;
        else if (state == PlayerState.ON_GROUND) currentSpeed -= change;

        currentSpeed = Mathf.Clamp(currentSpeed, normalSpeed, sprintSpeed);
    }

    void SprintControl()
    {
        Vector3 cameraDir = playerCamera.transform.forward;
        cameraDir = new Vector3(cameraDir.x, 0, cameraDir.z).normalized;
        Vector3 dirVelocity = velocity.normalized;
        float angle = Vector3.Angle(cameraDir, dirVelocity);

        if (Input.GetKey(KeyCode.LeftShift) && state == PlayerState.ON_GROUND)
        {
            state = PlayerState.IN_SPRINT;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && state == PlayerState.IN_SPRINT)
        {
            state = PlayerState.ON_GROUND;
        }
        if (angle > 50 && state == PlayerState.IN_SPRINT) state = PlayerState.ON_GROUND;
        if (staminaController.currentStam <= 0 && state == PlayerState.IN_SPRINT)
            state = PlayerState.ON_GROUND;
    }

    void HorizontalMove()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        Vector3 dir = transform.right * inputX + transform.forward * inputZ;
        // сделать проверку на слайдинг
        if (state == PlayerState.ON_GROUND || state == PlayerState.IN_SPRINT)
        {
            velocity = dir * currentSpeed;
            if (velocity.magnitude > currentSpeed) velocity = velocity.normalized * currentSpeed;
        }
        else if (state == PlayerState.IN_AIR)
            velocity += dir * Time.deltaTime * flySpeedControl;

        controller.Move(velocity * Time.deltaTime);
    }

    void VerticalMovement()
    {
        isGrounded = Physics.CheckSphere(legs.transform.position, 0.2f, groundMask);
        velocityY = velocityY + gravity * Time.deltaTime;

        if (!isGrounded) state = PlayerState.IN_AIR;
        else if (state != PlayerState.IN_SPRINT && state != PlayerState.IN_SLIDE) 
            state = PlayerState.ON_GROUND;

        if (isGrounded == true && velocityY < 0)
        {
            velocityY = -2f;

            if (Input.GetKeyDown(KeyCode.Space) && staminaController.currentStam > 0)
            {
                velocityY = playerJump;
                staminaController.SpendStamina(20);
            }
        }

        Vector3 velocityDown = new Vector3(0, velocityY, 0);
        controller.Move(velocityDown * Time.deltaTime);
    }
}