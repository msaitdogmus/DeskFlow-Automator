namespace DeskFlow.Portfolio;

public enum RecordState
{
    Queued,
    Processing,
    Completed,
    Failed
}

public sealed record IncomingRecord(
    string Id,
    IReadOnlyDictionary<string, string> Fields,
    DateTimeOffset ReceivedAt);

public sealed record FieldMapping(string Source, string Target, int Order);

public sealed record ProcessingResult(
    string RecordId,
    RecordState State,
    int Attempts,
    TimeSpan Duration,
    string Message);

public interface IDesktopTarget
{
    Task WriteAsync(
        IReadOnlyDictionary<string, string> mappedFields,
        CancellationToken cancellationToken);
}

public interface ICommitLedger
{
    bool Contains(string recordId);
    void MarkCommitted(string recordId);
}
