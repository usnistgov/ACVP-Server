# Adding an algorithm

Use this procedure when adding or changing algorithm support. For an existing
algorithm and mode, create a new revision if the change affects the registration
properties in `Parameters.cs`. Otherwise, make the change on the latest revision
available on the deployed server. Deprecated revisions remain in the source;
if the deployed revision is unclear, confirm it with a maintainer.

## 1. Establish the contract

- Confirm the defining standard and its draft or final version. The Gen/Vals
  often precede the ACVP schema; use the specification where it exists and
  confirm open contract decisions before implementing behavior that depends on them.
- **Registration:** Record the exact `algorithm`, optional `mode`, and `revision`.
  List the capabilities a registrant can declare in `Parameters.cs`, their units,
  required and optional properties, and defaults. Specify what
  `ParameterValidator.cs` must enforce: allowed values, minimums, maximums,
  increments, and relationships between properties.
- **Test vectors:** For each supported test type, specify the inputs the IUT
  receives in `prompt.json` and the outputs that belong in `expectedResults.json`.
  Identify which fields belong to a group or a case, their encodings and units,
  and when they are present.
- Identify authoritative known-answer vectors or an independent reference for
  crypto tests. Do not use the new implementation as its own correctness oracle.
- Choose current examples for the relevant family and data flow. Ascon provides
  hash/XOF/AEAD examples; SPDM provides a KDF; XECDH includes deferred cases.
  `ML-KEM/FIPS203/v1_0/EncapDecap` shows KEM encapsulation and decapsulation;
  `ML-DSA/FIPS204/v1_0/SigVer` shows signature verification. `HMAC/v2_0` and
  `SHAKE/FIPS202` show revisions that reuse existing crypto/oracle operations.
  Their generation code is under
  `gen-val/src/generation/src/NIST.CVP.ACVTS.Libraries.Generation/`.
  Follow the contract when it differs from an example.

## 2. Map the implementation

Use the feature branch for the change, or create one from the intended base
before editing. Identify existing components to reuse and the files to add or
change. Trace one case through generation, oracle dispatch, observer grain,
crypto, and result validation. Cover the applicable layers:

| Layer | Check |
| --- | --- |
| Common | `AlgoMode` enum and serialized identity. |
| Crypto | Interfaces, factory, primitive, and known-answer tests. |
| Oracle | Parameter/result types, `IOracle`, and dispatch. |
| Orleans | Grain interface, implementation, and dependency registration. |
| Generation | Parameters, validation, groups/cases, factories, generators, projections, and validators. |
| Discovery | Advertise each `AlgoMode` in exactly one `RegisterInjections.cs`; run the existing `GenValInvokerTests` to check coverage and uniqueness. |
| Tests/solutions | Focused crypto/generation tests, integration fixture, and solution wiring. |

For deferred cases, follow the resolver and completion path as well. Reuse the
project's interfaces and injectable dependencies. A revision may reuse existing
crypto, oracle methods, and grains. Keep unrelated refactors outside the change.

## 3. Implement crypto and registration

- Implement the standard using the repository's crypto interfaces and factory
  patterns. Pass entropy as a function argument or inject its source when
  tests need deterministic inputs.
- Test supported operations, relevant empty/boundary inputs, and invalid inputs
  against the contract. Keep expensive tests in the appropriate test category.
- Match `AlgoMode` naming and its `EnumMember` value to the algorithm/mode/revision
  identity. Use the specified revision, not a guess based on a namespace name.
- Set fixed `TestVectorSet` identity fields to canonical values and test the
  identity produced by the complete vector-set creation path.
- Use `MathDomain` for registration properties that accept a range of values.
  Apply the validation rules and defaults established in step 1.

## 4. Generate and validate cases

- Keep registration capabilities on `Parameters`. Carry the `MathDomain` on
  `TestGroup` so case generators can select values within the registered range.
  Put case-specific values on `TestCase` and shared values on `TestGroup`.
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
  tests first. Select the SDK from the project and `Directory.Build.props` target
  frameworks. Then run the integration fixture for the affected revision against
  a local Orleans server.

The checked-in host and test client settings already target the same local
cluster. If you need a local override:

- The sample host reads `gen-val/samples/sharedappsettings.json` directly.
- For the test client, copy its `sharedappsettings.json` to
  `sharedappsettings.local.json` in the integration test output directory and
  adjust the settings to match the host. The client selects this optional file
  with `DOTNET_ENVIRONMENT=local`.

From the repository root, start the local Orleans server in one terminal:

```sh
dotnet run --project gen-val/samples/NIST.CVP.ACVTS.Orleans.ServerHost --no-restore
```

In another terminal, run the affected fixture. For example, this runs the
small HMAC-SHA2-256 v2 cases for valid and corrupted responses:

```sh
tests=gen-val/src/generation/test/NIST.CVP.ACVTS.Libraries.Generation.HMAC.IntegrationTests
DOTNET_ENVIRONMENT=local dotnet test "$tests" --no-restore --filter 'FullyQualifiedName~v2_0.GenValTestsHmacSha2_256&Name!~LotsOfTests'
```

Omit `&Name!~LotsOfTests` to run the full fixture in a clean worktree: the
large-vector test replaces its algorithm's sample directory under
`gen-val/json-files/`. Stop the server after testing.

In the affected algorithm's generated files,
check identity, field placement, lengths, counts, and validation dispositions
using selected fields and structural summaries. Local samples can omit cases
available on the deployed server. Include only artifacts required for the change.

## 6. Prepare the review

Run `git status --short` to include new files in the review inventory, then
`git diff --check` and, after staging, `git diff --cached --check`. Inspect new files
directly before staging. Review the changed-file list for unrelated or generated
churn. Describe the supported scope, standard/schema references, validation
commands and actual results, and remaining decisions or environment limitations.
When shared interfaces changed, include relevant existing-algorithm checks.
