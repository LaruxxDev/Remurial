using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Game/InputReader")]
public class InputReader : ScriptableObject,
    PlayerInputActions.IPlayerActions,
    PlayerInputActions.IUIActions
{
    private PlayerInputActions _input;

    // ── MOVIMIENTO ───────────────────────────────────────────────
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;

    // ── CÁMARA FOTOGRÁFICA ───────────────────────────────────────
    public event Action OnAttackStarted;    // clic izq / RT → disparar foto
    public event Action OnAimStarted;       // clic der / LT → apuntar/zoom
    public event Action OnAimCanceled;

    // ── FLASH ────────────────────────────────────────────────────
    public event Action OnFlashStarted;     // F / B → usar flash
    public event Action OnToggleFlash;      // Q / dpad-left → encender/apagar flash

    // ── INTERACCIÓN (Hold) ───────────────────────────────────────
    public event Action OnInteractStarted;  // empieza a mantener E → mostrar barra UI
    public event Action OnInteractPerformed;// Hold completado → ejecutar acción
    public event Action OnInteractCanceled; // soltó antes → cancelar barra UI

    // ── REVELADO ─────────────────────────────────────────────────
    public event Action OnRevealUp;         // Shift / RT → subir temperatura
    public event Action OnRevealDown;       // Ctrl / RB → bajar temperatura

    // ── INVENTARIO / UI ──────────────────────────────────────────
    public event Action OnInventario;       // Tab / dpad-up
    public event Action OnReload;           // R / Y → recargar carrete
    public event Action OnSave;             // G / dpad-down → guardar

    // ────────────────────────────────────────────────────────────

    private void OnEnable()
    {
        _input ??= new PlayerInputActions();
        _input.Player.SetCallbacks(this);
        _input.UI.SetCallbacks(this);
        EnableGameplay();
    }

    private void OnDisable() => _input.Disable();

    // ── Control de contextos (llama esto al abrir inventario, pausa, etc.) ──
    public void EnableGameplay()
    {
        _input.Player.Enable();
        _input.UI.Disable();
    }

    public void EnableUI()
    {
        _input.Player.Disable();
        _input.UI.Enable();
    }

    public void DisableAll() => _input.Disable();

    // ── Implementación IPlayerActions ───────────────────────────

    void PlayerInputActions.IPlayerActions.OnMove(InputAction.CallbackContext ctx)
        => OnMoveEvent?.Invoke(ctx.ReadValue<Vector2>());

    void PlayerInputActions.IPlayerActions.OnLook(InputAction.CallbackContext ctx)
        => OnLookEvent?.Invoke(ctx.ReadValue<Vector2>());

    // Attack = disparar foto (clic izq / RT)
    void PlayerInputActions.IPlayerActions.OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started) OnAttackStarted?.Invoke();
    }

    // Aim = apuntar/zoom (clic der / LT)
    void PlayerInputActions.IPlayerActions.OnAim(InputAction.CallbackContext ctx)
    {
        if (ctx.started)   OnAimStarted?.Invoke();
        if (ctx.canceled)  OnAimCanceled?.Invoke();
    }

    // Interact con Hold: tres fases para la barra de progreso
    void PlayerInputActions.IPlayerActions.OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)    OnInteractStarted?.Invoke();
        if (ctx.performed)  OnInteractPerformed?.Invoke();
        if (ctx.canceled)   OnInteractCanceled?.Invoke();
    }

    void PlayerInputActions.IPlayerActions.OnFlash(InputAction.CallbackContext ctx)
    {
        if (ctx.started) OnFlashStarted?.Invoke();
    }

    void PlayerInputActions.IPlayerActions.OnToggleFlash(InputAction.CallbackContext ctx)
    {
        if (ctx.started) OnToggleFlash?.Invoke();
    }

    void PlayerInputActions.IPlayerActions.OnRevealUp(InputAction.CallbackContext ctx)
    {
        if (ctx.started) OnRevealUp?.Invoke();
    }

    void PlayerInputActions.IPlayerActions.OnRevealDown(InputAction.CallbackContext ctx)
    {
        if (ctx.started) OnRevealDown?.Invoke();
    }

    void PlayerInputActions.IPlayerActions.OnInventario(InputAction.CallbackContext ctx)
    {
        if (ctx.started) OnInventario?.Invoke();
    }

    void PlayerInputActions.IPlayerActions.OnReload(InputAction.CallbackContext ctx)
    {
        if (ctx.started) OnReload?.Invoke();
    }

    void PlayerInputActions.IPlayerActions.OnSave(InputAction.CallbackContext ctx)
    {
        if (ctx.started) OnSave?.Invoke();
    }

    // ── Implementación IUIActions (mínimo necesario) ─────────────
    void PlayerInputActions.IUIActions.OnNavigate(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnSubmit(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnCancel(InputAction.CallbackContext ctx)
    {
        if (ctx.started) EnableGameplay(); // Escape cierra cualquier UI y vuelve al juego
    }
    void PlayerInputActions.IUIActions.OnPoint(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnClick(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnRightClick(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnMiddleClick(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnScrollWheel(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnTrackedDevicePosition(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnTrackedDeviceOrientation(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnRotate(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnRotateMando(InputAction.CallbackContext ctx) { }
}