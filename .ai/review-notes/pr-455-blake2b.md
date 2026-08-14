# PR #455: BLAKE2b RFC7693 Review Retrospective

- Pull request: [usnistgov/ACVP-Server#455](https://github.com/usnistgov/ACVP-Server/pull/455)
- Reviewer: `celic`
- Review captured: 2026-07-23
- Local re-audit: 2026-07-30
- GitHub state at re-audit: 15 current, non-outdated review threads; all 15
  remained unresolved on GitHub.

"Addressed" below describes the local working tree. A thread remains unresolved
on GitHub until the fixes are pushed and the thread is explicitly resolved.

## Comment Ledger

| # | Review point | Assessment | Local response and location |
| --- | --- | --- | --- |
| 1 | Asked whether `Blake2HashFunction` has a variant only to support a possible future BLAKE2s implementation. | Informational and valid; no change requested. | Kept the variant abstraction. `Blake2Factory` supports `Blake2b` and explicitly rejects unsupported variants. See crypto common `Hash/Blake2/Blake2HashFunction.cs` and crypto `Blake2/Blake2Factory.cs`. |
| 2 | BLAKE2b has no mode, and its revision should identify RFC 7693 instead of generic `1.0`. | Actionable and valid. | Changed `AlgoMode` to `BLAKE2b_RFC7693` / `BLAKE2b-RFC7693` and updated generation registration in `BLAKE2/v1_0/RegisterInjections.cs`. |
| 3 | The imported native implementation does not follow the project's usual hash interface; a simpler project-native, specification-based implementation would be preferable later. | Valid architectural direction, but explicitly outside this review's immediate focus. | No broad crypto rewrite was made. The existing `Blake2b` wrapper and native implementation remain a known future improvement rather than expanding this PR's scope. |
| 4 | `TestVectorSet.Algorithm` and `Revision` should be set to the expected canonical values. | Actionable and valid. | Initialized them to `BLAKE2b` and `RFC7693` in `BLAKE2/v1_0/TestVectorSet.cs`; registration identity now uses the matching `AlgoMode`. |
| 5 | `[JsonProperty("testType")]` is redundant. | Actionable and valid. | Removed the attribute from `BLAKE2/v1_0/TestGroup.cs`; default camel-case serialization supplies `testType`. |
| 6 | A digest length selected from a range belongs on each test case, while the group retains the allowed range. | Actionable and valid. | `TestGroup.DigestLength` is now a hidden `MathDomain`; concrete `TestCase.DigestLength` is generated per case and included by `PromptProjectionContractResolver`. Group and case generators were updated accordingly. |
| 7 | Registration `digestLen` should be a domain, not a list of integers. | Actionable and valid. | Replaced `List<int> DigestLengths` with `MathDomain DigestLength` in `Parameters.cs`; updated defaulting, validation, group generation, and tests. |
| 8 | The generator's `x % 8 == 0` predicate repeats alignment already guaranteed by parameter validation. | Actionable and valid. | Removed the byte-alignment filters from message/key random selection in `TestCaseGeneratorAft.cs`; random fill now draws from the validated domain. |
| 9 | Boundary coverage should match block-relative behavior, not only exact values such as 1016, 1024, and 1032. | Actionable and valid. | Replaced literal checks with modulo predicates for one byte before, exactly on, and one byte after a 1024-bit BLAKE2b block boundary. |
| 10 | Preparation should not mutate `NumberOfTestCasesToGenerate`; `ShuffleQueue` already repeats short lists, and candidate lists should not exceed the requested count. | Actionable and valid. | Made the AFT count a fixed 25, capped digest/message/key candidate construction at that count, and left repetition to `ShuffleQueue`. |
| 11 | The AFT validator should be named `TestCaseValidatorAft`, not `TestCaseValidatorHash`. | Actionable and valid. | Renamed the validator and its tests; updated `TestCaseValidatorFactory`. |
| 12 | `Blake2Parameters` does not need custom `Equals` and `GetHashCode`. | Actionable and valid. | Removed both overrides from `Oracle.Abstractions/ParameterTypes/Blake2Parameters.cs`. |
| 13 | The BLAKE2 oracle method should not split the cSHAKE, ParallelHash, and TupleHash method group. | Actionable and valid. | Moved `GetBlake2CaseAsync` below that related block in `IOracle.Hash.cs`. |
| 14 | Add a focused BLAKE solution like other algorithm families use. | Actionable and valid. | Added `gen-val/src/solutions/blake/Blake.sln` with the common, crypto, oracle/orleans, generation, and focused test projects needed by the algorithm path. |
| 15 | Add integration tests, using Ascon XOF as the example. | Actionable and valid. | Added the BLAKE2 RFC7693 integration project and `GenValTestsSingleRunnerBase` fixture, wired the integration project and Orleans server host into the focused and aggregate solutions, and generated the reviewable `gen-val/json-files/BLAKE2b-RFC7693/` bundle. |

## Audit Result

- All 15 comments were accounted for locally.
- 13 comments produced code, test, solution, or workflow-documentation changes.
- 2 comments were informational or future architectural direction and did not
  request an immediate code change.
- No comment was invalid. Comments 1 and 3 differed only in disposition: they
  were useful observations, not blockers for the current PR.
- GitHub still showed every thread as unresolved at the re-audit. The code must
  be pushed, replies added where useful, and threads resolved before claiming the
  online review is fully closed.

## August 2026 Follow-up

On August 7, `celic` reviewed the integration output and confirmed that the JSON
files were complete and the unit tests looked good. The follow-up identified one
additional required registration change and one small crypto-interface alignment:

- `digestLen` must be supplied by the registrant. The validator must reject an
  omitted domain instead of defaulting it to 512 bits, because the gen/vals are
  also consumed as a library and must preserve the caller's declared support.
- Optional `keyLen` remained acceptable as implemented. Registering an explicit
  zero for unkeyed use was described as a preference, not a required change.
- The imported BLAKE2b primitive could remain behind a narrow wrapper, but
  unkeyed and keyed use should align with the repository's hash and MAC
  abstractions.

On August 12, `celic` pushed commit `a0b521a9` directly to the PR branch using
maintainer edit access. That commit adjusted the integration-test layout and
focused solution organization and regenerated the JSON artifacts; it did not
merge PR #455 or address the required `digestLen` behavior.

The August 14 local response:

- Removed the implicit 512-bit `digestLen` default and added a focused missing-
  domain validation test.
- Added a common one-shot `IHash` abstraction inherited by `ISha`, and made
  `IBlake2` expose both `IHash` and `IMac` behavior.
- Separated the unkeyed `HashMessage` path from keyed `Generate` behavior while
  retaining the existing native BLAKE2b implementation behind the wrapper.
- Updated the Orleans grain and crypto tests to exercise the hash and MAC paths.

## What This Review Taught Us

- Do not infer revision identity from a `v1_0` namespace; verify the standard and
  ACVP registration strings.
- Registration capabilities, group generation state, and emitted case values are
  three different layers. A variable numeric capability should remain a domain
  until a concrete test case is generated.
- Generator coverage should describe cryptographic behavior classes. Modulo
  relationships survive different valid domains; hard-coded examples do not.
- `ShuffleQueue` owns repetition. Preparation owns a bounded, intentional set of
  candidates and should not rewrite the public case count.
- Match validators to the ACVP test type and include the entire algorithm path in
  a focused solution.
- Generation unit tests with a mocked oracle do not replace full-path integration
  coverage. Run the real Orleans server, generate the JSON artifacts, and inspect
  their actual registration, prompt, internal, expected-result, and validation
  shapes.
- Reviewer comments can be valid without requiring immediate code. Record that
  disposition explicitly so future work does not confuse deferred architecture
  direction with a missed blocker.

## Current Local BLAKE2b Shape

- Identity: `BLAKE2b-RFC7693`; vector revision: `RFC7693`.
- Registration `digestLen`, `msgLen`, and `keyLen` are modeled as `MathDomain`
  values.
- Generated `digestLen`, `len`, and optional `keyLen` are concrete test-case
  values.
- AFT generation has a fixed count of 25 and covers valid 1024-bit block-relative
  message-length patterns.
- Validator naming, oracle grouping, parameter DTO shape, and focused solution
  layout now match the review direction.
- The BLAKE2 integration fixture exercises the registration-to-Orleans-to-crypto
  path and writes the five-file JSON bundle used for output-shape review.
