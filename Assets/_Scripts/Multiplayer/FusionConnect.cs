using Cysharp.Threading.Tasks;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FusionConnect : MonoBehaviour
{
    public static FusionConnect instance;

    [SerializeField] private GameObject runner;
    private GameObject _runner;

    [Space]
    private NetworkRunner networkRunner;
    private NetworkSceneManagerDefault networkSceneManager;
    private FusionCenter center;

    [Header("Connection")]
    [SerializeField] private GameObject canvasConnection;
    [SerializeField] private GameObject canvasLoading;

    [Space]
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField roomName;

    private void Awake()
    {
        instance = this;

        canvasConnection.SetActive(true);
        canvasLoading.SetActive(false);

        InstantiateRunner();
    }

    private void InstantiateRunner()
    {
        _runner = Instantiate(runner);
        networkRunner = _runner.GetComponent<NetworkRunner>();
        networkSceneManager = _runner.GetComponent<NetworkSceneManagerDefault>();
        center = _runner.GetComponent<FusionCenter>();
    }

    public void StartRoom()
    {
        Internal_StartRoom().Forget();
    }

    private async UniTaskVoid Internal_StartRoom()
    {
        networkRunner.ProvideInput = true;

        SceneRef scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

        if (scene.IsValid)
        {
            new NetworkSceneInfo().AddSceneRef(scene, LoadSceneMode.Additive);
        }

        center.playername = userName.text;

        canvasConnection.SetActive(false);
        canvasLoading.SetActive(true);

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = roomName.text,
            Scene = scene,
            SceneManager = networkSceneManager
        });

        canvasLoading.SetActive(false);
    }

    public void CloseRoom()
    {
        networkRunner.Shutdown();
        canvasConnection.SetActive(true);

        InstantiateRunner();
    }
}