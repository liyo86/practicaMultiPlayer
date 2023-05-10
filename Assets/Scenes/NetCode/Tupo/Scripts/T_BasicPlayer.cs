using UnityEngine;
using Unity.Netcode;
using System;

public class T_BasicPlayer : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        Move();
    }

    private void Move()
    {
        if (IsOwner)
        {
            if (IsServer)
            {
                Vector3 position = new Vector2(
                    UnityEngine.Random.Range(-3, 3),
                    UnityEngine.Random.Range(-3, 3)
                    );
                transform.position = position;
            }
            else if (IsServer)
            {
                MoveRandomServerRpc();
            }
        }

    }

    [ServerRpc]
    public void MoveRandomServerRpc()
    {
        Vector3 position = new Vector2(
            UnityEngine.Random.Range(-3, 3),
            UnityEngine.Random.Range(-3, 3)
            );
        transform.position = position;
    }
}
