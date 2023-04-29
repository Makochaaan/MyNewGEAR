using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStatus))]
public class AnimationForPlayer : MonoBehaviour
{
    // アニメーターコントローラと、キャラクターの状態(着地しているか、等)
    private Animator myAnim;
    private CharacterStatus characterStatus;

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーの入力を取得
        if (MoveForPlayer._gameInputs != null)
        {
            // プレイヤーの移動入力を二次元ベクトルで取得
            Vector2 inputDirection = MoveForPlayer._gameInputs.Player.Move.ReadValue<Vector2>();
            // 入力の大きさが0.2より大きいなら移動しているはずなので
            // アニメーターコントローラの、動いている真偽値を真に
            if (inputDirection.magnitude > 0.2f)
            {
                myAnim.SetBool("isMoving", true);
                // ダッシュ中はアニメーション再生速度を早める
                if (MoveForPlayer._gameInputs.Player.Dash.ReadValue<float>() == 1)
                {
                    myAnim.SetFloat("MoveSpeed", characterStatus.boostFactor);
                }
                else
                {
                    myAnim.SetFloat("MoveSpeed", 1);
                }
            }
            else
            {
                myAnim.SetBool("isMoving", false);
            }
        }
        // キャラクターが着地した瞬間は着地アニメーション
        if (characterStatus.landedThisFrame)
        {
            myAnim.SetBool("Land", true);
            myAnim.SetBool("InAir", false);
            return;
        }
        // キャラクターが地上にいないなら空中姿勢アニメーション
        if (!characterStatus.isOnGround)
        {
            myAnim.SetBool("InAir", true);
            myAnim.SetBool("Land", false);
        }
    }
}
