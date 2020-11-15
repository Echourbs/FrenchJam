using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraControl : MonoBehaviour
{
    [HideInInspector]
    public CinemachineVirtualCamera cam;

    [HideInInspector]
    public float originalFov;


    [HideInInspector]
    public float desirableFov;
    
    [HideInInspector]
    public float cameraDesirableCenterHeight;
    
    public float time = 1;

    [HideInInspector]
    public Transform mapedCameraTargetTransform;



    void Start()
    {
        cam = Camera.main.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        originalFov = desirableFov = cam.m_Lens.FieldOfView;
        
        cameraDesirableCenterHeight = 0;
    }

    public void ResetOnBossFightEnd()
    {
        desirableFov = originalFov;

        cam.Follow = MovementScript.player.transform;
        cam.LookAt = null;

        ChangeCamera();
    }

    void ChangeCamera()
    {
        if (cam.m_Lens.FieldOfView != originalFov || mapedCameraTargetTransform.localPosition.y != 0)
        {
            float calculatedValueFov = desirableFov / time;
            cam.m_Lens.FieldOfView = Mathf.MoveTowards(cam.m_Lens.FieldOfView, originalFov, calculatedValueFov * Time.deltaTime);

            Invoke("ChangeCamera", Time.deltaTime);
        }
        else
        {
            cam.Follow = MovementScript.player.transform;
            cam.LookAt = null;
        }
    }
}
