using UnityEngine;
using UnityEngine.Localization;


[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public LocalizedString line;                            // Texto
        [Range(0.01f, 0.5f)] public float textSpeed = 0.05f;    // Velocidad

        public Sprite portrait;                                 // Imagen
        public AudioClip sound;                                 // Sonido
    }

    public DialogueLine[] lines;
}