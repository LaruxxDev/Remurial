using System.Collections;
using System.Linq;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light primary_light;
    [SerializeField] private Light secondary_light;
    [SerializeField] private MeshRenderer primary_meshRenderer;
    [SerializeField] private MeshRenderer secondary_meshRenderer;

    [Header("Speed")]
    public float flickerSpeed = 0.1f;
    public float minSpeed = 0.1f;
    public float maxSpeed = 0.2f;


    [Header("Intensity")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 5.0f;


    [Header("Range")]
    public float rangeSpeed = 0.1f;
    public float minRange = 1f;
    public float maxRange = 5f;

    private float _minRange;
    private float _maxRange;


    private void Start()
    {
        StartFlicker();
    }

    private void StartFlicker()
    {
        flickerSpeed = Random.Range(minSpeed, maxSpeed);

        Invoke("StartFlicker", flickerSpeed);

        Flicker();
    }

    private void Flicker()
    {


        // Intensidad
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        primary_light.intensity = randomIntensity;
        secondary_light.intensity = randomIntensity;


        // Modificar Valores
        _maxRange = maxRange / 100;
        _minRange = minRange / 100;

        // Acceder al color
        Color c = primary_meshRenderer.material.color;
        c.a = Random.Range(_minRange, _maxRange);

        // Modificar el color
        primary_meshRenderer.material.color = c;
        secondary_meshRenderer.material.color = c;

        //primary_meshRenderer.sharedMaterial.color.a
        // Retocar::
        // Rango
        //float randomRange = Random.Range(minRange, maxRange);
        //light.range = randomRange;
    }
}