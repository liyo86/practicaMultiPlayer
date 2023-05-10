using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class T_NetworkMovementPlayer : NetworkBehaviour
{
    private const int BUFFER_SIZE = 1024;

    [Header("References")]
    [SerializeField]
    private CharacterController _cc;

    [Header("Parameters")]
    [SerializeField]
    private float _speed;

    [SerializeField]
    private T_TransformState[] _states = new T_TransformState[BUFFER_SIZE];
    [SerializeField]
    private T_InputData[] _inputs = new T_InputData[BUFFER_SIZE];

    private NetworkVariable<T_TransformState> ServerTransformState = new NetworkVariable<T_TransformState>();
    private T_TransformState _previousTransformState;

    private int _tick;
    private float _tickTimer;
    private float _tickRateTime = 1 / 30f;

    private void Awake()
    {
        if (!_cc)
            _cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        ServerTransformState.OnValueChanged += OnServerTransformChanged;
    }

    private void OnServerTransformChanged(T_TransformState previousValue, T_TransformState newValue)
    {
        _previousTransformState = ServerTransformState.Value;
    }

    public void ProcessSimulatedPlayer()
    {
        _tickTimer += Time.deltaTime;
        if (_tickTimer > _tickRateTime)
        {
            if (ServerTransformState.Value.HasStartedMoving)
            {
                transform.position = ServerTransformState.Value.Position;
                transform.rotation = ServerTransformState.Value.Rotation;
            }

            _tick++;
            _tickTimer -= _tickRateTime;
        }
    }

    public void ProcessLocalPlayer(Vector2 moveInput) // Cambiar por inputData quizá
    {
        _tickTimer += Time.deltaTime;
        if (_tickTimer > _tickRateTime)
        {
            int bufferIndex = _tick % BUFFER_SIZE;


            if (IsServer)
            {
                MovePlayer(moveInput);

                T_TransformState currentTransformState = new T_TransformState()
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
                MovePlayerServerRpc(moveInput, _tick);
            }

            T_TransformState currentState = new T_TransformState()
            {
                Tick = _tick,
                Position = transform.position,
                Rotation = transform.rotation,
                HasStartedMoving = true
            };

            T_InputData input = new T_InputData()
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
        _cc.Move(new Vector3(input.x, input.y, 0) * _speed * _tickRateTime);
    }

    [ServerRpc]
    private void MovePlayerServerRpc(Vector2 input, int tick)
    {
        _cc.Move(new Vector3(input.x, input.y, 0) * _speed * _tickRateTime);

        T_TransformState serverTransformState = new T_TransformState()
        {
            Tick = tick,
            Position = transform.position,
            Rotation = transform.rotation,
            HasStartedMoving = true
        };

        _previousTransformState = ServerTransformState.Value;
        ServerTransformState.Value = serverTransformState;
    }


}
