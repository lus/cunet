using Cunet.Protocol.Packets.Handshaking.Server;
using Cunet.Protocol.Packets.Status.Client;
using Cunet.Protocol.Packets.Status.Server;
using Cunet.Protocol.Session;
using Cunet.Protocol.Util;

namespace Cunet.Protocol.Packets;

/// <summary>
///     Represents a <see cref="IPacketRegistry" /> mapping all official packets.
/// </summary>
public class DefaultPacketRegistry : IPacketRegistry {
    private Dictionary<SessionState, Dictionary<Type, int>> _serverBoundPacketIds = new();

    private Dictionary<SessionState, Dictionary<int, IPacketRegistry.PacketSupplier>> _serverBoundPacketSuppliers =
        new();

    private Dictionary<SessionState, Dictionary<Type, int>> _clientBoundPacketIds = new();

    private Dictionary<SessionState, Dictionary<int, IPacketRegistry.PacketSupplier>> _clientBoundPacketSuppliers =
        new();

    public DefaultPacketRegistry() {
        RegisterDefaultPackets();
    }

    public void RegisterServerBoundPacket<TPacket>(
        SessionState state,
        int id,
        IPacketRegistry.PacketSupplier supplier
    ) where TPacket : IServerBoundPacket {
        Dictionary<Type, int> idMap = _serverBoundPacketIds.GetOrSet(state, _ => new Dictionary<Type, int>());
        idMap[typeof(TPacket)] = id;

        Dictionary<int, IPacketRegistry.PacketSupplier> supplierMap = _serverBoundPacketSuppliers.GetOrSet(
            state,
            _ => new Dictionary<int, IPacketRegistry.PacketSupplier>()
        );
        supplierMap[id] = (ReadOnlySpan<byte> input, out int consumed) => supplier(input, out consumed);
    }

    public void RegisterServerBoundPacket<TPacket>(SessionState state, int id)
        where TPacket : IServerBoundPacket<TPacket> {
        RegisterServerBoundPacket<TPacket>(
            state,
            id,
            (ReadOnlySpan<byte> input, out int consumed) => TPacket.Read(input, out consumed)
        );
    }

    public void RegisterClientBoundPacket<TPacket>(
        SessionState state,
        int id,
        IPacketRegistry.PacketSupplier supplier
    ) where TPacket : IClientBoundPacket {
        Dictionary<Type, int> idMap = _clientBoundPacketIds.GetOrSet(state, _ => new Dictionary<Type, int>());
        idMap[typeof(TPacket)] = id;

        Dictionary<int, IPacketRegistry.PacketSupplier> supplierMap = _clientBoundPacketSuppliers.GetOrSet(
            state,
            _ => new Dictionary<int, IPacketRegistry.PacketSupplier>()
        );
        supplierMap[id] = (ReadOnlySpan<byte> input, out int consumed) => supplier(input, out consumed);
    }

    public void RegisterClientBoundPacket<TPacket>(SessionState state, int id)
        where TPacket : IClientBoundPacket<TPacket> {
        RegisterClientBoundPacket<TPacket>(
            state,
            id,
            (ReadOnlySpan<byte> input, out int consumed) => TPacket.Read(input, out consumed)
        );
    }

    public int? FindServerBoundPacketId(SessionState state, Type type) {
        return _serverBoundPacketIds.GetValueOrDefault(state)?.GetValueOrDefault(type);
    }

    public int? FindServerBoundPacketId<TPacket>(SessionState state) where TPacket : IServerBoundPacket {
        return FindServerBoundPacketId(state, typeof(TPacket));
    }

    public IPacketRegistry.PacketSupplier?
        FindServerBoundPacketSupplier(SessionState state, int id) {
        return _serverBoundPacketSuppliers.GetValueOrDefault(state)?.GetValueOrDefault(id);
    }

    public int? FindClientBoundPacketId(SessionState state, Type type) {
        return _clientBoundPacketIds.GetValueOrDefault(state)?.GetValueOrDefault(type);
    }

    public int? FindClientBoundPacketId<TPacket>(SessionState state) where TPacket : IClientBoundPacket {
        return FindClientBoundPacketId(state, typeof(TPacket));
    }

    public IPacketRegistry.PacketSupplier?
        FindClientBoundPacketSupplier(SessionState state, int id) {
        return _clientBoundPacketSuppliers.GetValueOrDefault(state)?.GetValueOrDefault(id);
    }

    private void RegisterDefaultPackets() {
        RegisterServerBoundPacket<HandshakePacket>(SessionState.Handshaking, 0x00);
        RegisterServerBoundPacket<StatusRequestPacket>(SessionState.Status, 0x00);
        RegisterServerBoundPacket<PingRequestPacket>(SessionState.Status, 0x01);

        RegisterClientBoundPacket<StatusResponsePacket>(SessionState.Status, 0x00);
        RegisterClientBoundPacket<PongResponsePacket>(SessionState.Status, 0x01);
    }
}
