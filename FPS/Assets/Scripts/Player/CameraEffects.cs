using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] float fieldOfViewSpeed = 100;
    PlayerMovement player;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerState state = player.GetPlayerState();
        float cameraFOV = cam.fieldOfView;
        if (state == PlayerState.IN_SPRINT) cameraFOV += fieldOfViewSpeed * Time.deltaTime;
        else if (state != PlayerState.IN_AIR) cameraFOV -= fieldOfViewSpeed * Time.deltaTime;
        cameraFOV = Mathf.Clamp(cameraFOV, 60, 80);
        cam.fieldOfView = cameraFOV;
    }
}
