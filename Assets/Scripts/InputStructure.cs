using Fusion;
using UnityEngine;

public struct InputStructure : INetworkInput
{
    public Vector2 moveDirection;
    public NetworkBool isJumping;
}
