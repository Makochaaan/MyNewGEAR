using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraControl : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float sensitivityX, sensitivityY;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = MoveForPlayer._gameInputs.Player.Camera.ReadValue<Vector2>();
        if (inputVector.magnitude > 0.2f)
        {
            Camera.main.transform.RotateAround(transform.position, Vector3.up, inputVector.x * sensitivityX * Time.deltaTime);
            Camera.main.transform.RotateAround(transform.position, Camera.main.transform.right, inputVector.y * sensitivityY * Time.deltaTime);
        }
        transform.position = player.position;
    }       
}