using Cunet.Protocol.Serialization;
using Cunet.Protocol.Session;
using JetBrains.Annotations;

namespace Cunet.Protocol.Packets;

/// <summary>
///     Represents a packet registry mapping packet IDs to types and vice versa.
///     The <see cref="DefaultPacketRegistry" /> maps all official packets.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers | ImplicitUseTargetFlags.WithInheritors)]
public interface IPacketRegistry {
    /// <summary>
    ///     Represents a packet supplier creating packet instances.
    /// </summary>
    public delegate IPacket PacketSupplier(ReadOnlySpan<byte> input, out int consumed);

    /// <summary>
    ///     Registers a server-bound packet.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <param name="id">The ID of the packet.</param>
    /// <param name="supplier">The supplier responsible for reading the packet.</param>
    /// <typeparam name="TPacket">The type of the packet.</typeparam>
    public void RegisterServerBoundPacket<TPacket>(SessionState state, int id, PacketSupplier supplier)
        where TPacket : IServerBoundPacket;

    /// <summary>
    ///     Registers a server-bound packet implementing <see cref="IServerBoundPacket{TSelf}" />, using
    ///     <see cref="IReadableProtocolElement{TSelf}.Read" /> as its supplier.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <param name="id">The ID of the packet.</param>
    /// <typeparam name="TPacket">The type of the packet.</typeparam>
    public void RegisterServerBoundPacket<TPacket>(SessionState state, int id)
        where TPacket : IServerBoundPacket<TPacket>;

    /// <summary>
    ///     Registers a client-bound packet.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <param name="id">The ID of the packet.</param>
    /// <param name="supplier">The supplier responsible for reading the packet.</param>
    /// <typeparam name="TPacket">The type of the packet.</typeparam>
    public void RegisterClientBoundPacket<TPacket>(SessionState state, int id, PacketSupplier supplier)
        where TPacket : IClientBoundPacket;

    /// <summary>
    ///     Registers a client-bound packet implementing <see cref="IClientBoundPacket{TSelf}" />, using
    ///     <see cref="IReadableProtocolElement{TSelf}.Read" /> as its supplier.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <param name="id">The ID of the packet.</param>
    /// <typeparam name="TPacket">The type of the packet.</typeparam>
    public void RegisterClientBoundPacket<TPacket>(SessionState state, int id)
        where TPacket : IClientBoundPacket<TPacket>;

    /// <summary>
    ///     Tries to retrieve a mapped packet ID based on a server-bound packet type.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <param name="type">The type of the packet to retrieve the ID for.</param>
    /// <returns>The packet ID or <c>null</c> if none was found.</returns>
    public int? FindServerBoundPacketId(SessionState state, Type type);

    /// <summary>
    ///     Tries to retrieve a mapped packet ID based on a server-bound packet type.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <typeparam name="TPacket">The type of the packet to retrieve the ID for.</typeparam>
    /// <returns>The packet ID or <c>null</c> if none was found.</returns>
    public int? FindServerBoundPacketId<TPacket>(SessionState state) where TPacket : IServerBoundPacket;

    /// <summary>
    ///     Tries to retrieve a mapped packet supplier based on a server-bound packet ID.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <param name="id">The ID of the packet.</param>
    /// <returns>The packet supplier or <c>null</c> if none was found.</returns>
    public PacketSupplier? FindServerBoundPacketSupplier(SessionState state, int id);

    /// <summary>
    ///     Tries to retrieve a mapped packet ID based on a client-bound packet type.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <param name="type">The type of the packet to retrieve the ID for.</param>
    /// <returns>The packet ID or <c>null</c> if none was found.</returns>
    public int? FindClientBoundPacketId(SessionState state, Type type);

    /// <summary>
    ///     Tries to retrieve a mapped packet ID based on a client-bound packet type.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <typeparam name="TPacket">The type of the packet to retrieve the ID for.</typeparam>
    /// <returns>The packet ID or <c>null</c> if none was found.</returns>
    public int? FindClientBoundPacketId<TPacket>(SessionState state) where TPacket : IClientBoundPacket;

    /// <summary>
    ///     Tries to retrieve a mapped packet supplier based on a client-bound packet ID.
    /// </summary>
    /// <param name="state">The session state the packet belongs to.</param>
    /// <param name="id">The ID of the packet.</param>
    /// <returns>The packet supplier or <c>null</c> if none was found.</returns>
    public PacketSupplier? FindClientBoundPacketSupplier(SessionState state, int id);
}
