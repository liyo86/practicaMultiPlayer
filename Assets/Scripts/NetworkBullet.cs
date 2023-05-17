using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkBullet : NetworkBehaviour
{
    [SerializeField] private float _bulletSpeed = 5f;
    [SerializeField] private float _timeLiving = 5f;
    
    private TickTimer _lifeTime;

    public override void Spawned()
    {
        _lifeTime = TickTimer.CreateFromSeconds(Runner, _timeLiving);
    }

    public void Init()
    {
        _lifeTime = TickTimer.CreateFromSeconds(Runner, _timeLiving);
    }

    public override void FixedUpdateNetwork()
    {
        if (_lifeTime.ExpiredOrNotRunning(Runner))
        {
            Runner.Despawn(Object);
            return;
        }

        Vector3 direction = transform.position.x >= 0f ? -transform.right : transform.right;
            
        transform.Translate(direction * _bulletSpeed * Runner.DeltaTime);
    }
}
