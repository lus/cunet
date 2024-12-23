namespace Cunet.Protocol.Serialization;

/// <summary>
///     Occurs if a <see cref="IReadableProtocolElement{TSelf}" /> could not be read.
/// </summary>
/// <param name="message">An optional message.</param>
/// <param name="cause">An optional inner exception.</param>
public class ReadException(string? message = null, Exception? cause = null) : Exception(message, cause);
