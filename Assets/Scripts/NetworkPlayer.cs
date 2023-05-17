using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer local;

    [SerializeField]
    private InputHandler _handler;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
            local = this;
    }
}
