using UnityEngine;

[CreateAssetMenu(menuName = "Player/Configuration")]
public class PlayerConfiguration : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _turnSpeed = 120f;
    [SerializeField][Range(0f, 100f)] float _cameraSpeedPercent = 50f;

    public float MoveSpeed => _moveSpeed;
    public float TurnSpeed => _turnSpeed;

    public float CameraMoveSpeed => _moveSpeed * (_cameraSpeedPercent / 100f);

    [Header("Health")]
    [SerializeField] int _maxHealth = 6;
    [SerializeField] int _healthRegen = 1;

    public int MaxHealth  => _maxHealth;
    public int HealthRegen => _healthRegen;
}