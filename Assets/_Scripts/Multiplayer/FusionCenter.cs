using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FusionCenter : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner networkRunner;
    [SerializeField] private NetworkSceneManagerDefault networkSceneManager;

    [Space]
    [SerializeField] private NetworkPrefabRef playerPrefab;
    private readonly Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new();

    private NetInputData data = new();

    private bool _fire1;
    private bool _jump;

    private const string _Jump = "Jump";
    private const string _Vertical = "Vertical";
    private const string _Horizontal = "Horizontal";

    public void StartHost()
    {
        StartGame(GameMode.Host).Forget();
    }
    public void StartJoin()
    {
        StartGame(GameMode.Client).Forget();
    }

    public async UniTaskVoid StartGame(GameMode mode)
    {
        // Let the Fusion runner know that we will be providing user input
        networkRunner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        SceneRef scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        NetworkSceneInfo sceneInfo = new();

        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = networkSceneManager
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new(player.RawEncoded % runner.Config.Simulation.PlayerCount * 2 - 7, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

            // Keep track of the player avatars for easy access
            spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    }

    private void Update()
    {
        _fire1 |= Input.GetMouseButton(0);
        _jump |= Input.GetButton(_Jump);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        data.vertical = Input.GetAxis(_Vertical);
        data.horizontal = Input.GetAxis(_Horizontal);

        data.buttons.Set(NetInputData.Fire1, _fire1);
        data.buttons.Set(NetInputData.Jump, _jump);

        _fire1 = false;
        _jump = false;

        input.Set(data);
    }

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

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
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
}