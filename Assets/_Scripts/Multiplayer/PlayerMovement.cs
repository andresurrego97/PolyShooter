using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController _controller;

    [Space]
    [SerializeField] private float PlayerSpeed = 2f;

    private Vector3 move;

    // FixedUpdateNetwork is only executed on the StateAuthority
    public override void FixedUpdateNetwork()
    {
        move = PlayerSpeed * Runner.DeltaTime * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        _controller.Move(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }
}