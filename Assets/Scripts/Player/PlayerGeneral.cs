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
    [SerializeField] private PlayerInput PlayerInput;


    [Header("Movement")]
    PlayerMovement PlayerMovement;
    public PlayerMovement MOVEMENT => PlayerMovement;


    [Header("Health System")]
    PlayerHealth PlayerHealth;
    public PlayerHealth HEALTH => PlayerHealth;


    [Header("PhotoController")]
    [SerializeField] PhotoController FotoController;
    public PhotoController PHOTO => FotoController;
    public RevealPhoto heldPhoto;
    public GameObject flashObject;


    [Header("Animations")]
    [SerializeField] AnimatorManager AnimatorManager;
    public AnimatorManager ANIMATION => AnimatorManager;

    [SerializeField] private SkinnedMeshRenderer PlayerModel;
    public SkinnedMeshRenderer MODEL => PlayerModel;


    [Header("Inspection")]
    [SerializeField] InspectSystem InspectSystem;
    public InspectSystem INSPECT => InspectSystem;
    public GameObject inspectionItem;


    [Header("Inventory")]
    [SerializeField] InventoryManager InventoryManager;
    public InventoryManager INVENTORY => InventoryManager;


    [Header("Cameras")]
    public CinemachineCamera firstPersonCamera;
    public CinemachineCamera thirdPersonCamera;
    public CinemachineCamera inspectionCamera;

    public GameObject heldItem;
    public Transform heldPosition;

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

        mainCamera = FindAnyObjectByType<Camera>().transform;
    }

    void Start()
    {
        StateMachine.ChangeState(States.NeutralState(StateMachine));
    }

    void Update()
    {
        StateMachine.Update();
    }

    void FixedUpdate() => StateMachine.FixedUpdate();

    void LateUpdate() => StateMachine.LateUpdate();
}