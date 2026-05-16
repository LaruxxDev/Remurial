using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Lines")]
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private float textSpeed;

    [Header("Bool")]
    [Tooltip("Can be retriggered multiple times.")]
    [SerializeField] private bool canRetrigger;
    [Space]
    [SerializeField] private bool triggered = false;


    // Position
    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
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
    public void Interact(PlayerGeneral player) => StartDialogue(player);

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

}
