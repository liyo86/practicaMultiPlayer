using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class T_MovementPlayer : NetworkBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        if (!IsLocalPlayer)
            return;
        float _xInput = Input.GetAxis("Horizontal");
        float _yInput = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(_xInput, _yInput);
        moveDirection.Normalize();

        if (IsServer)
            transform.Translate(moveDirection * _speed * Time.deltaTime);
        else
            MoveServerRpc(moveDirection * _speed * Time.deltaTime);


    }

    [ServerRpc]
    public void MoveServerRpc(Vector2 direction)
    {
        transform.Translate(direction);
    }

}
