using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetLobby : NetworkBehaviour
{
    public static NetLobby instance;

    public bool Ready => spawned && ReadyLobby;
    [Networked] private bool ReadyLobby { get; set; }

    public List<NetPlayerController> players = new();
    [Networked] public int teamA_count { get; set; } = 0;
    [Networked] public int teamB_count { get; set; } = 0;

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
    private bool spawned = false;
    private NetworkRunner networkRunner;
    private int prevSecondsLeft = -2;
    private int secondsLeft = -1;

    private void Awake()
    {
        instance = this;

        canvasPreGame.SetActive(false);
        timerText.text = "--";

        canvasInGame.SetActive(false);
    }

    public void Init(NetworkRunner _runner)
    {
        networkRunner = _runner;
        Debug.LogWarning("Init");
    }

    public void PlayerSpawn(NetPlayerController controller, out GameObject preGameIcon)
    {
        preGameIcon = null;
        players.Add(controller);

        switch (controller.Team)
        {
            case Teams.TeamA:
                teamA_count += 1;

                team = Instantiate(teamA, teamA.transform.parent);
                team.GetComponentInChildren<TextMeshProUGUI>().text = controller.NetName;
                preGameIcon = team;
                break;

            case Teams.TeamB:
                teamB_count += 1;

                team = Instantiate(teamB, teamB.transform.parent);
                team.GetComponentInChildren<TextMeshProUGUI>().text = controller.NetName;
                preGameIcon = team;
                break;
        }

        team.SetActive(true);
    }

    public void PlayerDespawn(NetPlayerController netPlayerController)
    {
        players.Remove(netPlayerController);
    }

    public override void Spawned()
    {
        spawned = true;

        if (!StartTimer.Expired(networkRunner))
        {
            canvasPreGame.SetActive(true);
        }
        else
        {
            canvasInGame.SetActive(true);
        }

        Debug.LogWarning("Spawned");
    }

    public override void FixedUpdateNetwork()
    {
        if (networkRunner.IsSharedModeMasterClient && !timerCreated)
        {
            StartTimer = TickTimer.CreateFromSeconds(networkRunner, waitSeconds);

            timerCreated = true;
            Debug.LogWarning("timerCreated");
        }
    }

    private void Update()
    {
        if (!spawned)
            return;

        if (StartTimer.Expired(networkRunner))
        {
            OnChangeSeconds(0);
            return;
        }

        if (StartTimer.IsRunning)
        {
            OnChangeSeconds(
                (((int)StartTimer.TargetTick - networkRunner.Tick) / networkRunner.TickRate) + 1);
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