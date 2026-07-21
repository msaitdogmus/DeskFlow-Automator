# Architecture

```text
Proprietary source adapter
          │
          ▼
 validation + field mapping
          │
          ▼
 cancellation-aware queue ───► append-only audit
          │
          ▼
 idempotency check + bounded retry
          │
          ▼
 UI Automation / SendInput target adapter
```

The source and target are interfaces because neither the proprietary wire
protocol nor the final Windows controls should leak into queue policy. This
also lets validation mode use an isolated target without touching another
application.

The full application adds an Avalonia desktop shell, editable mappings,
metrics, audit export and responsive layouts for full, half and narrow windows.

## Failure policy

Validation errors stop before the target is touched. Transient write failures
use bounded retries, while committed record IDs are skipped on later runs.
