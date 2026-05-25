# PROJECT_RULES

- Don't use `Resources.Load`. All dependencies/configs must be injected via DI (transparent wiring).
- Don't create or modify `*.asmdef` files without explicit request.
- Don't add new `ScriptableObject` assets without explicit request (confirm first).
- Input changes: any rename/remove of `InputAction` requires confirmation.
- ECS system order changes require confirmation.
- Before adding a new ECS/input/gameplay system, check existing systems for the same responsibility. Do not create a second producer of the same event/component/request unless explicitly replacing the old owner in the same task.
- If a bug exposes a missing gameplay/system architecture, do not solve it with a narrow patch by default. First explain the underlying system gap, propose a proper design, and only use a temporary workaround if explicitly approved.
- New gameplay-affecting settings/constants: expose via config/inspector and provide code defaults.
- When adding or changing a gameplay feature, update the relevant project documentation in `Docs/` during the same task. If there is no suitable document yet, create one.
- If implementation details have design ambiguity or missing gameplay intent, ask for clarification before coding instead of filling gaps with hidden assumptions.
