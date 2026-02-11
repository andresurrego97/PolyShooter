using Fusion;

public class NetBall : NetworkBehaviour
{
    [Networked] private TickTimer Life { get; set; }

    public void Init()
    {
        Life = TickTimer.CreateFromSeconds(Runner, 2f);
    }

    public override void FixedUpdateNetwork()
    {
        if (Life.Expired(Runner))
        {
            Runner.Despawn(Object);
            return;
        }

        transform.position += 5 * Runner.DeltaTime * transform.forward;
    }
}