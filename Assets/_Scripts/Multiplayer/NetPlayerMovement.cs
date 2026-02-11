using Fusion;
using UnityEngine;

public class NetPlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController _controller;

    [Space]
    [SerializeField] private float PlayerSpeed = 2f;
    [SerializeField] private float JumpForce = 5f;
    //[SerializeField] private float GravityValue = -9.81f;

    private bool _jumpPressed;
    private Vector3 _velocity;
    private Vector3 move;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            CameraComposer.instance.target = transform;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        // FixedUpdateNetwork is only executed on the StateAuthority

        if (_controller.isGrounded)
        {
            _velocity = new Vector3(0, -1, 0);
        }

        move = PlayerSpeed * Runner.DeltaTime * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        _velocity.y += /*GravityValue*/ Physics.gravity.y * Runner.DeltaTime;
        if (_jumpPressed && _controller.isGrounded)
        {
            _velocity.y += JumpForce;
        }
        _controller.Move(move + _velocity * Runner.DeltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        _jumpPressed = false;
    }
}