# Algorithm Coding Conventions

These are durable conventions for adding or changing algorithms in ACVP
Server. They are preferred project style unless the applicable standard,
nearby current code, or explicit reviewer direction requires something else.

## Naming and Revisions

- Model an implementation as algorithm / mode / revision.
- Make an `AlgoMode` enum name encode the same identity as its `EnumMember`
  value.
- Prefer the defining standard as the revision when that is the algorithm's
  ACVP identity, such as `RFC7693`, `SP800-232`, `FIPS204`, or `FIPS205`.
- Do not infer `1.0` only from an older namespace or a neighboring algorithm.
- Initialize `TestVectorSet.Algorithm`, `Mode`, and `Revision` to the canonical
  values expected from that implementation. Verify the generic vector factory
  and parameter validator preserve those values.

Source: [PR #455 review, comments 2 and 4](../review-notes/pr-455-blake2b.md).

## Registration and Domain Modeling

- Use `MathDomain` for numeric registration capabilities that can be ranges,
  sets, or single values.
- Apply defaults for omitted optional domains in `ParameterValidator`, not in a
  generator.
- Validate bounds and increments in `ParameterValidator`.
- Do not repeat an already validated alignment constraint in a generator unless
  generation intentionally narrows the domain for another reason.

Source: [PR #455 review, comments 7 and 8](../review-notes/pr-455-blake2b.md).

## Test Groups and Test Cases

- `Parameters` represents the capabilities allowed by registration.
- `TestGroup` carries shared metadata and allowed domains used while generating
  its cases.
- `TestCase` carries each concrete value selected by generation.
- If a value can vary by case, emit it on `TestCase`, even when an early design
  happens to create one group per value.
- Keep a concrete value on `TestGroup` only when every case in that group shares
  it by schema design.

Source: [PR #455 review, comment 6](../review-notes/pr-455-blake2b.md).

## JSON Projection

- Do not add `[JsonProperty]` only to restate the serializer's default camel-case
  name.
- Use JSON attributes when a name differs, a property must be ignored, or a
  contract resolver needs explicit behavior.
- Recheck server, prompt, and result projections whenever data moves between
  parameters, groups, and cases.
- Prompt projection includes the inputs required by the IUT. Result projection
  includes supplied result fields and identifiers, not generation-only domains.

Source: [PR #455 review, comments 5 and 6](../review-notes/pr-455-blake2b.md).

## Generator Coverage

- Keep `NumberOfTestCasesToGenerate` stable during preparation.
- Start candidate lists with valid minimum and maximum values.
- Add values by behavior class. For block-oriented algorithms, select valid
  values immediately before, exactly on, and immediately after a block boundary.
- Express recurring boundaries with modulo predicates rather than one literal.
- Fill remaining slots with valid random values and keep the distinct candidate
  count at or below `NumberOfTestCasesToGenerate`.
- Let `ShuffleQueue` repeat a short candidate list. Do not inflate the requested
  case count to fit preparation output.
- Test the behavior pattern and fixed case count, not only one literal produced
  by one domain.

Source: [PR #455 review, comments 8-10](../review-notes/pr-455-blake2b.md).

## Crypto and Oracle Style

- Prefer project-native crypto interfaces and implementation style.
- When imported/reference crypto code is a deliberate first implementation,
  keep its wrapper small and record the follow-up concern without expanding the
  current PR beyond review scope.
- Future-facing variant abstractions are acceptable when currently unsupported
  variants fail explicitly.
- Keep simple oracle parameter/result DTOs minimal. Add value equality only when
  a caller requires it.
- Group `IOracle` members by algorithm family or standard; do not split a related
  block to insert an unrelated algorithm.

Source: [PR #455 review, comments 1, 3, 12, and 13](../review-notes/pr-455-blake2b.md).

## Naming, Tests, and Solutions

- Name test-case generators and validators for their test type, such as
  `TestCaseGeneratorAft` and `TestCaseValidatorAft`.
- Add crypto known-answer tests for primitive behavior.
- Add generation tests for registration validation, group shape, concrete case
  generation, projections, and result validation when those surfaces change.
- Add a focused solution under `gen-val/src/solutions/<algorithm>/<Algorithm>.sln`.
- Include the common/math, crypto, oracle/orleans, generation, and focused test
  projects required to build the complete algorithm path.

Source: [PR #455 review, comments 11 and 14](../review-notes/pr-455-blake2b.md).
