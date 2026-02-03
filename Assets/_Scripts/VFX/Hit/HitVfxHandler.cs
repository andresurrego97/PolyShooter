using Cysharp.Threading.Tasks;
using UnityEngine;

public class HitVfxHandler : MonoBehaviour
{
    public static HitVfxHandler Instance;

    [SerializeField] private GameObject hitVfxPrefab;
    [SerializeField] private Transform hitVfxParent;
    [SerializeField] private int poolSize = 10;

    private PoolSystem pool;
    private GameObject vfx;

    private void Awake()
    {
        Instance = this;

        Init().Forget();
    }

    [ContextMenu("Test")]
    private async UniTaskVoid Init()
    {
        pool = new PoolSystem();
        await pool.Create(hitVfxPrefab, poolSize, hitVfxParent);
    }

    public void SetHit(Vector3 position, Vector3 normal)
    {
        vfx = pool.GetObject();
        vfx.transform.position = position;
        vfx.transform.up = normal;
        vfx.SetActive(true);
    }
}