using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkMovementPlayer : NetworkBehaviour
{
    private const int BUFFER_SIZE = 1024;

    [Header("References")]
    [SerializeField] private CharacterController _cc;
    
    [Header("Parameters")] 
    [SerializeField] private float _speed;

    [SerializeField] private TransformState[] _states = new TransformState[BUFFER_SIZE];
    [SerializeField] private InputData[] _inputs = new InputData[BUFFER_SIZE];

    private NetworkVariable<TransformState> ServerTransformState = new NetworkVariable<TransformState>();
    private TransformState _previousTransformState;
    
    private int _tick;
    private float _tickTimer;
    private float _tickRateTime = 1 / 30f;
    
    void Start()
    {
        ServerTransformState.OnValueChanged += OnServerTansformChanged;
    }

    private void OnServerTansformChanged(TransformState previousValue, TransformState newValue)
    {
        _previousTransformState = ServerTransformState.Value;
    }

    void Update()
    {
        
    }

    public void ProcessSimulatedPlayer()
    {
        _tickTimer += Time.deltaTime;
        if (_tickTimer > _tickRateTime)
        {
            if (ServerTransformState.Value.HasStartedMoving)
            {
                _tick = ServerTransformState.Value.Tick;
                transform.position = ServerTransformState.Value.Position;
                transform.rotation = ServerTransformState.Value.Rotation;
            }

            _tick++;
            _tickTimer -= _tickRateTime;
        }
    }
    
    public void ProcessLocalPlayer(Vector2 moveInput)
    {
        _tickTimer += Time.deltaTime;
        if (_tickTimer > _tickRateTime)
        {
            int bufferIndex = _tick % BUFFER_SIZE;
            
            if (IsServer)
            {
                MovePlayer(moveInput);

                TransformState currentTransformState = new TransformState()
                {
                    Tick = _tick,
                    Position = transform.position,
                    Rotation = transform.rotation,
                    HasStartedMoving = true
                };

                _previousTransformState = ServerTransformState.Value;
                ServerTransformState.Value = currentTransformState;
            }
            else
            {
                MovePlayer(moveInput);
                MovePlayerServerRpc(moveInput);
            }


            TransformState currentState = new TransformState()
            {
                Tick = _tick,
                Position = transform.position,
                Rotation = transform.rotation,
                HasStartedMoving = true
            };

            InputData input = new InputData()
            {
                MoveInput = moveInput
            };

            _states[bufferIndex] = currentState;
            _inputs[bufferIndex] = input;

            _tick++;
            _tickTimer -= _tickRateTime;
        }
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
