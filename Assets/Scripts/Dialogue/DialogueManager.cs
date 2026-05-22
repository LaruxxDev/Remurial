using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Singleton")]
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [Header("References")]
    [Header("Dialogue")]
    [SerializeField] private GameObject dialoguePanel;      // Objeto padre
    [SerializeField] private TextMeshProUGUI textComponent; // Texto
    [SerializeField] private GameObject arrowObject;        // Flecha

    [Header("Image")]
    [SerializeField] private GameObject imagePanel; // Objeto padre
    [SerializeField] private Image imageComponent;  // Imagen

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;


    [Space]
    [Header("Variables")]
    [SerializeField] private float baseSpeed;   // Velocidad estándar
    private float[] speeds;                     // Lista de velocidades
    private string[] lines;                     // Lineas de diálogo
    private Sprite[] portraits;                 // Lista de retratos
    private AudioClip[] sounds;                 // Lista de sonidos

    // Variables varias
    private int index;
    private bool isTyping = false;

    // Acciones
    private Coroutine typingCoroutine;
    public event Action OnDialogueEnd;





    public void StartDialogue(string[] dialogueLines, float[] lineSpeeds = null, Sprite[] linePortrait = null, AudioClip[] lineSound = null)
    {
        // Lineas de dialogo
        lines = dialogueLines;
        index = 0;

        // Velocidad
        speeds = new float[dialogueLines.Length];

        // Retrato
        portraits = new Sprite[dialogueLines.Length];

        // Sonido
        sounds = new AudioClip[dialogueLines.Length];

        // Asignación general
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            // Velocidad
            speeds[i] = (lineSpeeds != null && lineSpeeds[i] > 0f) ? lineSpeeds[i] : baseSpeed;

            // Retrato
            portraits[i] = linePortrait != null ? linePortrait[i] : null;

            // Sonido
            sounds[i] = lineSound != null ? lineSound[i] : null;
        }

        // Preparar el objeto en escena
        dialoguePanel.SetActive(true);
        textComponent.text = string.Empty;

        typingCoroutine = StartCoroutine(TypeLine());
    }

    // Hub de inputs
    public void HandleInput()
    {
        if (isTyping)
            SkipLine();
        else
            NextLine();
    }
    
    // Skip de línea
    private void SkipLine()
    {
        if (typingCoroutine != null) 
            StopCoroutine(typingCoroutine);

        textComponent.text = lines[index];
        arrowObject.SetActive(true);
        isTyping = false;
    }

    // Siguiente línea
    private void NextLine()
    {
        if (index < lines.Length - 1)   // Siguiente línea
        {
            index++;
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeLine());
        }
        else                            // Terminar diálogo
        {
            dialoguePanel.SetActive(false);
            OnDialogueEnd?.Invoke();
        }
    }

    // Escribir la línea
    private IEnumerator TypeLine()
    {
        if (portraits[index] != null)
        {
            imageComponent.sprite = portraits[index];
            imagePanel.gameObject.SetActive(true);
        }
        else
        {
            imagePanel.gameObject.SetActive(false);
        }

        isTyping = true;
        arrowObject.SetActive(false);
        textComponent.text = string.Empty;

        // Caracter a caracter
        foreach (char c in lines[index])
        {
            textComponent.text += c;    // Texto

            if (sounds[index] != null)
                audioSource.PlayOneShot(sounds[index]); // Sonido

            yield return new WaitForSecondsRealtime(speeds[index]);
        }

        isTyping = false;
        arrowObject.SetActive(true);
    }
}
