namespace DeskFlow.Portfolio;

/// <summary>
/// Representative core of the desktop automation queue.
/// Customer protocol and UI-control details stay behind interfaces.
/// </summary>
public sealed class RecordPipeline(
    IDesktopTarget target,
    ICommitLedger ledger,
    IReadOnlyList<FieldMapping> mappings)
{
    public async Task<ProcessingResult> ProcessAsync(
        IncomingRecord record,
        int retryCount,
        CancellationToken cancellationToken)
    {
        var started = DateTimeOffset.UtcNow;
        Validate(record);

        // A reconnect or application restart must not submit the same record twice.
        if (ledger.Contains(record.Id))
            return Result(RecordState.Completed, 0, "Already committed; duplicate skipped.");

        Exception? finalError = null;
        var maximumAttempts = Math.Max(1, retryCount + 1);

        for (var attempt = 1; attempt <= maximumAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await target.WriteAsync(Map(record), cancellationToken);
                ledger.MarkCommitted(record.Id);
                return Result(RecordState.Completed, attempt, "Committed successfully.");
            }
            catch (Exception error) when (error is not OperationCanceledException)
            {
                finalError = error;
                if (attempt < maximumAttempts)
                    await Task.Delay(TimeSpan.FromMilliseconds(300), cancellationToken);
            }
        }

        return Result(RecordState.Failed, maximumAttempts,
            finalError?.Message ?? "The target rejected the record.");

        ProcessingResult Result(RecordState state, int attempts, string message) =>
            new(record.Id, state, attempts, DateTimeOffset.UtcNow - started, message);
    }

    private IReadOnlyDictionary<string, string> Map(IncomingRecord record) =>
        mappings.OrderBy(item => item.Order).ToDictionary(
            item => item.Target,
            item => record.Fields[item.Source],
            StringComparer.OrdinalIgnoreCase);

    private void Validate(IncomingRecord record)
    {
        if (string.IsNullOrWhiteSpace(record.Id))
            throw new InvalidDataException("A record ID is required.");

        foreach (var mapping in mappings)
        {
            if (!record.Fields.TryGetValue(mapping.Source, out var value) ||
                string.IsNullOrWhiteSpace(value))
                throw new InvalidDataException($"Missing source field: {mapping.Source}");
        }
    }
}
