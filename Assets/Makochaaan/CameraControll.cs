using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraControll : MonoBehaviour
{
    [SerializeField, Tooltip("unitychan")]
    private GameObject TargetObject;
    private Camera cam;
    private Vector3 startPos;
    private Vector3 startAngle;

    private Vector3 lastTargetPosition;
 
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }
 
    // Update is called once per frame
    void Update()
    {
        if (cam == null)
        {
            return;
        }
 
        float sensitiveMove = 0.8f;
        float sensitiveRotate = 5.0f;
        float sensitiveZoom = 10.0f;

        // rotate camera
        float rotateX = Input.GetAxis("Mouse X") * sensitiveRotate;
    
        cam.transform.RotateAround (TargetObject.transform.position, Vector3.up, rotateX);

        cam.transform.position += TargetObject.transform.position - lastTargetPosition;
        lastTargetPosition = TargetObject.transform.position;
        
        // cam.transform.Rotate(0, rotateX, 0.0f);   
    }
        
}