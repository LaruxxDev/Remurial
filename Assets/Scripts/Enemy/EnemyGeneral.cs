using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class EnemyGeneral : MonoBehaviour
{
    #region References
    [Header("StateMachine")]
    StateMachine StateMachine;
    public StateMachine STATEMACHINE => StateMachine;

    public enum EnemyType
    {
        flashyEnemy,
        birbEnemy,
        petriEnemy
    }

    public EnemyType enemyType;

    [SerializeField] EnemyStateCollection States;
    public EnemyStateCollection STATES => States;


    State mainState;

    [Header("Componentes")]
    public Rigidbody Rigidbody;


    [Header("Configuration")]
    [SerializeField] EnemyConfiguration EnemyConfiguration;
    public EnemyConfiguration CONFIGURATION => EnemyConfiguration;


    [Header("Collider")]
    [SerializeField] EnemyCollision EnemyCollision;
    public EnemyCollision COLLISION => EnemyCollision;


    [Header("Movement")]
    [SerializeField] NavMeshAgent NavMeshAgent;
    public NavMeshAgent NAVMESH => NavMeshAgent;

    EnemyMovement EnemyMovement;
    public EnemyMovement MOVEMENT => EnemyMovement;


    [Header("Animations")]
    [SerializeField] AnimatorManager AnimationManager;
    public AnimatorManager ANIMATION => AnimationManager;
    #endregion


    private void Awake()
    {
        StateMachine = new StateMachine();
        EnemyMovement = new EnemyMovement(Rigidbody, NavMeshAgent, EnemyConfiguration);

        // Elegir el tipo de enemigo
        switch (enemyType)
        {
            case EnemyType.flashyEnemy:
                States = new FlashyStateCollection(this);

                EnemyCollision.detectedPlayer = GameObject.FindGameObjectWithTag("Player").transform;
                break;

            case EnemyType.birbEnemy:
                States = new BirbStateCollection(this);
                break;

            case EnemyType.petriEnemy:
                States = new PetriStateCollection(this);
                break;

            default:
                break;
        }


        States.Start(StateMachine);
    }

    void Start()
    {
        mainState = States.mainState;

        StateMachine.ChangeState(mainState);
    }

    void Update() => StateMachine.Update();

    void FixedUpdate() => StateMachine.FixedUpdate();

    void LateUpdate() => StateMachine.LateUpdate();
}