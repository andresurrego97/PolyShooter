using Fusion;
using TMPro;
using UnityEngine;

public static class NetLobbyExtensions
{
    public static bool SpawnedNetLobby()
    {
        return NetLobby.instance != null && NetLobby.instance.spawned;
    }
}

public class NetLobby : NetworkBehaviour
{
    public static NetLobby instance;

    public bool Ready => spawned && ReadyLobby;
    public bool spawned = false;
    [Networked] private bool ReadyLobby { get; set; }

    [Header("Pre Game")]
    [SerializeField] private GameObject canvasPreGame;
    [SerializeField] private int waitSeconds;

    [Space]
    [SerializeField] private GameObject teamA;
    [SerializeField] private GameObject teamB;

    [Space]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("In Game")]
    [SerializeField] private GameObject canvasInGame;

    private GameObject team;

    [Networked] private TickTimer StartTimer { get; set; }

    private bool timerCreated = false;
    private int prevSecondsLeft = -2;
    private int secondsLeft = -1;

    private void Awake()
    {
        instance = this;

        canvasPreGame.SetActive(false);
        timerText.text = "--";

        canvasInGame.SetActive(false);
    }

    public void PlayerSpawn(NetPlayerController controller, out GameObject preGameIcon)
    {
        preGameIcon = null;

        switch (controller.Team)
        {
            case Teams.TeamA:
                team = Instantiate(teamA, teamA.transform.parent);
                team.GetComponentInChildren<TextMeshProUGUI>().text = controller.NetName;
                preGameIcon = team;
                break;

            case Teams.TeamB:
                team = Instantiate(teamB, teamB.transform.parent);
                team.GetComponentInChildren<TextMeshProUGUI>().text = controller.NetName;
                preGameIcon = team;
                break;
        }

        team.SetActive(true);
    }

    public override void Spawned()
    {
        spawned = true;

        if (!StartTimer.Expired(Runner))
        {
            canvasPreGame.SetActive(true);
        }
        else
        {
            canvasInGame.SetActive(true);
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        spawned = false;
        base.Despawned(runner, hasState);
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsSharedModeMasterClient && !timerCreated)
        {
            StartTimer = TickTimer.CreateFromSeconds(Runner, waitSeconds);

            timerCreated = true;
        }
    }

    private void Update()
    {
        if (!spawned)
            return;

        if (StartTimer.Expired(Runner))
        {
            OnChangeSeconds(0);
            return;
        }

        if (StartTimer.IsRunning)
        {
            OnChangeSeconds(
                (((int)StartTimer.TargetTick - Runner.Tick) / Runner.TickRate) + 1);
        }
    }

    private void OnChangeSeconds(int ticksLeft)
    {
        secondsLeft = ticksLeft;

        if (secondsLeft != prevSecondsLeft)
        {
            UpdateText();

            if (secondsLeft == 0)
            {
                ReadyLobby = true;

                canvasPreGame.SetActive(false);
                canvasInGame.SetActive(true);
            }
        }

        prevSecondsLeft = secondsLeft;
    }

    private void UpdateText()
    {
        timerText.text = $"{secondsLeft}";
    }

    public void CloseRoom()
    {
        FusionConnect.instance.CloseRoom();
    }
}