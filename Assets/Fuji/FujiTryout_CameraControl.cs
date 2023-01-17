using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FujiTryout_CameraControl : MonoBehaviour
{
    private Debug_Player inputActions;
    [SerializeField] private Vector3 initialCameraOffset;
    [SerializeField] private float cameraRotH;
    [SerializeField] private float cameraRotV;
    private Vector2 mouseDelta;
    private Transform mainCam;
    private Transform followTarget;

    private void Awake()
    {
        inputActions = new Debug_Player();
        inputActions.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeCamera();
    }

    private void InitializeCamera()
    {
        mainCam = Camera.main.transform;
        followTarget = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //ÉJÉÅÉâëÄçÏ
        if (transform.position.y < mainCam.position.y)
        {
            mouseDelta = inputActions.Game.Camera.ReadValue<Vector2>();
            mainCam.RotateAround(transform.position, Vector3.up, mouseDelta.x * cameraRotH * Time.deltaTime);
            mainCam.RotateAround(transform.position, mainCam.right, mouseDelta.y * cameraRotV * Time.deltaTime);
        }
        else
        {
            mainCam.RotateAround(transform.position, mainCam.right, 10);
        }
        transform.position = followTarget.position;
    }
}
