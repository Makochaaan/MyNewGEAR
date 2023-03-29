using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MoveForPlayer : MonoBehaviour
{
    /*参考文献：http://qiita.com/yando/items/c406690c9ad87ecfc8e5
    * StandardAsset ThirdPersonUserControl.cs
    */
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private float dashFactor = 1.5f;
    private float dashFactorTemp;

    public static GameInputs _gameInputs;
    private Vector2 _moveInputValue;

    private Transform camTransform;
    private Vector3 camForward;
    private Vector3 inputDirection;
    private Vector3 moveDirection;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;
        // Input Actionインスタンス生成
        _gameInputs = new GameInputs();

        // Actionイベント登録
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

    private void FixedUpdate()
    {
        //キーボード数値取得。プレイヤーの方向として扱う
        float h = _moveInputValue.x;//横
        float v = _moveInputValue.y;//縦
                                    //2つのベクトルの各成分の乗算(Vector3.Scale)。単位ベクトル化(.normalized)
        camForward = Vector3.Scale(camTransform.forward, new Vector3(1, 0, 1)).normalized;

        inputDirection = (camForward * v) + (camTransform.right * h);

        //方向転換用Transform

        //方向転換
        moveDirection = new Vector3(_moveInputValue.x, 0, _moveInputValue.y).normalized;

        if (_moveInputValue.magnitude > 0.2f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(inputDirection), 360 * Time.deltaTime);
            moveDirection = Quaternion.LookRotation(camForward) * moveDirection;
        }
        else
        {
            return;
        }

        dashFactorTemp = (_gameInputs.Player.Dash.ReadValue<float>() == 1) ? dashFactor : 1;

        _rigidbody.MovePosition(transform.position + (moveDirection * _moveSpeed * Time.deltaTime) * dashFactorTemp);
        if (dashFactorTemp > 1 && _rigidbody.velocity.y < 0)
        {
            _rigidbody.AddForce(Vector3.up * -_rigidbody.velocity.y * 5 * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}