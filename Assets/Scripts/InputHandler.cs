using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Vector2 _moveDirection;
    private bool _isJumping;
    private bool _isShooting;

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _moveDirection = new Vector2(x, y);

        _isJumping = _isJumping | Input.GetKeyDown(KeyCode.Space);

        _isShooting = _isShooting | Input.GetMouseButtonDown(0);
    }

    public InputStructure GetInputs()
    {
        InputStructure inputs = new InputStructure();

        inputs.moveDirection = _moveDirection;
        inputs.isJumping = _isJumping;
        inputs.isShooting = _isShooting;

        _isJumping = false;
        _isShooting = false;

        return inputs;
    }

}
