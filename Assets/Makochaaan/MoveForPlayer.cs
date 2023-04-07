using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(CharacterStatus))]
public class MoveForPlayer : MonoBehaviour
{
    /*参考文献：http://qiita.com/yando/items/c406690c9ad87ecfc8e5
    * StandardAsset ThirdPersonUserControl.cs
    */
    [SerializeField] private float _jumpForce = 5;
    private float boostFactorTemp;
    private bool energyEnabled = true;

    public static GameInputs _gameInputs;
    private Vector2 _moveInputValue;

    private Transform camTransform;
    private Vector3 camForward;
    private Vector3 inputDirection;
    private Vector3 moveDirection;

    private Rigidbody _rigidbody;
    private CharacterStatus _characterStatus;

    private void Awake()
    {
        _characterStatus = GetComponent<CharacterStatus>();
        _rigidbody = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;
        // Input Actionインスタンス生成
        _gameInputs = new GameInputs();

        // Actionイベント登録
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;
        _gameInputs.Player.Jump.performed += OnJump;

        Cursor.lockState = CursorLockMode.Locked;
        boostFactorTemp = 1;
    }
    private void Start()
    {
        Invoke("EnableWithDelay",3);
    }
    void EnableWithDelay()
    {
        // Input Actionを機能させるためには、有効化する必要がある
        _gameInputs.Enable();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _moveInputValue = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_characterStatus.currentEnergy > _characterStatus.energyConsumption / 2)
        {
            // ジャンプする力を与える
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _characterStatus.currentEnergy -= _characterStatus.energyConsumption / 2;
        }
    }
    private IEnumerator LockEnergy(float delay)
    {
        energyEnabled = false;
        yield return new WaitForSeconds(delay);
        energyEnabled = true;
    }
    private void FixedUpdate()
    {
        //キーボード数値取得。プレイヤーの方向として扱う
        float h = _moveInputValue.x;//横
        float v = _moveInputValue.y;//縦
                                    //2つのベクトルの各成分の乗算(Vector3.Scale)。単位ベクトル化(.normalized)
        camForward = Vector3.Scale(camTransform.forward, new Vector3(1, 0, 1)).normalized;

        inputDirection = (camForward * v) + (camTransform.right * h);

        //方向転換
        moveDirection = new Vector3(_moveInputValue.x, 0, _moveInputValue.y).normalized;
        boostFactorTemp = (energyEnabled && _gameInputs.Player.Dash.ReadValue<float>() == 1 && _characterStatus.currentEnergy > 0) ? _characterStatus.boostFactor : 1;
        //エネルギー回復
        if (_characterStatus.isOnGround && boostFactorTemp == 1 && _characterStatus.currentEnergy < _characterStatus.maxEnergy)
        {
            _characterStatus.currentEnergy += _characterStatus.energyRecoverySpeed * Time.deltaTime;
        }

        if (_moveInputValue.magnitude > 0.2f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(inputDirection), 360 * Time.deltaTime);
            moveDirection = Quaternion.LookRotation(camForward) * moveDirection;
        }
        else
        {
            return;
        }

        _rigidbody.MovePosition(transform.position + (moveDirection * _characterStatus.speed * (100.0f/_characterStatus.weight) * Time.deltaTime) * boostFactorTemp);

        //ダッシュ中にエネルギーを消費する
        if (boostFactorTemp > 1)
        {
            _characterStatus.currentEnergy -= _characterStatus.energyConsumption * Time.deltaTime;
            //エネルギーがほぼ空になったらブースト使用を禁止し、回復する
            if (_characterStatus.currentEnergy < 1) StartCoroutine(LockEnergy(1));
            //空中ダッシュは重力を軽減する
            if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.AddForce(Vector3.up * -_rigidbody.velocity.y * 5 * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }
}