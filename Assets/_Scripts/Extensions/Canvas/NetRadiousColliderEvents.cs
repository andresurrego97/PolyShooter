using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class NetRadiousColliderEvents :  NetworkBehaviour
{
    public readonly Dictionary<Collider, NetPlayerController> colliders = new();

    [SerializeField] private NetPlayerController controller;
    [SerializeField] private Collider thisCollider;

    private NetPlayerController otherPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority || other == thisCollider)
            return;

        otherPlayer = other.GetComponentInParent<NetPlayerController>();

        if (controller.Team != otherPlayer.Team)
            colliders.Add(other, otherPlayer);

        otherPlayer = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority || other == thisCollider)
            return;

        if (colliders.ContainsKey(other))
        {
            colliders.Remove(other);
            otherPlayer = null;
        }
    }
}