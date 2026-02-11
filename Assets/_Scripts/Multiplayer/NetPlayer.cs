using Fusion;
using UnityEngine;

public class NetPlayer : NetworkBehaviour
{
    [SerializeField] private NetworkCharacterController netCharacterController;

    [Space]
    [SerializeField] private MeshRenderer meshRenderer;
    private Material material;

    [Space]
    [SerializeField] private NetBall netBall;
    [SerializeField] private NetPhysxBall netPhysxBall;
    [SerializeField] private Transform ballRoot;

    private Vector3 direction = Vector3.zero;
    private NetworkButtons lastButtons;

    private ChangeDetector changeDetector;
    [Networked] public bool SpawnedProjectile { get; set; }

    private void Awake()
    {
        material = Instantiate(meshRenderer.material);
        material.color = Color.grey;
        meshRenderer.material = material;
    }

    public override void Spawned()
    {
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

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

                SpawnedProjectile = !SpawnedProjectile;
            }
            else if (data.buttons.WasPressed(lastButtons, NetInputData.Fire2))
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

                SpawnedProjectile = !SpawnedProjectile;
            }

            lastButtons = data.buttons;
        }
    }

    public override void Render()
    {
        foreach (string change in changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(SpawnedProjectile):
                    material.color = Color.white;
                    break;
            }
        }

        material.color = Color.Lerp(material.color, Color.grey, Time.deltaTime * 5);
    }
}