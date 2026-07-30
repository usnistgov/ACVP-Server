# Adding an Algorithm

Use this guide for a new algorithm, mode, or revision. Read
`../conventions/algorithm-coding-conventions.md` and the relevant
`../review-notes/` files first.

## 1. Establish the Source of Truth

- Confirm the exact ACVP `algorithm`, optional `mode`, and `revision` strings.
- Read the applicable registration and test-vector schema plus the defining
  standard. Record units explicitly; this codebase commonly models lengths in
  bits even when the primitive is byte-oriented.
- Capture supported test types, required and optional properties, defaults,
  ranges, increments, output fields, block sizes, and empty-input behavior.
- Choose two or three nearby implementations as references. Use one for the
  algorithm family, one for the current repository architecture, and one for a
  similar registration shape when available.
- Search the full repository for the closest `AlgoMode`, factory registrations,
  oracle calls, grains, contract resolvers, tests, and focused solutions. A
  working crypto class alone is not a complete algorithm addition.

Build a working manifest before editing:

| Layer | Expected artifacts |
| --- | --- |
| Common | `AlgoMode` identity and shared helpers, when needed. |
| Crypto common | Function descriptor, variant enum, interface, and factory interface. |
| Crypto | Primitive/wrapper, factory implementation, and known-answer tests. |
| Oracle abstractions | Minimal parameter/result DTOs and `IOracle` method. |
| Oracle implementation | Dispatch from the oracle to the correct observer grain. |
| Orleans | Grain interface, grain implementation, and dependency registration. |
| Generation | Parameters, validation, groups, cases, generators, validators, projections, and DI registration. |
| Tests | Validation, generation shape, boundary behavior, projections, validation, and crypto behavior. |
| Solution | Focused solution under `gen-val/src/solutions/<algorithm>/`. |

## 2. Define Identity and Registration

- Add the identity to
  `gen-val/src/common/src/NIST.CVP.ACVTS.Libraries.Common/AlgoMode.cs`.
- Treat identity as algorithm / mode / revision. Make the enum name and
  `EnumMember` value describe the same identity.
- Use the standard identifier for a standards-based revision when that matches
  the ACVP schema. Do not infer `1.0` from an older namespace name.
- Register the `AlgoMode` in the generation layer's `RegisterInjections`.
- Initialize `TestVectorSet.Algorithm`, `Mode`, and `Revision` to canonical output
  values. Confirm the generic vector factory and parameter validation cannot
  leave a noncanonical value in generated output.
- Add a focused assertion for the canonical vector-set identity when introducing
  a new naming pattern.

## 3. Model the ACVP Data Shape

Add or update files under
`gen-val/src/generation/src/NIST.CVP.ACVTS.Libraries.Generation/<Algorithm>/<Revision>`.

- `Parameters.cs` models registration capabilities. Use `MathDomain` when a
  numeric value may be a range, set, or singleton.
- `ParameterValidator.cs` applies defaults and validates allowed strings, bounds,
  increments, and required domains.
- `TestGroup.cs` carries shared metadata and allowed domains used to generate
  cases.
- `TestCase.cs` carries concrete selected values, input data, and expected/result
  fields.
- `TestVectorSet.cs` carries canonical identity and generated groups.
- Contract resolvers define the server, prompt, and result JSON surfaces.

Apply this placement test to every property:

1. Does registration describe a set of allowed values? Put it on `Parameters` as
   a domain.
2. Is that allowed set needed while creating cases? Carry a deep copy on
   `TestGroup`, usually with `[JsonIgnore]`.
3. Is one concrete value selected for each generated case? Emit it on `TestCase`.
4. Is the value truly shared by every case according to the ACVP schema? Only
   then emit it at group level.

Do not add `[JsonProperty]` merely to restate default camel-case serialization.
After moving a field, inspect every contract resolver so registration-only
domains do not leak into prompts and concrete case values are not omitted.

## 4. Implement the Crypto Layer

- Add shared descriptors and interfaces under
  `gen-val/src/crypto/src/NIST.CVP.ACVTS.Libraries.Crypto.Common`.
- Add implementation code under
  `gen-val/src/crypto/src/NIST.CVP.ACVTS.Libraries.Crypto/<Algorithm>`.
- Prefer the repository's existing crypto interfaces and factory patterns.
- Keep unsupported variants explicit: reject them clearly instead of silently
  routing them to a supported implementation.
- If imported/reference code is accepted for the initial change, keep the
  project-facing wrapper narrow and record provenance or future replacement work
  in the PR, without disguising it as project-native code.
