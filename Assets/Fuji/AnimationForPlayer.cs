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
        if (MoveForPlayer._gameInputs != null)
        {
            Vector2 inputDirection = MoveForPlayer._gameInputs.Player.Move.ReadValue<Vector2>();

            if (inputDirection.magnitude > 0.2f)
            {
                myAnim.SetBool("isMoving", true);

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
        if (characterStatus.landedThisFrame)
        {
            myAnim.SetBool("Land", true);
            myAnim.SetBool("InAir", false);
            return;
        }
        if (!characterStatus.isOnGround)
        {
            myAnim.SetBool("InAir", true);
            myAnim.SetBool("Land", false);
        }
    }
}
