# PROJECT_RULES

- Don't use `Resources.Load`. All dependencies/configs must be injected via DI (transparent wiring).
- Don't create or modify `*.asmdef` files without explicit request.
- Don't add new `ScriptableObject` assets without explicit request (confirm first).
- Input changes: any rename/remove of `InputAction` requires confirmation.
- ECS system order changes require confirmation.
- New gameplay-affecting settings/constants: expose via config/inspector and provide code defaults.