- Add known-answer tests, including keyed/unkeyed and boundary behavior relevant
  to the standard.

## 5. Connect Oracle and Orleans

- Add minimal parameter and result types under
  `gen-val/src/oracle/src/NIST.CVP.ACVTS.Libraries.Oracle.Abstractions`.
- Add the `IOracle` method next to its algorithm family without splitting a block
  of methods from another standard.
- Add oracle dispatch under
  `gen-val/src/oracle/src/NIST.CVP.ACVTS.Libraries.Crypto.Oracle`.
- Add or update the observer grain interface and implementation under
  `gen-val/src/orleans/src`.
- Register factories, observers, and grains in the relevant DI setup.
- Trace one generated case end to end: generation parameters, oracle DTO, oracle
  dispatch, grain call, crypto factory, result DTO, and generated `TestCase`.
- Keep DTOs simple. Do not add `Equals` or `GetHashCode` unless a real caller
  depends on value equality.

## 6. Build Generation and Validation

- Add test-group generators and their factory for every supported test type.
- Add matching test-case generators and validators. Names should identify the
  test type, such as `TestCaseGeneratorAft` and `TestCaseValidatorAft`.
- Keep `NumberOfTestCasesToGenerate` fixed unless the protocol explicitly makes
  it dynamic.
- Build each candidate list from valid min/max values, required behavior classes,
  and random fill. Keep the distinct list at or below the requested case count.
- Use modulo predicates for recurring block-boundary classes. For a block size
  `B` and byte-aligned domains, typical classes are `x % B == B - 8`,
  `x % B == 0`, and `x % B == 8` when each is valid.
- Let `ShuffleQueue` repeat shorter lists. Do not increase the requested test
  count merely because preparation produced another candidate.
- Do not repeat alignment predicates already guaranteed by parameter validation.
- Preserve optional values correctly. For example, distinguish an omitted key
  domain from a selected zero-length key according to the schema and local
  serialization pattern.

## 7. Add Focused Tests

Cover the behavior introduced by the algorithm:

- Registration accepts valid domains, applies documented defaults, and rejects
  values outside bounds or increments.
- Group generation preserves domains and creates the expected test types.
- Case generation emits concrete values in the correct JSON layer and calls the
  oracle with matching values.
- Candidate generation keeps its fixed count and includes behavior patterns such
  as block-boundary remainders, not only one literal.
- Prompt and result projections include exactly the intended fields.
- Result validators pass matching results, reject mismatches, and report missing
  required output.
- Crypto tests use authoritative vectors and cover all supported variants and
  modes.
- Canonical algorithm/mode/revision values survive complete vector-set creation.

## 8. Add and Run the Focused Solution

- Add `gen-val/src/solutions/<algorithm>/<Algorithm>.sln`.
- Include the common/math, crypto, oracle/orleans, generation, and test projects
  needed for the complete path.
- Restore and test the focused solution before running broader suites:

```text
dotnet restore gen-val/src/solutions/<algorithm>/<Algorithm>.sln
dotnet test gen-val/src/solutions/<algorithm>/<Algorithm>.sln --no-restore
git diff --check
```

If shared infrastructure changed, follow with the relevant broader solution or
project tests.

## 9. Definition of Done

An algorithm addition is complete when:

- ACVP identity and JSON shape match the schema and standard.
- Valid registration succeeds and invalid registration fails for the right
  reasons.
- Generated groups retain allowed domains while cases emit selected values.
- Generation reaches the intended crypto implementation through oracle and
  Orleans wiring.
- Prompt and result projections serialize the intended fields.
- Result validation compares every required output.
- Focused tests cover primitive correctness, schema shape, and boundary behavior.
- A focused solution restores and tests successfully, or any environmental
  blocker is reported with the exact command and output.
- Relevant prior review lessons were checked and new review feedback is recorded
  in a PR-specific retrospective after review.

## 10. Agent Task Template

```text
Read AGENTS.md, .ai/conventions/algorithm-coding-conventions.md,
.ai/guides/adding-an-algorithm.md, and relevant .ai/review-notes files.

Add <algorithm / mode / revision> using <standard and ACVP schema>. Before
editing, map the complete common, crypto, oracle, Orleans, generation, test, and
solution path using nearby implementations. Model numeric registration
capabilities with MathDomain, retain allowed domains on TestGroup, and emit each
selected concrete value on TestCase. Use canonical identity strings, cover
behavioral boundaries, keep generated case counts stable, update projections,
add the focused solution, and run its restore/test commands plus git diff --check.
```
