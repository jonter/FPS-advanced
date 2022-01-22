using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float playerSpeed = 10;
    [SerializeField] float playerJump = 20;

    CharacterController controller;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocityDown;

    bool isGrounded = false;

    [SerializeField] GameObject legs;
    [SerializeField] LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        velocityDown = Vector3.zero; 
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(legs.transform.position, 0.4f, groundMask);

        float inputX = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
        float inputZ = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;

        Vector3 velocity = transform.right * inputX + transform.forward * inputZ;


        controller.Move(velocity);

        velocityDown.y = velocityDown.y + gravity * Time.deltaTime;

        if (isGrounded == true)
        {
            velocityDown.y = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocityDown.y = playerJump;
            }

        }

        

        controller.Move(velocityDown * Time.deltaTime);

        
    }
}
