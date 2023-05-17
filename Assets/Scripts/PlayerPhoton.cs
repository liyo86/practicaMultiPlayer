using Fusion;
using UnityEngine;

public class PlayerPhoton : NetworkBehaviour
{
    public const int MAX_LIFES = 8;
    
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private NetworkCharacterControllerPrototype _character;
    
    [SerializeField]
    private NetworkBullet _bulletPrefab;

    [SerializeField] [Networked] private int _lives { get; set; }

    public static void OnLifeChanged(Changed<PlayerPhoton> changed)
    {
        if (changed.Behaviour._lives <= 0)
        {
            changed.Behaviour.Die();
        }
    }

    public void Die()
    {
        Debug.Log("A tomar por culo");
    }
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

    public override void Spawned()
    {
        _lives = MAX_LIFES;
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
                _lives--;
            }

            Vector3 _lookDirection = new Vector2(0, 0);
            Vector3 _lookTransform = transform.position;

            if (transform.position.x > 0f)
            {
                _lookDirection = Vector2.left;
            }
            else
            {
                _lookDirection = Vector2.right;
            }
                
            
            if (input.isShooting)
            {
                Runner.Spawn(_bulletPrefab, transform.position + (_lookDirection) * 0.5f, 
                    Quaternion.LookRotation(_lookDirection),
                    Object.InputAuthority,
                    (runner, o) =>
                    {
                        o.GetComponent<NetworkBullet>().Init();
                    });
            }

        }
    }

}
