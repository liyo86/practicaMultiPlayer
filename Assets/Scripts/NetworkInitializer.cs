using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkInitializer : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("References")]
    [SerializeField]
    private NetworkPrefabRef _playerPrefab;
    [SerializeField]
    private CanvasGroup _buttonCG;

    private Dictionary<PlayerRef, NetworkObject> _players = new Dictionary<PlayerRef, NetworkObject>();

    private NetworkRunner _runner;


    #region NETWORK_INTERFACE
    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new InputStructure();
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        data.moveDirection = new Vector2(xMove, yMove);
        data.isJumping = Input.GetKeyDown(KeyCode.Space);

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            NetworkObject netObj = runner.Spawn(
                _playerPrefab,
                new Vector3(
                    UnityEngine.Random.Range(-3, 3),
                    UnityEngine.Random.Range(-3, 3),
                    0f),
                Quaternion.identity,
                player
                );
            _players.Add(player, netObj);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_players.TryGetValue(player, out NetworkObject netObj))
        {
            runner.Despawn(netObj);
            _players.Remove(player);
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    #endregion

    public void StartHost()
    {
        StartGame(GameMode.Host);
    }

    public void StartClient()
    {
        StartGame(GameMode.Client);
    }

    private async void StartGame(GameMode mode)
    {
        _buttonCG.interactable = false;

        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        }
        );

        _buttonCG.alpha = 0f;
    }

}
