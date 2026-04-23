using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGeneral : MonoBehaviour
{
    #region References
    [Header("StateMachine")]
    StateMachine _stateMachine;
    public StateMachine StateMachine => _stateMachine;

    StateCollection _states;
    public StateCollection States => _states;


    [Header("Componentes")]
    public Rigidbody Rigidbody;
    public Transform mainCamera;

    [Header("Configuration")]
    [SerializeField] PlayerConfiguration _playerConfiguration;
    public PlayerConfiguration Configuration => _playerConfiguration;


    [Header("Collider")]
    [SerializeField] PlayerCollision _playerCollision;
    public PlayerCollision Collision => _playerCollision;


    [Header("Inputs")]
    GameInputReader _input;
    public GameInputReader Input => _input;


    [Header("Movement")]
    PlayerMovement _playerMovement;
    public PlayerMovement Movement => _playerMovement;


    [Header("Cameras")]
    public CinemachineCamera firstPersonCamera;
    public CinemachineCamera thirdPersonCamera;

    CameraController _cameraController;
    public CameraController CameraController => _cameraController;

    [Header("Objects")]
    public GameObject flashObject;

    [Header("Animations")]
    [SerializeField] AnimatorManager _animationManager;
    public AnimatorManager Animator => _animationManager;
    #endregion


    private void Awake()
    {
        _stateMachine = new StateMachine();
        _states = new StateCollection(this);
        _input = new GameInputReader();
        _playerMovement = new PlayerMovement(Rigidbody, _playerConfiguration, this);
        _cameraController = new CameraController();
    }

    void Start()
    {
        _stateMachine.ChangeState(_states.NeutralState(_stateMachine));
    }

    void Update()
    {
        _stateMachine.Update();

        if (!showSubState)
            return;

        // DEBUG
        stateText.text = _stateMachine.state.Name;

        if (_stateMachine.state is NeutralState neutral)
            subStateText.text = neutral.subMachine.state.Name;
        if (_stateMachine.state is OnCameraState onCamera)
            subStateText.text = onCamera.subMachine.state.Name;
    }

    [Header("Debug")]
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI subStateText;
    public bool showSubState;

    void FixedUpdate() => _stateMachine.FixedUpdate();

    void LateUpdate() => _stateMachine.LateUpdate();
}