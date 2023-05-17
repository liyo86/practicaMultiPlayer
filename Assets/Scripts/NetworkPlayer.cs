using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local;

    [SerializeField]
    public InputHandler Handler;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
            Local = this;
    }
}
