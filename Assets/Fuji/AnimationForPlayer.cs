using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationForPlayer : MonoBehaviour
{
    private Animator myAnim;
    private Transform groundTemp;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputDirection = MoveForPlayer._gameInputs.Player.Move.ReadValue<Vector2>();
        if(inputDirection.magnitude > 0.2f)
        {
            myAnim.SetFloat("MoveSpeed", 1);
            myAnim.speed = (MoveForPlayer._gameInputs.Player.Dash.ReadValue<float>() == 1) ? 1.5f : 1;
        }
        else
        {
            myAnim.SetFloat("MoveSpeed", 0);
        }
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up * 0.2f,Vector3.down, out hit, 0.5f))
        {
            if(groundTemp == null)
            {
                Debug.Log("Land");
                myAnim.SetBool("Land", true);
                myAnim.SetBool("Jump", false);
                groundTemp = hit.transform;
                return;
            }
        }
        else
        {
            if (groundTemp != null)
            {
                Debug.Log("Jump");
                myAnim.SetBool("Jump", true);
                myAnim.SetBool("Land", false);
                groundTemp = null;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.3f);
    }
}
