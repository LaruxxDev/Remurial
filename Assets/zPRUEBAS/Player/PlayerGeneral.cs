using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGeneral : MonoBehaviour
{
    #region References
    [Header("StateMachine")]
    StateMachine StateMachine;
    public StateMachine STATEMACHINE => StateMachine;

    StateCollection States;
    public StateCollection STATES => States;


    [Header("Componentes")]
    public Rigidbody Rigidbody;
    public Transform mainCamera;

    [Header("Configuration")]
    [SerializeField] PlayerConfiguration PlayerConfiguration;
    public PlayerConfiguration CONFIGURATION => PlayerConfiguration;


    [Header("Collider")]
    [SerializeField] PlayerCollision PlayerCollision;
    public PlayerCollision COLLISION => PlayerCollision;


    [Header("Inputs")]
    InputTransformer InputTransformer;
    public InputTransformer INPUTTRANSFORMER => InputTransformer;


    [Header("Movement")]
    PlayerMovement PlayerMovement;
    public PlayerMovement MOVEMENT => PlayerMovement;


    [Header("Cameras")]
    public CinemachineCamera firstPersonCamera;
    public CinemachineCamera thirdPersonCamera;


    [Header("Objects")]
    public GameObject flashObject;


    //[Header("Animations")]
    //[SerializeField] AnimationManager AnimationManager;
    //public AnimationManager ANIMATION => AnimationManager;
    #endregion


    private void Awake()
    {
        StateMachine = new StateMachine();
        States = new StateCollection(this);
        InputTransformer = new InputTransformer();
        PlayerMovement = new PlayerMovement(Rigidbody, PlayerConfiguration, this);
    }

    void Start()
    {
        StateMachine.ChangeState(States.NeutralState(StateMachine));
    }

    void Update()
    {
        StateMachine.Update();


        // DEBUG
        stateText.text = StateMachine.state.Name;

        if (StateMachine.state is NeutralState neutral)
            subStateText.text = neutral.subMachine.state.Name;
        if (StateMachine.state is OnCameraState onCamera)
            subStateText.text = onCamera.subMachine.state.Name;
    }

    [Header("Debug")]
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI subStateText;

    void FixedUpdate() => StateMachine.FixedUpdate();

    void LateUpdate() => StateMachine.LateUpdate();
}