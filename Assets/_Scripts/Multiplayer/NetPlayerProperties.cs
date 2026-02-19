using Fusion;
using System;

public class NetPlayerProperties : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(HealthChanged))] public int NetLife { get; set; } = 100;
    public Action OnLifeChange;

    private void HealthChanged()
    {
        OnLifeChange?.Invoke();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(int damage)
    {
        NetLife -= damage;
    }
}