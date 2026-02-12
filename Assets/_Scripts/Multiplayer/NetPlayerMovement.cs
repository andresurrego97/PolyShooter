using Fusion;
using UnityEngine;

public class NetPlayerMovement : NetworkBehaviour
{
    [SerializeField] private Transform root;

    [Space]
    [SerializeField] private float PlayerSpeed = 2f;

    private Vector3 move;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            CameraComposer.instance.target = root;
        }
    }

    public override void FixedUpdateNetwork()
    {
        move = PlayerSpeed * Runner.DeltaTime * new Vector3(Input.GetAxis(Inputs._Horizontal), 0, Input.GetAxis(Inputs._Vertical));

        if (move != Vector3.zero)
        {
            root.forward = move;
            root.Translate(move * Runner.DeltaTime, Space.World);
        }
    }
}