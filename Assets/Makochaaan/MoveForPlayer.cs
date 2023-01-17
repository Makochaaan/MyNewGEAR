using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveForPlayer : MonoBehaviour {
    /*参考文献：http://qiita.com/yando/items/c406690c9ad87ecfc8e5
    * StandardAsset ThirdPersonUserControl.cs
    */
    private Transform CamPos;
    private Vector3 Camforward;
    private Vector3 ido;
    private Vector3 Animdir = Vector3.zero;
    

    float runspeed = 0.2f;
    float mousePos;
    void Start()
    {   
        Cursor.lockState = CursorLockMode.Locked;
        if (Camera.main != null)
        {
            CamPos = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        }

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        //キーボード数値取得。プレイヤーの方向として扱う
        float h = Input.GetAxis("Horizontal");//横
        float v = Input.GetAxis("Vertical");//縦

        //カメラのTransformが取得されてれば実行
        if (CamPos != null)
        {
            //2つのベクトルの各成分の乗算(Vector3.Scale)。単位ベクトル化(.normalized)
            Camforward = Vector3.Scale(CamPos.forward, new Vector3(1, 0, 1)).normalized;
            //移動ベクトルをidoというトランスフォームに代入
            ido = v * Camforward * runspeed + h * CamPos.right * runspeed;
            //Debug.Log(ido);
        }

        //現在のポジションにidoのトランスフォームの数値を入れる
        transform.position = new Vector3(
        transform.position.x + ido.x,
        0,
        transform.position.z + ido.z);

        //方向転換用Transform

        Vector3 AnimDir = ido;
        AnimDir.y = 0;
        //方向転換
        if (AnimDir.sqrMagnitude > 0.001)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward,AnimDir,5f*Time.deltaTime,0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        } else {
            // if (Math.Abs((Screen.width/2)-Input.mousePosition.x)>50||Math.Abs((Screen.height/2)-Input.mousePosition.y)>50){
            transform.rotation = Quaternion.LookRotation(Camforward);
            // }
        }
    }

}