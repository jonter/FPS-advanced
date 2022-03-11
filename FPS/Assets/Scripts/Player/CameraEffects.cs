using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] float fieldOfViewSpeed = 100;
    PlayerMovement player;
    Camera cam;

    Vector3 sprintRotation, coilRotation;

    [Header("Sprint Settings")]
    [SerializeField] float sprintAmplitude = 5;
    [SerializeField] float sprintRate = 1;

    [Header("Coil (shooting) Settings")]
    [SerializeField] float coilAmplitude = 10;
    [SerializeField] float coilRate = 5;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        cam = GetComponent<Camera>();
    }

    public IEnumerator Coil(float amplitudeMult = 1, float rateMult = 1)
    {
        float xRot = -0.01f;
        float t2 = 0;

        while(xRot < 0)
        {
            t2 += Time.deltaTime * coilRate * rateMult;
            xRot = -Mathf.Sin(t2) * coilAmplitude * amplitudeMult;  
            coilRotation = new Vector3(xRot, 0, 0);
            yield return null;
        }
        coilRotation = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerState state = player.GetPlayerState();
        bool isGrounded = player.GetGroundState();
        ZoomCamera(state, isGrounded);
        SprintEffect(state, isGrounded);

        cam.GetComponent<PlayerLook>().extraRotate = sprintRotation + coilRotation;
    }

    void SprintEffect(PlayerState state, bool isGrounded)
    {
        if (state == PlayerState.SPRINT && isGrounded)
        {
            timer += Time.deltaTime;
            float yRot = Mathf.Sin(timer*sprintRate) * sprintAmplitude;
            float xRot = Mathf.Sin(timer * 2 * sprintRate) * sprintAmplitude/2;
            sprintRotation = new Vector3(xRot, yRot, 0);
        }
        else if(timer > 0)
        {
            StartCoroutine(EndSprint());
        }
    }

    IEnumerator EndSprint()
    {
        timer = 0;
        float t2 = 0;
        Vector3 start = sprintRotation;
        Vector3 end = new Vector3(0, 0, 0);
        while(t2 < 1)
        {
            t2 += Time.deltaTime * sprintRate * 3;
            sprintRotation = Vector3.Lerp(start, end, t2);
            yield return null;
        }
        sprintRotation = end;
    }

    void ZoomCamera(PlayerState state, bool isGrounded)
    {
        float cameraFOV = cam.fieldOfView;
        if (state == PlayerState.SPRINT || state == PlayerState.SLIDE)
            cameraFOV += fieldOfViewSpeed * Time.deltaTime;
        else if (isGrounded) cameraFOV -= fieldOfViewSpeed * Time.deltaTime;
        cameraFOV = Mathf.Clamp(cameraFOV, 60, 80);
        cam.fieldOfView = cameraFOV;
    }
}
