using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Vector2 _moveDirection;
    private bool _isJumping;

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _moveDirection = new Vector2(x, y);

        _isJumping = _isJumping | Input.GetKeyDown(KeyCode.Space);


    }

    public InputStructure GetInputs()
    {
        InputStructure inputs = new InputStructure();

        inputs.moveDirection = _moveDirection;
        inputs.isJumping = _isJumping;

        _isJumping = false;

        return inputs;
    }

}
