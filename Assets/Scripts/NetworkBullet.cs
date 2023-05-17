using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkBullet : NetworkBehaviour
{
    [SerializeField] private float _bulletSpeed = 5f;
    [SerializeField] private float _timeLiving = 5f;

    private Vector3 _direction;

    private TickTimer _lifeTime;

    public override void Spawned()
    {
        _lifeTime = TickTimer.CreateFromSeconds(Runner, _timeLiving);
    }

    public void Init()
    {
        _lifeTime = TickTimer.CreateFromSeconds(Runner, _timeLiving);
        _direction = transform.position.x >= 0f ? -transform.right : transform.right;
    }

    public override void FixedUpdateNetwork()
    {
        if (_lifeTime.ExpiredOrNotRunning(Runner))
        {
            Runner.Despawn(Object);
            return;
        }

        transform.Translate(_direction * _bulletSpeed * Runner.DeltaTime);
    }
}
