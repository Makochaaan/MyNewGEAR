using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MoveForPlayer : MonoBehaviour {
    /*参考文献：http://qiita.com/yando/items/c406690c9ad87ecfc8e5
    * StandardAsset ThirdPersonUserControl.cs
    */
    [SerializeField] private float _moveForce = 5;
    [SerializeField] private float _jumpForce = 5;

    [SerializeField,Tooltip("unitychan")]
    private Transform CamPos;
    private Vector3 Camforward;
    private Vector3 ido;
    private Vector3 move;

    private Rigidbody _rigidbody;
    private GameInputs _gameInputs;
    private Vector2 _moveInputValue;
    private float _dashInputValue;

    [SerializeField, Tooltip("unitychan")]
    private GameObject TargetObject;
    
    
    float angle;
    float mousePos;
    float runspeed = 1.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // Input Actionインスタンス生成
        _gameInputs = new GameInputs();

        // Actionイベント登録
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;
        _gameInputs.Player.Jump.performed += OnJump;

        // Input Actionを機能させるためには、
        // 有効化する必要がある
        _gameInputs.Enable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _moveInputValue = context.ReadValue<Vector2>();

    }

    private void OnJump(InputAction.CallbackContext context)
    {
        // ジャンプする力を与える
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    void Start()
    {   
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        //キーボード数値取得。プレイヤーの方向として扱う
        float h = _moveInputValue.x;//横
        float v = _moveInputValue.y;//縦
        // Debug.Log(new Vector2(h,v));

        //カメラのTransformが取得されてれば実行
        if (CamPos != null)
        {
            //2つのベクトルの各成分の乗算(Vector3.Scale)。単位ベクトル化(.normalized)
            Camforward = Vector3.Scale(CamPos.forward, new Vector3(1, 0, 1)).normalized;
            
            ido = (Camforward * v) + (CamPos.right * h);
            //Debug.Log(ido);
        }

        //現在のポジションにidoのトランスフォームの数値を入れる
        // transform.position = new Vector3(
        // transform.position.x + ido.x,
        // 0,
        // transform.position.z + ido.z);

        //方向転換用Transform

        //方向転換
        Vector3 newDir= ido;
        move = new Vector3(_moveInputValue.x,0,_moveInputValue.y);

        if(new Vector2(h,v).magnitude > 0.2f){
            transform.rotation =Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(newDir),360*Time.deltaTime);
            move = Quaternion.LookRotation(Camforward) * move;
        }

        // _rigidbody.AddForce();
        if(_gameInputs.Player.Dash.ReadValue<float>() == 1){
            runspeed=1.5f;
        } else {
            runspeed=1.0f;
        }
        _rigidbody.MovePosition(TargetObject.transform.position+(move* _moveForce* Time.deltaTime)* runspeed);
        
        
    }

}