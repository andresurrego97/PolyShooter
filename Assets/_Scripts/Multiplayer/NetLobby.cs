using Fusion;
using UnityEngine;

public class NetLobby : MonoBehaviour
{
    public static NetLobby instance;

    private NetworkRunner networkRunner;

    public void Init(NetworkRunner _runner)
    {
        Debug.LogWarning($"Init, IsSharedModeMasterClient: {_runner.IsSharedModeMasterClient}");
        networkRunner = _runner;

        // crear timer, manejar canvases, solo poner el boton de iniciar con este, y al iniciar, mandar RPC a todos
    }
}