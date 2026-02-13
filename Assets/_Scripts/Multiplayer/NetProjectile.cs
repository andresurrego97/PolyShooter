using Fusion;
using UnityEngine;

public class NetProjectile : NetworkBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 2;
    [SerializeField] private float speed = 5;

    [Networked] private TickTimer Life { get; set; }

    private Transform parent;

    public void Init()
    {
        Life = TickTimer.CreateFromSeconds(Runner, lifetime);
    }

    public override void FixedUpdateNetwork()
    {
        if (Life.Expired(Runner))
        {
            Runner.Despawn(Object);
            return;
        }

        transform.position += speed * Runner.DeltaTime * transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        parent = collision.transform.parent;

        if (parent != null && parent.TryGetComponent(out NetPlayerProperties properties))
            properties.DealDamageRpc(damage);

        if (Object != null)
            Runner.Despawn(Object);
    }
}