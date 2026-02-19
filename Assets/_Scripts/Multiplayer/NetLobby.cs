using Fusion;
using TMPro;
using UnityEngine;

public class NetLobby : NetworkBehaviour
{
    public static bool readyLobby = false;

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
        FusionCenter.OnPlayerJoin += PlayerJoin;

        canvasPreGame.SetActive(false);
        timerText.text = "--";

        canvasInGame.SetActive(false);
    }

    private void OnDestroy()
    {
        FusionCenter.OnPlayerJoin -= PlayerJoin;
    }

    public void Init(NetworkRunner _runner)
    {
        networkRunner = _runner;
        canvasPreGame.SetActive(true);
    }

    private void PlayerJoin(NetPlayerController controller)
    {
        switch (controller.Team)
        {
            case Teams.TeamA:
                team = Instantiate(teamA, teamA.transform.parent);
                team.GetComponentInChildren<TextMeshProUGUI>().text = controller.NetName;
                break;

            case Teams.TeamB:
                team = Instantiate(teamB, teamB.transform.parent);
                team.GetComponentInChildren<TextMeshProUGUI>().text = controller.NetName;
                break;
        }

        team.SetActive(true);
    }

    public override void Spawned()
    {
        spawned = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (!timerCreated)
        {
            if (networkRunner.IsSharedModeMasterClient)
            {
                StartTimer = TickTimer.CreateFromSeconds(networkRunner, waitSeconds);
            }

            timerCreated = true;
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
                (((int)StartTimer.TargetTick - networkRunner.Tick) / networkRunner.TickRate)+ 1);
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
                readyLobby = true;

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