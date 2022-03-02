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
        ZoomCamera(state);
        SprintEffect(state);

        cam.GetComponent<PlayerLook>().extraRotate = sprintRotation + coilRotation;
    }

    void SprintEffect(PlayerState state)
    {
        if (state == PlayerState.IN_SPRINT)
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
            t2 += Time.deltaTime * sprintRate * 2;
            sprintRotation = Vector3.Lerp(start, end, t2);
            yield return null;
        }
        sprintRotation = end;
    }

    void ZoomCamera(PlayerState state)
    {
        float cameraFOV = cam.fieldOfView;
        if (state == PlayerState.IN_SPRINT || state == PlayerState.IN_SLIDE)
            cameraFOV += fieldOfViewSpeed * Time.deltaTime;
        else if (state != PlayerState.IN_AIR) cameraFOV -= fieldOfViewSpeed * Time.deltaTime;
        cameraFOV = Mathf.Clamp(cameraFOV, 60, 80);
        cam.fieldOfView = cameraFOV;
    }
}
