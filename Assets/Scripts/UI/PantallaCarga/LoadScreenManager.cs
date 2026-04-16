
using UnityEngine;
using UnityEngine.UIElements;

public class LoadScreenManager : MonoBehaviour
{
    private VisualElement _spinner;
    private float rotationAngle = 0f;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _spinner = root.Q<VisualElement>("LoadingSpinner");
    }

    void Update()
    {
        if (_spinner != null)
        {
            Debug.Log("Ruedecita existe");
            // Aumentamos el ángulo (puedes ajustar la velocidad multiplicando por una variable)
            rotationAngle += Time.deltaTime * 360f; 
            
            // Aplicamos la rotación
            _spinner.style.rotate = new Rotate(new Angle(rotationAngle));
        }
    }
}
