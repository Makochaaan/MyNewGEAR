using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTest : MonoBehaviour
{
    [SerializeField] private GameInputs gameInput;
    // Start is called before the first frame update
    void Start()
    {
        gameInput = new GameInputs();
        gameInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameInput.Player.Camera.ReadValue<Vector2>());
    }
}
