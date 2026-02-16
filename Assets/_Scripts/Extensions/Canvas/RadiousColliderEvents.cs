using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class RadiousColliderEvents :  NetworkBehaviour
{
    public readonly Dictionary<Collider, PlayerController> colliders = new();

    [SerializeField] private PlayerController controller;
    [SerializeField] private Collider thisCollider;

    private PlayerController otherPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority || other == thisCollider)
            return;

        otherPlayer = other.GetComponentInParent<PlayerController>();

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