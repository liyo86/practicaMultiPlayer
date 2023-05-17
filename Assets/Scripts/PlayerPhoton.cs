using Fusion;
using UnityEngine;

public class PlayerPhoton : NetworkBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private NetworkCharacterControllerPrototype _character;


    private void Awake()
    {
        if (!_character)
            _character = GetComponent<NetworkCharacterControllerPrototype>();

        if (!_character)
        {
            Debug.LogError("No hay character controller prototype");
            this.enabled = false;
        }

    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out InputStructure input))
        {
            Vector2 v = input.moveDirection;
            _character.Move(_speed * Runner.DeltaTime *
                v.normalized
                );

            if (input.isJumping)
            {
                _character.Jump();
            }

        }
    }

}
