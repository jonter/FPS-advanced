using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    float xRotation = 0;

    [SerializeField] float mouseSensivity = 2;

    [SerializeField] GameObject player;

    public Vector3 extraRotate;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity;

        xRotation = xRotation - mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);


        Vector3 rot = new Vector3(xRotation, 0, 0) + extraRotate;
        transform.localRotation = Quaternion.Euler(rot);

        player.transform.Rotate(Vector3.up*mouseX);
    }
}
