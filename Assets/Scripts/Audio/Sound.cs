using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Audio/Sound")]
public class Sound : ScriptableObject
{
    public string name;
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;
    
    public bool loop;
    public AudioMixerGroup mixerGroup;

    [Header("Opciones 3D")]
    [Range(0f, 1f)]
    public float spatialBlend = 0f; // 0 = 2D, 1 = 3D
    public float minDistance = 1f;  // Distancia a la que el sonido está al máximo
    public float maxDistance = 50f; // Distancia a la que el sonido deja de escucharse
}