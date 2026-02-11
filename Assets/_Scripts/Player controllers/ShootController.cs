using UnityEngine;

public class ShootController : MonoBehaviour
{
    [Header("Hitscan")]
    [SerializeField] private float hitScanRange = 100f;
    [SerializeField] private Transform root;
    [SerializeField] private LayerMask layerMask;

    private readonly RaycastHit[] hit = new RaycastHit[1];

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(root.position, root.position + root.forward * hitScanRange);
    }

    //private void OnEnable()
    //{
    //    PlayerInputActions.OnFireInput += OnFire;
    //}

    //private void OnDisable()
    //{
    //    PlayerInputActions.OnFireInput -= OnFire;
    //}

    private void OnFire()
    {
        if (Physics.RaycastNonAlloc(root.position, root.forward, hit, hitScanRange, layerMask) == 1)
        {
            HitVfxHandler.Instance.SetHit(hit[0].point, hit[0].normal);
        }
    }
}