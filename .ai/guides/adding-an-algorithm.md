# Adding an algorithm

Use this procedure for a new algorithm, mode, or revision. For an extension,
identify which steps are already satisfied and change only the affected layers.

## 1. Establish the contract

- Confirm the defining standard (including draft/final version) and ACVP
  registration/vector specification.
  Record the exact `algorithm`, optional `mode`, and `revision`, supported test
  types, units, required/optional fields, defaults, and capability domains.
- Identify authoritative known-answer vectors or an independent reference for
  crypto tests. Do not use the new implementation as its own correctness oracle.
- Identify missing schema or scope decisions before implementing behavior that
  depends on them. Describe the question and continue independent work where possible.
- Choose current examples for the relevant family and data flow. Ascon provides
  hash/XOF/AEAD examples; SPDM provides a KDF; XECDH includes deferred cases.
  `ML-DSA/FIPS204/v1_0/SigVer` provides signature verification; `HMAC/v2_0` and
  `SHAKE/FIPS202` show revisions that reuse existing crypto/oracle operations.
  Their generation code is under
  `gen-val/src/generation/src/NIST.CVP.ACVTS.Libraries.Generation/`.
  Follow the contract when it differs from an example.

## 2. Map the implementation

Before editing, identify existing components to reuse and the files to add or
change. Trace one case through generation, oracle dispatch, observer grain,
crypto, and result validation. Cover the applicable layers:

| Layer | Check |
| --- | --- |
| Common | `AlgoMode` enum and serialized identity. |
| Crypto | Interfaces, factory, primitive, and known-answer tests. |
| Oracle | Parameter/result types, `IOracle`, and dispatch. |
| Orleans | Grain interface, implementation, and dependency registration. |
| Generation | Parameters, validation, groups/cases, factories, generators, projections, and validators. |
| Discovery | `RegisterInjections` advertises `ISupportedAlgoModeRevisions`; `GenValInvokerTests` checks enum coverage and uniqueness. |
| Tests/solutions | Focused crypto/generation tests, integration fixture, and solution wiring. |

For deferred cases, follow the resolver and completion path as well. Reuse the
project's interfaces and injectable dependencies. A revision may reuse existing
crypto, oracle methods, and grains. Keep unrelated refactors outside the change.

## 3. Implement crypto and registration

- Implement the standard using the repository's crypto interfaces and factory
  patterns. Make entropy injectable when deterministic testing requires it.
- Test supported operations, relevant empty/boundary inputs, and invalid inputs
  against the contract. Keep expensive tests in the appropriate test category.
- Match `AlgoMode` naming and its `EnumMember` value to the algorithm/mode/revision
  identity. Use the specified revision, not a guess based on a namespace name.
  Extend `AlgoModeLookupHelperTests` to check that the contract's literal algorithm,
  optional mode, and revision resolve to the intended enum through
  `AlgoModeLookupHelper.GetAlgoModeFromStrings`.
- Set fixed `TestVectorSet` identity fields to canonical values and test the
  identity produced by the complete vector-set creation path.
- Use `MathDomain` where the ACVP schema specifies a domain; use the specified
  array or scalar type otherwise. Validate required capabilities, allowed values,
  bounds, and increments in `ParameterValidator`; apply only defaults permitted
  by the contract. Preserve the registrant's capabilities.

## 4. Generate and validate cases

- Keep registration capabilities on `Parameters`. Carry generation domains on
  `TestGroup` where needed; place selected values on `TestCase` when the schema
  makes them case-specific. Keep truly shared emitted values at group level.
- Name generators and validators for their test type. Include valid extrema and
  relevant behavior classes when selecting candidate values. Follow the test
  type's count policy: keep fixed counts stable and cap their candidate queues,
  letting `ShuffleQueue` repeat short lists. Preserve coverage-driven counts where
  required; SHAKE/FIPS202 derives its count during preparation.
- For block-based behavior, cover valid positions around block boundaries across
  the registered domain, using block-relative remainders where appropriate.
  Avoid repeating constraints already guaranteed by parameter validation.
- Inspect prompt, internal, and result contract resolvers. Include the required
  IUT inputs and outputs without leaking expected answers or generation-only
  domains into prompts. Avoid redundant JSON naming attributes.
- Test valid/invalid registrations, group/case shape, projections, matching and
  mismatching results, and missing or malformed required output. For verification
  tests, check that negative cases exercise their intended failure condition.

## 5. Connect and exercise the complete path

- Wire factories, oracle methods, observer grains, and dependency registration.
  Keep related oracle method families together and DTOs minimal.
- Add the focused solution under `gen-val/src/solutions/<family>/` or update the
  existing one. Include the required test projects and server host for integration.
  Update aggregate solution entries where the existing project pattern requires it.
- Follow an appropriate `GenValTestsSingleRunnerBase` fixture. A current example
  is `gen-val/src/generation/test/NIST.CVP.ACVTS.Libraries.Generation.Ascon.XOF.IntegrationTests/SP800_232/XOF128/GenValTests.cs`.
  Cover successful validation and deliberately corrupted results.
- When sample and non-sample paths differ, exercise non-sample generation and
  deferred completion with separately supplied valid and corrupted IUT responses.
  Feeding sample expected results back through validation does not cover that path.
- Restore the focused solution and run the affected crypto and generation unit
  tests first. Then start the local server using the repository's development
  configuration and run the affected integration fixtures. Select the SDK from
  the project and `Directory.Build.props` target frameworks.

The larger integration test replaces affected `gen-val/json-files/` directories.
Before running it, check those destinations for local work, including untracked
files, and preserve it. Filter shared projects to the intended fixtures/revisions;
for example, HMAC-SHA2-256 v2 uses
`FullyQualifiedName~v2_0.GenValTestsHmacSha2_256`. Broaden deliberately when needed.
Replace the project and filter placeholders below. Commands use POSIX shell syntax:

```sh
DOTNET_ENVIRONMENT=dev dotnet run --project gen-val/samples/NIST.CVP.ACVTS.Orleans.ServerHost/NIST.CVP.ACVTS.Orleans.ServerHost.csproj --no-restore
DOTNET_ENVIRONMENT=dev dotnet test <algorithm-integration-project.csproj> --no-restore --filter '<relevant-fixture-filter>'
```

Run the server in a separate terminal. Compare `gen-val/samples/sharedappsettings.json`
with the test client's `sharedappsettings*.json`; both must address the same
cluster. The checked-in local configuration uses the localhost gateway on port
30000 and cluster `cryptoDist-local`. Stop the server after testing. Report
unavailable dependencies or connectivity failures without treating them as
passing tests.

Inspect the generated registration, prompt, internal projection, expected results,
and validation output for identity, field placement, lengths, counts, and
dispositions. Use selected fields and structural summaries. Review generated-file
changes separately and include only artifacts required for the contribution.

## 6. Prepare the review

Run `git status --short` to include new files in the review inventory, then
`git diff --check` and, after staging, `git diff --cached --check`. Inspect new files
directly before staging. Review the changed-file list for unrelated or generated
churn. Describe the supported scope, standard/schema references, validation
commands and actual results, and remaining decisions or environment limitations.
When shared interfaces changed, include relevant existing-algorithm checks.
