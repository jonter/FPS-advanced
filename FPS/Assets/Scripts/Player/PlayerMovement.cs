using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    WALK,
    SPRINT,
    SLIDE,
    CROUCH,
    WALLRUN
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

    Vector3 wallNormal;

    float gravity;

    bool isGrounded = false;
    bool disableWallrun = false;

    [SerializeField] GameObject legs;
    [SerializeField] LayerMask groundMask;

    float alpha = 0;

    public float GetSpeed()
    {
        return velocity.magnitude;
    }

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
        WallrunControl();
        SpeedChangeControl();
        SizeControl();
        print(state);
    }

    void WallrunControl()
    {
        if (disableWallrun) return;
        if(isGrounded == true)
        {
            if (state == PlayerState.WALLRUN) state = PlayerState.WALK;
        }
        else
        {
            RaycastHit rayInfo = CheckRaycastsForWall();
            if (rayInfo.transform == null)
            {
                if (state == PlayerState.WALLRUN) state = PlayerState.SPRINT;
                return;
            }
            if (state != PlayerState.SPRINT) return;
            wallNormal = rayInfo.normal;
            Vector3 dirVel = new Vector3(velocity.x, 0, velocity.z).normalized;
            float degree = Vector3.Angle(dirVel, rayInfo.normal);
            if (degree < 130 && degree > 85)
                state = PlayerState.WALLRUN;
            else state = PlayerState.SPRINT;
        }

    }

    RaycastHit CheckRaycastsForWall()
    {
        float heightRay = 0.5f;
        RaycastHit rayInfo;
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = transform.position;
            Vector3 dir;
            if (i < 3) dir = transform.right * 0.5f;
            else dir = -transform.right * 0.5f;

            bool isHit = Physics.Raycast(pos, dir, out rayInfo, 1f ,groundMask);
            if (isHit) return rayInfo;

            heightRay -= 0.5f;
            if (heightRay < -0.6f) heightRay = 0.5f;
        }
        return new RaycastHit();
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

        if (state == PlayerState.WALLRUN)
            velocity = dir * currentSpeed;
        else if (!isGrounded) // изменение скорости в полёте
            velocity += dir * Time.deltaTime * flySpeedControl;
        else if (state == PlayerState.SLIDE)
            velocity -= velocity.normalized * Time.deltaTime * slideSlowness;
        else // перемещение при соприкосновении с землёй
            velocity = dir * currentSpeed;

        if (velocity.magnitude > currentSpeed) velocity = velocity.normalized * currentSpeed;

        controller.Move(velocity * Time.deltaTime);
    }

    void VerticalMovement()
    {
        isGrounded = Physics.CheckSphere(legs.transform.position, 0.2f, groundMask);
        if(state == PlayerState.WALLRUN && velocityY < 0) 
            velocityY += gravity * Time.deltaTime * 0.2f;
        else 
            velocityY += gravity * Time.deltaTime;

        if (isGrounded) PreventWallrun();

        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f;

            if (Input.GetKeyDown(KeyCode.Space) && staminaController.currentStam > 0)
            {
                if(state == PlayerState.CROUCH) velocityY = playerJump/2;
                else velocityY = playerJump;
                staminaController.SpendStamina(20);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)&& state == PlayerState.WALLRUN 
            && staminaController.currentStam > 0)
        {
            velocityY = playerJump;
            velocity += wallNormal * 10;
            state = PlayerState.SPRINT;
            staminaController.SpendStamina(20);
            StartCoroutine(WallrunDisable());
        }

        Vector3 velocityDown = new Vector3(0, velocityY, 0);
        controller.Move(velocityDown * Time.deltaTime);
    }

    IEnumerator WallrunDisable()
    {
        disableWallrun = true;
        yield return new WaitForSeconds(0.2f);
        disableWallrun = false;
    }



    void PreventWallrun()
    {
        RaycastHit hitInfo;
        bool isHitBottom = Physics.Raycast(legs.transform.position, 
            -legs.transform.up, out hitInfo, 1, groundMask);
        if (isHitBottom == false) return;

        float degreeLand = Vector3.Angle(Vector3.up, hitInfo.normal);
        if (degreeLand < 50) return;

        isGrounded = false;
        velocity += new Vector3(hitInfo.normal.x, 0, hitInfo.normal.y) * Time.deltaTime * 40;
    }

}
