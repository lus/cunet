namespace Cunet.Protocol.Serialization;

/// <summary>
///     Occurs if a <see cref="IWritableProtocolElement" /> could not be written.
/// </summary>
/// <param name="message">An optional message.</param>
/// <param name="cause">An optional inner exception.</param>
public class WriteException(string? message = null, Exception? cause = null) : Exception(message, cause);
