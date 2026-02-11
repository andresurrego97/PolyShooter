using Fusion;

public struct NetInputData : INetworkInput
{
    public float vertical;
    public float horizontal;

    public const byte Fire1 = 0;
    public const byte Fire2 = 1;
    public const byte Jump = 2;

    public NetworkButtons buttons;
}