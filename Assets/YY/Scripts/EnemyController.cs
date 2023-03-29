using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 velocity;
    public float speed;
    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        direction = new Vector3(x, 0, z);
        direction.Normalize();
        velocity = direction * speed;
        rb.velocity = velocity;
    }
}