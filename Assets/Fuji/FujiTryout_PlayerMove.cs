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
        if (inputActions.Game.Jump.ReadValue<float>() == 1)
        {
            destination += Vector3.up * jumpPower * Time.deltaTime;
        }

        //�_�b�V���A�W�����v���͏d�͖���
        myRb.useGravity = (inputActions.Game.Jump.ReadValue<float>() == 1 || inputActions.Game.Boost.ReadValue<float>() == 1) ? false : true;

        //���́B���͂��Ȃ��ꍇ����ȍ~�̏����͈Ӗ����Ȃ��̂�return
        inputDirectionRaw = inputActions.Game.Move.ReadValue<Vector2>();
        if (!(inputDirectionRaw.magnitude != 0 || inputActions.Game.Jump.ReadValue<float>() != 0))
        {
            return;
        }
        
        //�_�b�V���A�W�����v�̂͂��߂Ɉړ��x�N�g�������Z�b�g
        if (inputActions.Game.Jump.WasPressedThisFrame() || inputActions.Game.Boost.WasPressedThisFrame())
        {
            myRb.velocity = Vector3.zero;
        }

        if (inputActions.Game.Boost.ReadValue<float>() == 1)
        {
            boostMultTemp = boostMult;
        }
        else
        {
            boostMultTemp = 1;
        }
        //�J������̎��@�ړ�
        inputDirectionBasedOnCamera = ((mainCam.right * inputDirectionRaw.x) + (Vector3.Scale(mainCam.forward,new Vector3(1,0,1)) * inputDirectionRaw.y)).normalized;
        if (inputDirectionRaw.magnitude > 0.2f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(inputDirectionBasedOnCamera, Vector3.up), 360 * Time.deltaTime);
            destination += inputDirectionBasedOnCamera * moveSpeed * boostMultTemp * Time.deltaTime;
        }
        myRb.MovePosition(destination);
        /*
        
        inputDirectionRaw = inputActions.Game.Move.ReadValue<Vector2>();
        if (inputDirectionRaw.magnitude > 0.2f)
        {
            destination += new Vector3(inputDirectionRaw.x, 0, inputDirectionRaw.y) * moveSpeed * boostMultTemp * Time.deltaTime;
        }
        myRb.MovePosition(destination);
        */
    }
}
