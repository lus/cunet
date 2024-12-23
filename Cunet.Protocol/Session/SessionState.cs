using JetBrains.Annotations;

namespace Cunet.Protocol.Session;

/// <summary>
///     Represents the state a player connection is in.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public enum SessionState {
    Handshaking,
    Status,
    Login,
    Configuration,
    Play,
}
