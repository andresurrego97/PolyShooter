using Fusion;
using UnityEngine;

public class NetPhysxBall : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;

    [Networked] private TickTimer Life { get; set; }

    public void Init(Vector3 forward)
    {
        Life = TickTimer.CreateFromSeconds(Runner, 5.0f);
        rb.linearVelocity = forward;
    }

    public override void FixedUpdateNetwork()
    {
        if (Life.Expired(Runner))
            Runner.Despawn(Object);
    }
}