using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform player;
    [SerializeField] private SaveData saveData;
    private float sensitivity, aimFactor,aimFactorTemp, flipX, flipY;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        sensitivity = saveData.jsonProperty.cameraSensitivity;
        //�X���C�_�[�Ƃ̌��ˍ����Œl��10����100�Ȃ̂�100�Ŋ����Ċ����Ƃ���
        aimFactor = saveData.jsonProperty.aimFactor / 100;
        flipX = saveData.jsonProperty.flipX;
        flipY = saveData.jsonProperty.flipY;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = MoveForPlayer._gameInputs.Player.Camera.ReadValue<Vector2>();
        bool isAimButtonPressed = (MoveForPlayer._gameInputs.Player.Aim.ReadValue<float>() == 1);
        aimFactorTemp = isAimButtonPressed ? aimFactor : 1;
        if (inputVector.magnitude > 0.2f)
        {
            Camera.main.transform.RotateAround(transform.position, Vector3.up, inputVector.x * sensitivity * flipX * aimFactorTemp * Time.deltaTime);
            Camera.main.transform.RotateAround(transform.position, Camera.main.transform.right, inputVector.y * sensitivity * flipY * aimFactorTemp * Time.deltaTime);
        }
        transform.position = player.position;
    }
}