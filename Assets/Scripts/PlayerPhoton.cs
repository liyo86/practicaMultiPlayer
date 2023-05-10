using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhoton : NetworkBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    private NetworkCharacterControllerPrototype _character;


    private void Awake()
    {
        _character = GetComponent<NetworkCharacterControllerPrototype>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out InputStructure input))
        {
            input.moveDirection.Normalize();
            _character.Move(_speed * new Vector2(input.moveDirection.x, input.moveDirection.y) * Runner.DeltaTime);
        }
    }

}
