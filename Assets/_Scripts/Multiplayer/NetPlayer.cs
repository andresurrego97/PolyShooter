using Fusion;
using UnityEngine;

public class NetPlayer : NetworkBehaviour
{
    [SerializeField] private NetworkCharacterController netCharacterController;

    [Space]
    [SerializeField] private NetBall netBall;
    [SerializeField] private NetPhysxBall netPhysxBall;
    [SerializeField] private Transform ballRoot;

    private Vector3 direction = Vector3.zero;
    private NetworkButtons lastButtons;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetInputData data))
        {
            direction.x = data.horizontal;
            direction.z = data.vertical;

            netCharacterController.Move(5 * Runner.DeltaTime * direction);

            //if (data.buttons.WasPressed(lastButtons, NetInputData.Fire1)) // Fire1 Pressed
            //if (data.buttons.IsSet(NetInputData.Fire1)) // Fire1 Set
            //if (data.buttons.WasReleased(lastButtons, NetInputData.Fire1)) // Fire1 Released

            if (data.buttons.WasPressed(lastButtons, NetInputData.Jump))
            {
                netCharacterController.Jump();
            }

            if (!HasStateAuthority)
                return;

            if (data.buttons.WasPressed(lastButtons, NetInputData.Fire1))
            {
                Runner.Spawn(
                    netBall,
                    ballRoot.position,
                    Quaternion.LookRotation(ballRoot.forward, ballRoot.up),
                    Object.InputAuthority,
                    (runner, o) =>
                    {
                        // Initialize the Ball before synchronizing it
                        o.GetComponent<NetBall>().Init();
                    });
            }

            if (data.buttons.WasPressed(lastButtons, NetInputData.Fire2))
            {
                Runner.Spawn(
                    netPhysxBall,
                    ballRoot.position,
                    Quaternion.LookRotation(ballRoot.forward, ballRoot.up),
                    Object.InputAuthority,
                    (runner, o) =>
                    {
                        // Initialize the Ball before synchronizing it
                        o.GetComponent<NetPhysxBall>().Init(10 * ballRoot.forward);
                    });
            }

            lastButtons = data.buttons;
        }
    }
}