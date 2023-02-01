using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FujiTryout_PlayerMove : MonoBehaviour
{
    private Rigidbody myRb;
    private Debug_Player inputActions;
    private Vector2 inputDirectionRaw;
    private Vector3 inputDirectionBasedOnCamera;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float boostMult;
    [SerializeField] private float jumpPower;
    private float boostMultTemp;
    
    private Transform mainCam;
    
    private void Awake()
    {
        myRb = GetComponent<Rigidbody>();
        inputActions = new Debug_Player();
        inputActions.Enable();
    }
    // Start is called before the first frame update
    private void Start()
    {
        mainCam = Camera.main.transform;
        boostMultTemp = boostMult;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = transform.position;
        /*
        if (inputActions.Game.Jump.ReadValue<float>() == 1)
        {
            myRb.AddForce(Physics.gravity * -0.2f, ForceMode.Force);
            destination += Vector3.up * jumpPower * Time.deltaTime;   
        }
        
        if(inputActions.Game.Boost.ReadValue<float>() == 1)
        {
            boostMultTemp = boostMult;
        }
        else
        {
            boostMultTemp = 1;
        }
        //ÉJÉÅÉâäÓèÄÇÃé©ã@à⁄ìÆ
        inputDirectionRaw = inputActions.Game.Move.ReadValue<Vector2>();
        inputDirectionBasedOnCamera = (mainCam.right * inputDirectionRaw.x) + (new Vector3(mainCam.forward.x, 0, mainCam.forward.z) * inputDirectionRaw.y);

        if (inputDirectionRaw.magnitude > 0.2f)
        {
            transform.LookAt(Vector3.Lerp(transform.position + transform.forward.normalized, transform.position + inputDirectionBasedOnCamera.normalized, 5*Time.deltaTime), Vector3.up);
            destination += inputDirectionBasedOnCamera * moveSpeed * boostMultTemp * Time.deltaTime;
        }
        myRb.MovePosition(destination);
        */
        inputDirectionRaw = inputActions.Game.Move.ReadValue<Vector2>();
        if (inputDirectionRaw.magnitude > 0.2f)
        {
            destination += new Vector3(inputDirectionRaw.x, 0, inputDirectionRaw.y) * moveSpeed * boostMultTemp * Time.deltaTime;
        }
        myRb.MovePosition(destination);
    }
}
