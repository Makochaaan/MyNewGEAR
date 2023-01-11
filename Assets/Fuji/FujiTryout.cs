using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FujiTryout : MonoBehaviour
{
    private Rigidbody myRb;
    private Debug_Player inputActions;
    private Vector2 inputDirectionRaw;
    private Vector3 inputDirectionBasedOnCamera;
    [SerializeField] private float moveSpeed;

    [SerializeField] private Vector3 initialCameraOffset;
    [SerializeField] private float cameraRotH;
    [SerializeField] private float cameraRotV;

    [SerializeField] private Transform playerCamera;
    private Vector2 mouseDelta;
    private void Awake()
    {
        myRb = GetComponent<Rigidbody>();
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
        playerCamera.position = transform.position + initialCameraOffset;
    }

    // Update is called once per frame
    void Update()
    {
        //ÉJÉÅÉâäÓèÄÇÃé©ã@à⁄ìÆ
        inputDirectionRaw = inputActions.Game.Move.ReadValue<Vector2>();
        inputDirectionBasedOnCamera = (playerCamera.right * inputDirectionRaw.x) + (new Vector3(playerCamera.forward.x, 0, playerCamera.forward.z) * inputDirectionRaw.y);

        if (inputDirectionRaw.magnitude > 0.2f)
        {
            transform.LookAt(Vector3.Lerp(transform.position + transform.forward.normalized, transform.position + inputDirectionBasedOnCamera.normalized, 5*Time.deltaTime), Vector3.up);
            myRb.MovePosition(transform.position + inputDirectionBasedOnCamera * moveSpeed * Time.deltaTime);
        }

        //ÉJÉÅÉâëÄçÏ
        if (transform.position.y < playerCamera.position.y)
        {
            mouseDelta = inputActions.Game.Camera.ReadValue<Vector2>();
            playerCamera.RotateAround(transform.position, Vector3.up, mouseDelta.x * cameraRotH * Time.deltaTime);
            playerCamera.RotateAround(transform.position, playerCamera.right, mouseDelta.y * cameraRotV * Time.deltaTime);
        }
        else
        {
            playerCamera.RotateAround(transform.position, playerCamera.right, 10);
        }
        playerCamera.parent.position = transform.position;
    }
}
