# Review Retrospectives

Keep one retrospective per reviewed pull request. Name files
`pr-<number>-<topic>.md`.

Each retrospective should contain:

- PR link, reviewer, review date, and re-audit date.
- GitHub thread state at the time of the audit.
- One numbered entry per review thread.
- Whether each comment was actionable, informational, or deferred.
- The concrete response and files changed for actionable comments.
- Any remaining work or reason no code change was made.
- A short summary of lessons learned from that PR.

Keep these notes source-specific. Do not describe the creation or maintenance of
the `.ai` directory here. Promote repeatable rules into `../conventions/` and
workflow changes into `../guides/`, then cite this retrospective as their source.

An addressed local comment may still appear unresolved on GitHub until the
change is pushed and the review thread is explicitly resolved. Record both facts
instead of treating them as the same state.

## Retrospectives

- [PR #455: BLAKE2b RFC7693](pr-455-blake2b.md)
