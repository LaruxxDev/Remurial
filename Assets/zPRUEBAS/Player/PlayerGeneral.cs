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


    [Header("Health System")]
    PlayerHealth PlayerHealth;
    public PlayerHealth HEALTH => PlayerHealth;


    [Header("Cameras")]
    public CinemachineCamera firstPersonCamera;
    public CinemachineCamera thirdPersonCamera;


    [Header("Objects")]
    public GameObject flashObject;


    [Header("Animations")]
    [SerializeField] PhotoController FotoController;
    public PhotoController PHOTO => FotoController;


    [Header("Photograph")]
    public RevealPhoto heldPhoto;


    [Header("Inspection")]
    [SerializeField] InspectSystem InspectSystem;
    public InspectSystem INSPECT;
    public GameObject inspectionItem;

    [Header("InputMap")]
    [SerializeField] private PlayerInput PlayerInput;

    [Header("Inventory")]
    [SerializeField] InventoryManager InventoryManager;
    public InventoryManager INVENTORY => InventoryManager;




    //[Header("Animations")]
    //[SerializeField] AnimationManager AnimationManager;
    //public AnimationManager ANIMATION => AnimationManager;
    #endregion


    private void Awake()
    {
        // StateMachine
        StateMachine = new StateMachine();
        States = new StateCollection(this);

        // Inputs
        InputTransformer = new InputTransformer(PlayerInput);

        // Stats
        PlayerMovement = new PlayerMovement(Rigidbody, PlayerConfiguration, this);
        PlayerHealth = new PlayerHealth(PlayerConfiguration, this);
    }

    void Start()
    {
        StateMachine.ChangeState(States.NeutralState(StateMachine));
    }

    void Update()
    {
        StateMachine.Update();


        // DEBUG
        // States
        stateText.text = StateMachine.state.Name;

        // SubStates
        if (StateMachine.state is NeutralState neutral)
            subStateText.text = neutral.subMachine.state.Name;
        if (StateMachine.state is OnCameraState onCamera)
            subStateText.text = onCamera.subMachine.state.Name;
        if (STATEMACHINE.state is DialogueState || STATEMACHINE.state is InspectState || STATEMACHINE.state is InventoryState || STATEMACHINE.state is DeadState)
            subStateText.text = "";

        // HP
        hpText.text = PlayerConfiguration.health.ToString();
    }

    [Header("Debug")]
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI subStateText;
    public TextMeshProUGUI hpText;
    [Space]
    public SavePoint savePointA;
    public SavePoint savePointB;


    void FixedUpdate() => StateMachine.FixedUpdate();

    void LateUpdate() => StateMachine.LateUpdate();
}