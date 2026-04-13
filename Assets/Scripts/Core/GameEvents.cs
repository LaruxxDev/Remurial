public static class GameEvents {
    public static event Action<Vector2> OnMove;
    public static event Action OnInteract;      // tecla E
    public static event Action OnInspect;       // tecla F / click derecho
    public static event Action OnShoot;         // foto
    public static event Action<float> OnZoom;
    public static event Action OnSwitchCamera;
}