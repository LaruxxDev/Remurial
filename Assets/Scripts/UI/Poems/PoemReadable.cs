using UnityEditor.Build.Pipeline;
using UnityEngine;

public class PoemReadable : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerGeneral player;
    [SerializeField] private GameObject poem;
    public static GameObject poema { get; private set; }


    [Header("Variables")]
    [SerializeField] private string description;

    private bool isReading = false;
    public bool isInspectable => false;

    public string GetInteractText() => description;

    public void Interact(GameObject interactor)
    {
        isReading = true;
        poema.SetActive(isReading);

        PlayerGeneral PLAYER = interactor.transform.GetComponentInChildren<PlayerGeneral>();
        if (PLAYER != null)        
            PLAYER.STATEMACHINE.ChangeState(PLAYER.STATES.ReadingState(PLAYER.STATEMACHINE));      
    }

    public bool UseItem(GameObject item) => false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poema = poem;
    }
}
