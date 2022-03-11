using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    WALK,
    SPRINT,
    SLIDE,
    CROUCH
}

[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    PlayerState state = PlayerState.WALK;

    Vector3 velocity;
    float velocityY;

    [SerializeField] float crouchSpeed = 5;
    [SerializeField] float normalSpeed = 8;
    [SerializeField] float sprintSpeed = 12;
    [SerializeField] float playerJump = 20;
    [SerializeField] float flySpeedControl = 4;
    // то насколько быстро изменяется currentSpeed от normalSpeed до sprintSpeed
    [SerializeField] float accelerationSpeed = 4;
    [SerializeField] float crouchStateSpeed = 10; 
    [SerializeField] float slideSlowness = 2; 
   
    float currentSpeed;
    CharacterController controller;
    Camera playerCamera;
    StaminaController staminaController;

    float gravity;

    bool isGrounded = false;

    [SerializeField] GameObject legs;
    [SerializeField] LayerMask groundMask;

    float alpha = 0;

    public bool GetGroundState()
    {
        return isGrounded;
    }

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
        CrouchControl();
        SpeedChangeControl();
        SizeControl();

    }

    void SizeControl()
    {
        if (state == PlayerState.CROUCH || state == PlayerState.SLIDE)
            alpha += crouchStateSpeed * Time.deltaTime;
        else alpha -= crouchStateSpeed * Time.deltaTime;
        alpha = Mathf.Clamp(alpha, 0, 1);

        float height = Mathf.Lerp(1.8f, 1.2f, alpha);
        float capsuleY = Mathf.Lerp(0, -0.3f, alpha);
        float camY = Mathf.Lerp(0.6f, 0, alpha);

        controller.height = height;
        controller.center = new Vector3(0, capsuleY, 0);
        playerCamera.transform.localPosition = new Vector3(0, camY, 0);
    }

    void CrouchControl()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (state == PlayerState.SPRINT) state = PlayerState.SLIDE;
            else if(state == PlayerState.WALK) state = PlayerState.CROUCH;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if(state == PlayerState.CROUCH || state == PlayerState.SLIDE)
                state = PlayerState.WALK;
        }

        if (state == PlayerState.SLIDE && velocity.magnitude < 8) 
            state = PlayerState.CROUCH;
    }


    void SpeedChangeControl()
    {
        float change = accelerationSpeed * Time.deltaTime;
        if (state == PlayerState.SPRINT) currentSpeed += change;
        else if (state == PlayerState.CROUCH) currentSpeed -= change;
        else if (state == PlayerState.WALK)
        {
            if (currentSpeed > normalSpeed) currentSpeed -= change;
            else currentSpeed += change;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, crouchSpeed, sprintSpeed);
    }

    void SprintControl()
    {
        Vector3 cameraDir = playerCamera.transform.forward;
        cameraDir = new Vector3(cameraDir.x, 0, cameraDir.z).normalized;
        Vector3 dirVelocity = velocity.normalized;
        float angle = Vector3.Angle(cameraDir, dirVelocity);

        if (Input.GetKeyDown(KeyCode.LeftShift) && state == PlayerState.WALK)
        {
            state = PlayerState.SPRINT;
        }
        
        if (angle > 50 && state == PlayerState.SPRINT) state = PlayerState.WALK;
        if (staminaController.currentStam <= 0 && state == PlayerState.SPRINT)
            state = PlayerState.WALK;
        if(velocity.magnitude < crouchSpeed && state == PlayerState.SPRINT) 
            state = PlayerState.WALK;
    }

    public void StopSprint()
    {
        if (state == PlayerState.SPRINT)
            state = PlayerState.WALK;
    }

    void HorizontalMove()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        Vector3 dir = transform.right * inputX + transform.forward * inputZ;
        
        if(!isGrounded) // изменение скорости в полёте
            velocity += dir * Time.deltaTime * flySpeedControl;
        else if (state == PlayerState.SLIDE)
            velocity -= velocity.normalized * Time.deltaTime * slideSlowness;
        else // перемещение при соприкосновении с землёй
        {
            velocity = dir * currentSpeed;
            if (velocity.magnitude > currentSpeed) velocity = velocity.normalized * currentSpeed;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void VerticalMovement()
    {
        isGrounded = Physics.CheckSphere(legs.transform.position, 0.2f, groundMask);
        velocityY = velocityY + gravity * Time.deltaTime;

        if (isGrounded == true && velocityY < 0)
        {
            velocityY = -2f;

            if (Input.GetKeyDown(KeyCode.Space) && staminaController.currentStam > 0)
            {
                if(state == PlayerState.CROUCH) velocityY = playerJump/2;
                else velocityY = playerJump;
                staminaController.SpendStamina(20);
            }
        }

        Vector3 velocityDown = new Vector3(0, velocityY, 0);
        controller.Move(velocityDown * Time.deltaTime);
    }
}
