# AGENTS.md

This file is the repository entry point for coding agents. Keep it concise and point to `.ai/` for deeper project guidance.

## Required Reading

Before algorithm work, read these files in order:

1. `.ai/README.md`
2. `.ai/conventions/algorithm-coding-conventions.md`
3. `.ai/guides/adding-an-algorithm.md`
4. Relevant files under `.ai/review-notes/`

## Repository Map

- `gen-val/src/common/src`: shared enums, math/domain helpers, JSON helpers, and cross-cutting common code.
- `gen-val/src/crypto/src`: crypto implementations and crypto-facing abstractions.
- `gen-val/src/crypto/test`: crypto primitive and wrapper tests.
- `gen-val/src/generation/src`: ACVP registration parsing, validation, test generation, projections, and result validation.
- `gen-val/src/generation/test`: generation unit and integration tests.
- `gen-val/src/oracle/src`: oracle abstraction and dispatch.
- `gen-val/src/orleans/src`: observer grains used by oracle generation.
- `gen-val/src/solutions`: focused Visual Studio solutions grouped by algorithm or subsystem.
- `.ai`: durable agent guidance, conventions, workflows, and review retrospectives.
- `notes`: human/raw notes; do not treat as canonical agent guidance unless promoted into `.ai`.

## Algorithm Work Rules

- Follow existing patterns in nearby algorithms, but verify the ACVP schema and standard before copying structure.
- Model algorithms as algorithm / mode / revision. Prefer standard identifiers such as `RFC7693`, `SP800-232`, or `FIPS204` over generic `1.0` when that is the repo convention for the algorithm.
- Use `MathDomain` for numeric registration capabilities that can be ranges or sets.
- Put allowed domains and shared metadata on `Parameters`/`TestGroup`; put selected concrete values on `TestCase`.
- Keep generated `TestVectorSet` algorithm/mode/revision strings canonical when the implementation has fixed values.
- Add or update focused solution files under `gen-val/src/solutions/<algorithm>/`.
- Add focused tests for validation, generation shape, projection shape, and crypto behavior touched by the change.

## Validation

Use the smallest focused check that covers your change first.

- Restore focused solution: `dotnet restore gen-val/src/solutions/<algorithm>/<Algorithm>.sln`
- Test focused solution: `dotnet test gen-val/src/solutions/<algorithm>/<Algorithm>.sln --no-restore`
- Check whitespace before finishing: `git diff --check`

If `dotnet` fails because package restore needs network access or because the local sandbox blocks the CLI, report that clearly with the exact command attempted.

## Documentation Rules

- Put reusable agent instructions in `.ai/`.
- Put durable coding/style rules in `.ai/conventions/`.
- Put process workflows in `.ai/guides/`.
- Put source-specific review retrospectives in `.ai/review-notes/`.
- When a reviewer teaches a repeatable convention, promote it from review notes into `.ai/conventions/` and cite the originating review note.
