using Fusion;
//using System;
using System.Collections.Generic;
using UnityEngine;

public class RadiousColliderEvents : /*MonoBehaviour*/ NetworkBehaviour
{
    //public bool HasStateAuthority;

    //public Action<PlayerController> OnEnterPlayer;
    //public Action<PlayerController> OnExitPlayer;
    public readonly Dictionary<Collider, PlayerController> colliders = new();

    [SerializeField] private Collider thisCollider;

    private PlayerController otherPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority || other == thisCollider)
            return;

        otherPlayer = other.GetComponentInParent<PlayerController>();
        colliders.Add(other, otherPlayer);
        //OnEnterPlayer?.Invoke(otherPlayer);
        otherPlayer = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority || other == thisCollider)
            return;

        if (colliders.ContainsKey(other))
        {
            //otherPlayer = colliders[other];
            colliders.Remove(other);
            //OnExitPlayer?.Invoke(otherPlayer);
            otherPlayer = null;
        }
    }
}