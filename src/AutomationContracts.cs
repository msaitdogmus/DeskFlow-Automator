namespace DeskFlow.Portfolio;

/// <summary>Lifecycle states exposed by the processing queue.</summary>
public enum RecordState
{
    Queued,
    Processing,
    Completed,
    Failed
}

/// <summary>An immutable record received from the configured source.</summary>
public sealed record IncomingRecord(
    string Id,
    IReadOnlyDictionary<string, string> Fields,
    DateTimeOffset ReceivedAt);

/// <summary>Maps one source field to its ordered desktop destination.</summary>
public sealed record FieldMapping(string Source, string Target, int Order);

public sealed record ProcessingResult(
    string RecordId,
    RecordState State,
    int Attempts,
    TimeSpan Duration,
    string Message);

/// <summary>Keeps UI-specific write mechanics outside the queue policy.</summary>
public interface IDesktopTarget
{
    Task WriteAsync(
        IReadOnlyDictionary<string, string> mappedFields,
        CancellationToken cancellationToken);
}

/// <summary>Tracks committed IDs so retries remain idempotent.</summary>
public interface ICommitLedger
{
    bool Contains(string recordId);
    void MarkCommitted(string recordId);
}
