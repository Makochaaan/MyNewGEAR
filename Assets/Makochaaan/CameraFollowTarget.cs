using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField, Tooltip("unitychan")]
    private GameObject TargetObject;
    private Camera cam;
    private Vector3 startPos;
    private Vector3 startAngle;

    private Vector3 distance;
 
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        distance = cam.transform.position - TargetObject.transform.position;
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


        cam.transform.position = TargetObject.transform.position + distance;
        
        // cam.transform.Rotate(0, rotateX, 0.0f);   
    }
        
}