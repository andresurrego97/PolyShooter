using Fusion;
using UnityEngine;

public class NetPlayerMovement : NetworkBehaviour
{
    [SerializeField] private NetPlayerController controller;
    [SerializeField] private CharacterController cc;
    [SerializeField] private Transform root;

    [Space]
    [SerializeField] private float PlayerSpeed = 2;

    private Vector2 inputs = Vector2.zero;
    private Vector3 move = Vector3.up;

    private void Awake()
    {
        cc.enabled = false;
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
            CameraComposer.instance.Init(root, controller.Team);

        cc.enabled = true;
    }

    public override void FixedUpdateNetwork()
    {
        //if (!NetLobby.readyLobby)
        //    return;

        switch (controller.Team)
        {
            case Teams.TeamA:
                inputs.Set(Input.GetAxis(Inputs._Horizontal), Input.GetAxis(Inputs._Vertical));
                break;
            case Teams.TeamB:
                inputs.Set(-Input.GetAxis(Inputs._Horizontal), -Input.GetAxis(Inputs._Vertical));
                break;
        }

        move = PlayerSpeed * new Vector3(inputs.x, 0, inputs.y);

        if (!controller.enemySelected && move != Vector3.zero)
        {
            root.rotation = Quaternion.LookRotation(move, root.up);
        }

        move.y = cc.isGrounded ? 0 : Physics.gravity.y;

        if (!NetLobby.instance.Ready)
            return;

        cc.Move(move * Runner.DeltaTime);
    }
}