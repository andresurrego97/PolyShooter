using Fusion;
using UnityEngine;

public class NetPlayerActions : NetworkBehaviour
{
    [SerializeField] private Transform root;

    [Space]
    [SerializeField] private int damage = 10;

    private Ray ray;
    private RaycastHit hit;
    private NetPlayerProperties properties;

    private void Update()
    {
        if (!HasStateAuthority)
            return;

        ray = new(root.position + root.forward, root.forward);

        if (Input.GetButtonDown(Inputs._Fire1))
        {
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

            if (Physics.Raycast(ray, out hit, 10))
            {
                // filtrar por compare Tag

                if (hit.collider.isTrigger && hit.transform.TryGetComponent(out properties))
                {
                    properties.DealDamageRpc(damage);
                }
            }
        }
    }
}