using Fusion;
using UnityEngine;

public class NetLobby : MonoBehaviour
{
    public static NetLobby instance;

    [SerializeField] private int waitSeconds;

    [Networked] private TickTimer StartTimer { get; set; }

    private NetworkRunner networkRunner;

    private void Awake()
    {
        instance = this;
    }

    public void Init(NetworkRunner _runner)
    {
        networkRunner = _runner;
        if (_runner.IsSharedModeMasterClient)
        {
            StartTimer = TickTimer.CreateFromSeconds(networkRunner, waitSeconds);
        }

        // crear timer, manejar canvases, solo poner el boton de iniciar con este, y al iniciar, mandar RPC a todos
    }

    private void Update()
    {
        if (StartTimer.IsRunning)
        {
            Debug.LogWarning($"StartTimer({StartTimer} Tick({networkRunner.Tick})");
        }

        if (StartTimer.Expired(networkRunner))
        {
            Debug.LogWarning("fin");
        }
    }
}