using Fusion;
using UnityEngine;

public class NetPlayerMovement : NetworkBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private CharacterController cc;
    [SerializeField] private Transform root;

    [Space]
    [SerializeField] private float PlayerSpeed = 2;

    private Vector3 move = Vector3.up;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            CameraComposer.instance.target = root;
        }
    }

    public override void FixedUpdateNetwork()
    {
        move = PlayerSpeed * new Vector3(
            Input.GetAxis(Inputs._Horizontal),
            0,
            Input.GetAxis(Inputs._Vertical));

        if (move != Vector3.zero)
        {
            if (!controller.enemySelected)
            {
                root.rotation = Quaternion.LookRotation(move, root.up);
            }
        }

        move.y = cc.isGrounded ? 0 : Physics.gravity.y;

        cc.Move(move * Runner.DeltaTime);
    }
}