using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    float xRotation = 0;

    [SerializeField] float mouseSensivity = 100f;

    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensivity;

        // Вращение головы вверх-вниз
        xRotation = xRotation - mouseY;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0 , 0);

        // Поворот персонажа влево-вправо

        player.transform.Rotate(Vector3.up*mouseX);
    }
}
