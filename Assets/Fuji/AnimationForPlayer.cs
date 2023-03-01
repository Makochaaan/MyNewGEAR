using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStatus))]
public class AnimationForPlayer : MonoBehaviour
{
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
        if (characterStatus.landedThisFrame)
        {
            myAnim.SetBool("Land", true);
            myAnim.SetBool("Jump", false);
        }
        if (characterStatus.jumpedThisFrame)
        {
            myAnim.SetBool("Jump", true);
            myAnim.SetBool("Land", false);
            characterStatus.jumpedThisFrame = false;
        }
    }
}
