# Working in ACVP-Server

For a new algorithm, mode, or revision, follow
[Adding an algorithm](.ai/guides/adding-an-algorithm.md) before editing.
In Claude Code, this procedure is also available through `/add-algorithm`.

## Find the relevant code

Paths below are relative to the repository root:

| Path | Purpose |
| --- | --- |
| `gen-val/src/common/src` | Algorithm identities and shared helpers. |
| `gen-val/src/crypto/src` | Crypto implementations, interfaces, and factories. |
| `gen-val/src/crypto/test` | Primitive and wrapper tests. |
| `gen-val/src/generation/src` | Registration, generation, projections, and validation. |
| `gen-val/src/generation/test` | Generation unit and integration tests. |
| `gen-val/src/oracle/src` | Oracle interfaces and dispatch. |
| `gen-val/src/orleans/src` | Observer grains and dependency registration. |
| `gen-val/src/solutions` | Focused solutions by algorithm or subsystem. |
| `gen-val/samples/NIST.CVP.ACVTS.Orleans.ServerHost` | Local Orleans server host. |

Start with file names and the relevant source/test directories. Read a small
number of related implementations, then expand the search when necessary.

## Keep exploration focused

- Exclude `gen-val/json-files/`, `.git/`, `bin/`, and `obj/` from broad content
  searches. Generated JSON bundles are examples, not implementation source.
- When output inspection is needed, select the affected algorithm's files and
  inspect structure, counts, and relevant fields instead of dumping whole bundles.
- Scope Git history to relevant paths and a limited number of commits. Review
  changed-file lists or statistics before requesting detailed diffs.
- Keep successful build/test output short; inspect failure details as needed.

These practices limit unnecessary context; they do not enforce a token budget.

## Validate the change

Use the smallest relevant project or focused solution first. See README.md for
test categories and local-server setup. Integration tests that use Orleans need
a running server; unit tests with mocked dependencies do not prove that path.

```text
dotnet restore gen-val/src/solutions/<family>/<Solution>.sln
dotnet test <affected-test-project.csproj> --no-restore --filter <relevant-filter>
git diff --check
```

Replace placeholders with existing paths and appropriate test filters. Broaden
validation when shared code changes. Report commands actually run, their results,
and any environmental blockers; distinguish checks not run from checks passed.
