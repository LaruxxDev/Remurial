using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    
    // Lista para reciclar nuestros reproductores (Object Pool)
    private List<AudioSource> audioPool = new List<AudioSource>();
    public int poolSize = 10; // Cantidad de sonidos simultáneos que permites

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);

        // Pre-creamos los emisores de audio (vacíos) al iniciar el juego
        for (int i = 0; i < poolSize; i++)
        {
            GameObject emitter = new GameObject("AudioEmitter_" + i);
            emitter.transform.SetParent(this.transform);
            AudioSource source = emitter.AddComponent<AudioSource>();
            
            // Configuración óptima para 3D en Unity
            source.rolloffMode = AudioRolloffMode.Linear; 
            
            audioPool.Add(source);
        }
    }

    // Método para reproducir sonidos 2D (Música, UI)
    public void Play2D(string name)
    {
        PlaySoundAtPosition(name, Vector3.zero, false);
    }

    // Método para reproducir sonidos 3D en una posición específica
    public void Play3D(string name, Vector3 position)
    {
        PlaySoundAtPosition(name, position, true);
    }

    private void PlaySoundAtPosition(string name, Vector3 position, bool is3D)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sonido no encontrado: " + name);
            return;
        }

        // Buscar un AudioSource libre en el Pool
        AudioSource availableSource = audioPool.Find(source => !source.isPlaying);

        if (availableSource != null)
        {
            // Si es 3D, movemos el emisor a la posición. Si no, lo dejamos donde está.
            if (is3D) availableSource.transform.position = position;

            // Aplicamos las configuraciones del sonido
            availableSource.clip = s.clip;
            availableSource.volume = s.volume;
            availableSource.pitch = s.pitch * UnityEngine.Random.Range(0.95f, 1.05f);
            availableSource.loop = s.loop;
            availableSource.outputAudioMixerGroup = s.mixerGroup;
            
            // Aplicamos la configuración 3D
            availableSource.spatialBlend = s.spatialBlend;
            availableSource.minDistance = s.minDistance;
            availableSource.maxDistance = s.maxDistance;

            availableSource.Play();
        }
        else
        {
            Debug.LogWarning("No hay suficientes AudioSources en el Pool. ¡Sube el PoolSize!");
        }
    }
}