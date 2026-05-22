using UnityEngine;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Lines")]
    [SerializeField] private List<DialogueEntry> dialogues;
    [SerializeField] private float textSpeed;


    #region Entry
    // Position
    private void OnTriggerEnter(Collider other)
    {
        var player = other.transform.parent?.GetComponentInChildren<PlayerGeneral>();
        if (player != null)
        {
            int index = GetAvailableDialogueIndex(DialogueEntry.EntryType.OnTrigger);

            if (index >= 0)
            TryPlayDialogue(player, index);      
        }
    }    
    
    
    // Items / NPCs
    public void Interact(PlayerConfiguration configuration)
    {
        PlayerGeneral player = configuration.transform.parent.GetComponentInChildren<PlayerGeneral>();

        int index = GetAvailableDialogueIndex(DialogueEntry.EntryType.OnInteract);
        if (index >= 0)
            TryPlayDialogue(player, index);
    }
    #endregion

    #region Dialogue Actions
    // Devuelve el índice del dialogo
    private int GetAvailableDialogueIndex(DialogueEntry.EntryType source)
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            DialogueEntry entry = dialogues[i];
            bool sourceMatches = entry.entryType == source || entry.entryType == DialogueEntry.EntryType.OnBoth;

            bool available = !entry.triggered || entry.canRetrigger;

            if (sourceMatches && available)
                return i;
        }

        return -1;
    }


    // Detecta si puede o no empezar el dialogo
    public void TryPlayDialogue(PlayerGeneral player, int index)
    {
        if (index < 0 || index >= dialogues.Count)
            return;

        DialogueEntry entry = dialogues[index];

        if (entry.triggered && !entry.canRetrigger)
            return;

        if (!entry.canRetrigger)
            entry.triggered = true;

        StartDialogue(player, entry.data);
    }


    // Asigna las variables según el dialogo seleccionado
    private async void StartDialogue(PlayerGeneral player, DialogueData dialogueData)
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
    #endregion
}

[System.Serializable]
public class DialogueEntry
{
    public DialogueData data;

    [Tooltip("Can be retriggered multiple times.")]
    public bool canRetrigger;
    [HideInInspector] public bool triggered = false;

    public EntryType entryType;
    public enum EntryType
    {
        OnTrigger,
        OnInteract,
        OnBoth
    }
}