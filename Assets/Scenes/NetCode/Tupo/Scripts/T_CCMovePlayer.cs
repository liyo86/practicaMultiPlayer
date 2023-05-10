using Unity.Netcode;
using UnityEngine;

public class T_CCMovePlayer : NetworkBehaviour
{
    [Header("References")]
    [SerializeField]
    private CharacterController _cc;

    [Header("Parameters")]
    [SerializeField]
    private float _speed = 3f;


    private void Awake()
    {
        if (!_cc)
            _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!IsLocalPlayer)
            return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 moveInput = new Vector2(x, y);

        if (IsServer)
            MovePlayer(moveInput);
        else
            MovePlayerServerRpc(moveInput);
    }

    private void MovePlayer(Vector2 input)
    {
        _cc.Move(new Vector3(input.x, input.y, 0) * _speed * Time.deltaTime);
    }

    [ServerRpc]
    private void MovePlayerServerRpc(Vector2 input)
    {
        _cc.Move(new Vector3(input.x, input.y, 0) * _speed * Time.deltaTime);
    }


}
