using Unity.Netcode;
using UnityEngine;

public class T_PlayerController : NetworkBehaviour
{
    [SerializeField]
    private T_NetworkMovementPlayer _player;

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 input = new Vector2(x, y);

        if (IsLocalPlayer)
            _player.ProcessLocalPlayer(input);
        else
            _player.ProcessSimulatedPlayer();

    }

}
