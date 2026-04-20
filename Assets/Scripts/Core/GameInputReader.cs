using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Game/InputReader")]
public class GameInputReader : ScriptableObject,
    PlayerInputActions.IPlayerActions,
    PlayerInputActions.IUIActions
{
    private PlayerInputActions _input;

    // ── MOVIMIENTO ───────────────────────────────────────────────
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;

    // ── CÁMARA FOTOGRÁFICA ───────────────────────────────────────
    public event Action OnAttackStarted;    
    public event Action OnAimStarted;       
    public event Action OnAimCanceled;

    // ── FLASH ────────────────────────────────────────────────────
    public event Action OnFlashStarted;     
    public event Action OnToggleFlash;      

    // ── INTERACCIÓN (Hold) ───────────────────────────────────────
    public event Action OnInteractStarted;  
    public event Action OnInteractPerformed;
    public event Action OnInteractCanceled; 

    // ── REVELADO ─────────────────────────────────────────────────
    public event Action OnRevealUp;         
    public event Action OnRevealDown;       

    // ── INVENTARIO / UI ──────────────────────────────────────────
    public event Action OnInventario;       
    public event Action OnReload;           
    public event Action OnSave;             

    // ── EVENTOS DE INSPECCIÓN (NUEVOS) ───────────────────────────
    public event Action<bool> OnClickEvent;     // Para saber si mantiene pulsado el ratón
    public event Action<Vector2> OnRotateEvent; // Para el vector de rotación (ratón con click)
    public event Action<Vector2> OnRotateMandoEvent;// Para el vector de rotación (mando) 
    public event Action OnCancelUIEvent;        // Para salir de la inspección

    // ────────────────────────────────────────────────────────────

    private void OnEnable()
    {
        _input ??= new PlayerInputActions();
        _input.Player.SetCallbacks(this);
        _input.UI.SetCallbacks(this);
        EnableGameplay();
    }

    private void OnDisable() => _input.Disable();

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
    void PlayerInputActions.IPlayerActions.OnMove(InputAction.CallbackContext ctx) => OnMoveEvent?.Invoke(ctx.ReadValue<Vector2>());
    void PlayerInputActions.IPlayerActions.OnLook(InputAction.CallbackContext ctx) => OnLookEvent?.Invoke(ctx.ReadValue<Vector2>());
    void PlayerInputActions.IPlayerActions.OnAttack(InputAction.CallbackContext ctx) { if (ctx.started) OnAttackStarted?.Invoke(); }
    void PlayerInputActions.IPlayerActions.OnAim(InputAction.CallbackContext ctx) { if (ctx.started) OnAimStarted?.Invoke(); if (ctx.canceled) OnAimCanceled?.Invoke(); }
    void PlayerInputActions.IPlayerActions.OnInteract(InputAction.CallbackContext ctx) { if (ctx.started) OnInteractStarted?.Invoke(); if (ctx.performed) OnInteractPerformed?.Invoke(); if (ctx.canceled) OnInteractCanceled?.Invoke(); }
    void PlayerInputActions.IPlayerActions.OnFlash(InputAction.CallbackContext ctx) { if (ctx.started) OnFlashStarted?.Invoke(); }
    void PlayerInputActions.IPlayerActions.OnToggleFlash(InputAction.CallbackContext ctx) { if (ctx.started) OnToggleFlash?.Invoke(); }
    void PlayerInputActions.IPlayerActions.OnRevealUp(InputAction.CallbackContext ctx) { if (ctx.started) OnRevealUp?.Invoke(); }
    void PlayerInputActions.IPlayerActions.OnRevealDown(InputAction.CallbackContext ctx) { if (ctx.started) OnRevealDown?.Invoke(); }
    void PlayerInputActions.IPlayerActions.OnInventario(InputAction.CallbackContext ctx) { if (ctx.started) OnInventario?.Invoke(); }
    void PlayerInputActions.IPlayerActions.OnReload(InputAction.CallbackContext ctx) { if (ctx.started) OnReload?.Invoke(); }
    void PlayerInputActions.IPlayerActions.OnSave(InputAction.CallbackContext ctx) { if (ctx.started) OnSave?.Invoke(); }

    // ── Implementación IUIActions ─────────────────────────────
    void PlayerInputActions.IUIActions.OnNavigate(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnSubmit(InputAction.CallbackContext ctx) { }
    
    void PlayerInputActions.IUIActions.OnCancel(InputAction.CallbackContext ctx)
    {
        if (ctx.started) OnCancelUIEvent?.Invoke(); // Disparamos el evento para que la UI decida qué cerrar
    }
    
    void PlayerInputActions.IUIActions.OnClick(InputAction.CallbackContext ctx) 
    { 
        OnClickEvent?.Invoke(ctx.action.IsPressed());
    }
    
    void PlayerInputActions.IUIActions.OnRotate(InputAction.CallbackContext ctx) 
    { 
        OnRotateEvent?.Invoke(ctx.ReadValue<Vector2>());
    }
    
void PlayerInputActions.IUIActions.OnRotateMando(InputAction.CallbackContext ctx) 
    { 
        OnRotateMandoEvent?.Invoke(ctx.ReadValue<Vector2>());
    }

    void PlayerInputActions.IUIActions.OnPoint(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnRightClick(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnMiddleClick(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnScrollWheel(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnTrackedDevicePosition(InputAction.CallbackContext ctx) { }
    void PlayerInputActions.IUIActions.OnTrackedDeviceOrientation(InputAction.CallbackContext ctx) { }
}