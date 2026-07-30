# AI Project Guidance

This directory contains durable, repository-specific context for coding agents.
The root `AGENTS.md` is the primary entry point; it points here for the deeper
material that would make that file too large. Root `CLAUDE.md` imports
`AGENTS.md` so Claude Code reaches the same instructions.

The lowercase `.ai` name is a repository convention, not a tool standard. Do
not assume an agent will discover this directory without the link from
`AGENTS.md`.

## Reading Order

For algorithm work, read:

1. `conventions/algorithm-coding-conventions.md`
2. `guides/adding-an-algorithm.md`
3. Relevant files under `review-notes/`

Then inspect the applicable ACVP specification and two or three nearby
algorithm implementations before editing code.

## Directory Structure

- `conventions/`: durable coding and modeling rules that apply across PRs.
- `guides/`: repeatable, end-to-end workflows.
- `review-notes/`: one source-specific retrospective per reviewed PR.

The repository's `notes/` directory is for raw or human working notes. Content
there is not canonical agent guidance until it is deliberately promoted here.

## Maintenance Model

1. Record each review in `review-notes/pr-<number>-<topic>.md`.
2. Keep that note factual and PR-specific: what was said, whether it was
   actionable, what changed, and what remains open.
3. Promote repeatable lessons into `conventions/` and cite the originating
   review note.
4. Update a guide only when the lesson changes the reusable workflow.
5. Keep historical review notes intact. Add a dated re-audit section when the
   implementation or GitHub thread state changes.

Repository code, the applicable standard, and current reviewer direction take
precedence over these notes when they disagree.
