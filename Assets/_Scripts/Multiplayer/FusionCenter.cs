using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FusionCenter : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner networkRunner;

    [Space]
    [SerializeField] private GameObject playerPrefab;

    private Teams team = Teams.None;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"<b><color=white>OnPlayerJoined master({runner.IsSharedModeMasterClient})");

        NetLobby.instance.Init(runner);

        if (player == networkRunner.LocalPlayer)
        {
            networkRunner.Spawn(
                playerPrefab,
                Vector3.zero,
                Quaternion.identity,
                null,
                (netRunner, netObject) =>
                {
                    team = ((player.AsIndex & 1) == 0) ? Teams.TeamB : Teams.TeamA;
                    netObject.GetComponent<NetPlayerController>().Init(team);

                    switch (team)
                    {
                        case Teams.TeamA:
                            netObject.transform.GetChild(0).position = new((player.AsIndex * 3) - 7, 1, 0);
                            break;

                        case Teams.TeamB:
                            netObject.transform.GetChild(0).position = new((player.AsIndex * 3) - 7, 1, 27);
                            break;
                    }
                });
        }
    }

#pragma warning disable UNT0006 // Incorrect message signature
    public void OnConnectedToServer(NetworkRunner runner)
#pragma warning restore UNT0006 // Incorrect message signature
    {
        Debug.Log("<b><color=white>OnConnectedToServer");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("<b><color=white>OnConnectFailed");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("<b><color=white>OnConnectRequest");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("<b><color=white>OnCustomAuthenticationResponse");
    }

#pragma warning disable UNT0006 // Incorrect message signature
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
#pragma warning restore UNT0006 // Incorrect message signature
    {
        Debug.Log("<b><color=white>OnDisconnectedFromServer");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("<b><color=white>OnHostMigration");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //Debug.Log("<b><color=white>OnInput"); //update inputs
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("<b><color=white>OnInputMissing");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        //Debug.Log("<b><color=white>OnObjectEnterAOI"); //spawn proyectile
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("<b><color=white>OnObjectExitAOI");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("<b><color=white>OnPlayerLeft");
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        Debug.Log("<b><color=white>OnReliableDataProgress");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        Debug.Log("<b><color=white>OnReliableDataReceived");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("<b><color=white>OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("<b><color=white>OnSceneLoadStart");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("<b><color=white>OnSessionListUpdated");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("<b><color=white>OnShutdown");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("<b><color=white>OnUserSimulationMessage");
    }
}