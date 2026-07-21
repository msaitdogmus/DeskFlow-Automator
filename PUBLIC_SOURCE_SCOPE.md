# Public source scope

This repository contains the parts of DeskFlow that are useful for reviewing
the engineering approach: data contracts, validation, mapping, retry behavior,
idempotency boundaries and theme definitions.

The production desktop shell, customer-specific protocol adapter, Windows
control selectors and deployment configuration are intentionally not included.
They depend on the target application and are delivered only under a project
agreement.

Build outputs, executables, local settings, audit files and machine-specific
paths are excluded from version control.
