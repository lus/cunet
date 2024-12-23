using System.Text.Json;
using Cunet.Protocol.Packets.Status.Server;
using Cunet.Protocol.Serialization;
using JetBrains.Annotations;
using String = Cunet.Protocol.Primitive.String;

namespace Cunet.Protocol.Packets.Status.Client;

/// <summary>
///     Represents the client-bound status response packet sent by the server in response to the
///     <see cref="StatusRequestPacket" />. It provides the data shown in the server list.
/// </summary>
public readonly struct StatusResponsePacket : IClientBoundPacket<StatusResponsePacket> {
    private static readonly JsonSerializerOptions SerializerOptions = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    ///     The status data of the server.
    /// </summary>
    public required ResponseData Data { get; init; }

    public int CalculateSize() {
        return new String(JsonSerializer.Serialize(Data, SerializerOptions)).CalculateSize();
    }

    public int Write(Span<byte> output) {
        return new String(JsonSerializer.Serialize(Data, SerializerOptions)).Write(output);
    }

    public static StatusResponsePacket Read(ReadOnlySpan<byte> input, out int consumed) {
        string raw = String.Read(input, out consumed).Value;
        ResponseData? deserialized = JsonSerializer.Deserialize<ResponseData>(raw, SerializerOptions);
        if (deserialized == null) {
            throw new ReadException("could not deserialize status response");
        }
        return new StatusResponsePacket {
            Data = deserialized,
        };
    }

    /// <summary>
    ///     Represents the data contained in the status response.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ResponseData {
        /// <summary>
        ///     The name and version of the server.
        /// </summary>
        public required VersionData Version { get; init; }

        /// <summary>
        ///     Data about the player and slot count as well as the sample players shown by the Notchian client when hovering over
        ///     the player/slot count. If this is set to <c>null</c>, the Notchian client will show <c>???</c> instead of the
        ///     player/slot count.
        /// </summary>
        public PlayersData? Players { get; init; }

        /// <summary>
        ///     The MOTD of the server.
        /// </summary>
        public DescriptionData? Description { get; init; }

        /// <summary>
        ///     The Base64-encoded icon of the server with the data prepended with <c>data:image/png;base64,</c>.
        /// </summary>
        public string? Favicon { get; init; }

        /// <summary>
        ///     Whether the server enforces the secure chat feature.
        /// </summary>
        public bool EnforcesSecureChat { get; init; }

        /// <summary>
        ///     Represents data about the name and version of the server.
        /// </summary>
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public class VersionData {
            /// <summary>
            ///     The name of the server. This is not the MOTD.
            /// </summary>
            public required string Name { get; init; }

            /// <summary>
            ///     The protocol ID the server runs on.
            /// </summary>
            public required int Protocol { get; init; }
        }

        /// <summary>
        ///     Represents data about the online/max player count and the sample of connected players.
        /// </summary>
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public class PlayersData {
            /// <summary>
            ///     The maximum amount of players.
            /// </summary>
            public required int Max { get; init; }

            /// <summary>
            ///     The amount of currently online players.
            /// </summary>
            public required int Online { get; init; }

            /// <summary>
            ///     The player sample shown by the Notchian client when hovering over the player/slot count.
            /// </summary>
            public List<SampleEntry>? Sample { get; init; }

            /// <summary>
            ///     Represents an entry of the player <see cref="PlayersData.Sample" />.
            /// </summary>
            [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
            public class SampleEntry {
                /// <summary>
                ///     The name of the player.
                /// </summary>
                public required string Name { get; init; }

                /// <summary>
                ///     The UUID of the player.
                /// </summary>
                public required Guid Id { get; init; }
            }
        }

        /// <summary>
        ///     Represents the MOTD of a server.
        /// </summary>
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public class DescriptionData {
            /// <summary>
            ///     The MOTD of the server.
            /// </summary>
            public required string Text { get; init; }
        }
    }
}
