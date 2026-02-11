using Fusion;

public struct NetInputData : INetworkInput
{
    public float vertical;
    public float horizontal;

    public const byte Fire1 = 0;
    public const byte Jump = 1;

    public NetworkButtons buttons;
}