using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    [Header("Lines")]
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private float textSpeed;

    [Header("Bool")]
    [Tooltip("Can be retriggered multiple times.")]
    [SerializeField] private TriggerType triggerType;
    public enum TriggerType
    {
        onInteract,
        onEnter,
        both
    }

    [SerializeField] private bool canRetrigger;
    [Space]
    [SerializeField] private bool triggered = false;

    [Header("Interaction")]
    [SerializeField] private string interactText;
    public bool isInspectable => false;


    // Position
    private void OnTriggerEnter(Collider other)
    {
        if (triggered || triggerType == TriggerType.onInteract)
            return;

        var player = other.transform.parent?.GetComponentInChildren<PlayerGeneral>();
        if (player != null)
        {
            if (!canRetrigger)
                triggered = true;

            StartDialogue(player);
        }
    }

    // Items / NPCs
    //public void Interact(PlayerGeneral player) => StartDialogue(player);
    public void Interact(GameObject interactor)
    {
        if (triggered || triggerType == TriggerType.onEnter)
            return;

        PlayerGeneral player = interactor.GetComponentInChildren<PlayerGeneral>();

        if (player != null)
        {
            StartDialogue(player);
        }
    }

    private async void StartDialogue(PlayerGeneral player)
    {
        string[] resolvedLines = new string[dialogueData.lines.Length]; // Texto
        float[] speeds = new float[dialogueData.lines.Length];          // Velocidad
        Sprite[] portraits = new Sprite[dialogueData.lines.Length];     // Retrato
        AudioClip[] sounds = new AudioClip[dialogueData.lines.Length];  // Sonido

        for (int i = 0; i < dialogueData.lines.Length; i++)
        {
            resolvedLines[i] = await dialogueData.lines[i].line.GetLocalizedStringAsync().Task;
            speeds[i] = dialogueData.lines[i].textSpeed;
            portraits[i] = dialogueData.lines[i].portrait;
            sounds[i] = dialogueData.lines[i].sound;
        }


        DialogueManager.Instance.StartDialogue(resolvedLines, speeds, portraits, sounds);
        player.STATEMACHINE.ChangeState(player.STATES.DialogueState(player.STATEMACHINE));
    }



    public bool UseItem(GameObject item) => false;
    public string GetInteractText() => interactText;
}
