using Fusion;
using UnityEngine;

public class NetPlayerActions : NetworkBehaviour
{
    [SerializeField] private Transform root;

    [Space]
    [SerializeField] private NetProjectile projectile;
    [SerializeField] private Transform rootShoot;

    private void Update()
    {
        if (!NetLobby.instance.Ready)
            return;

        if (!HasStateAuthority)
            return;

        if (Input.GetButtonDown(Inputs._Fire1))
        {
            Runner.Spawn(
                    projectile,
                    rootShoot.position,
                    Quaternion.LookRotation(rootShoot.forward, rootShoot.up),
                    Object.InputAuthority,
                    (runner, o) =>
                    {
                        // Initialize the Ball before synchronizing it
                        o.GetComponent<NetProjectile>().Init();
                    });
        }
    }
}