using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ECGMonitor : VisualElement
{
    private List<Vector2> points = new List<Vector2>();
    private float timer = 0;
    public float healthPercent = 1.0f; // 1.0 = Verde, 0.5 = Amarillo, 0.1 = Rojo

    public ECGMonitor()
    {
        generateVisualContent += OnGenerateVisualContent;
        // Opcional: Evita que el mouse interfiera con el dibujo
        pickingMode = PickingMode.Ignore; 
    }
    public void Tick()
    {
        Debug.Log($"ECG Tick - Health: {healthPercent * 100}%");
        // El latido se acelera según baja la vida (multiplicador de 1x a 4x)
        float speedMultiplier = 1.0f + (1.0f - healthPercent) * 3.0f;
        timer += Time.deltaTime * speedMultiplier;
        
        float width = layout.width;
        float height = layout.height;
        if (float.IsNaN(width) || width <= 0) return; // Evitar errores antes del layout inicial

        // Generar el valor Y del latido (ECG real simulado)
        float y = GetECGValue(timer) * (height * 0.35f);
        
        // Insertar nuevo punto al inicio (izquierda)
        points.Insert(0, new Vector2(0, (height / 2) + y));

        // Mantener suficientes puntos para cubrir el ancho (basado en densidad)
        if (points.Count > 150) points.RemoveAt(points.Count - 1);

        MarkDirtyRepaint(); // Forzar a UIToolkit a redibujar este elemento
        Debug.Log($"ECG Points Count: {points.Count} - Latest Y: {y}");
    }

    private float GetECGValue(float t)
    {
        float x = t % 1.0f; // Ciclo normalizado de 1 seg
        // Simulación de complejo PQRST
        if (x < 0.1f) return Mathf.Sin(x * Mathf.PI * 10) * 0.15f;      // Onda P
        if (x < 0.15f) return 0;                                       // Segmento PR
        if (x < 0.20f) return -0.4f + (x-0.15f) * -2.0f;               // Onda Q
        if (x < 0.25f) return -1.0f + (x-0.20f) * 20.0f;               // Pico R (hacia arriba)
        if (x < 0.30f) return 1.0f - (x-0.25f) * 20.0f;                // Caída S
        if (x < 0.45f) return Mathf.Sin((x-0.3f) * Mathf.PI * 6) * 0.2f; // Onda T
        return 0; // Reposo
    }

    private void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        if (points.Count < 2) return;

        var painter = mgc.painter2D;
        painter.lineWidth = 2.5f;
        painter.lineCap = LineCap.Round;
        painter.strokeColor = GetColorByHealth();

        painter.BeginPath();
        painter.MoveTo(points[0]);

        float stepX = layout.width / 120f; // Espaciado entre puntos

        for (int i = 1; i < points.Count; i++)
        {
            painter.LineTo(new Vector2(i * stepX, points[i].y));
        }
        painter.Stroke();
    }

    private Color GetColorByHealth()
    {
        if (healthPercent > 0.65f) return new Color(0.2f, 1f, 0.2f); // Verde neón
        if (healthPercent > 0.30f) return new Color(1f, 0.8f, 0f);   // Naranja/Amarillo
        return new Color(1f, 0.1f, 0.1f);                             // Rojo
    }
}