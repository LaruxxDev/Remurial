using System.Collections;
using UnityEngine;

public class Tp : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform destino;
    [SerializeField] private float tiempoEnNegro = 1f; // Tiempo extra que se queda en negro antes de volver
    [SerializeField] private GameObject player;
    private bool estaTeletransportando = false;

    public void Interact(GameObject interactor)
    {
        // Evitamos que se use el TP varias veces seguidas si ya está en proceso
        if (estaTeletransportando) return;

        StartCoroutine(ProcesoTeletransporte(interactor));
    }

    private IEnumerator ProcesoTeletransporte(GameObject interactor)
    {
        estaTeletransportando = true;

        // 1. Iniciamos el fundido a negro
        GameManager.Instancia.HacerBlackout();

        // 2. Esperamos a que la pantalla esté negra (aprox 1 segundo o lo que dure tu fade)
        yield return new WaitForSeconds(1.0f); 

        // ── AQUÍ PODEMOS LA ANIMACIÓN EN EL FUTURO ──
        // yield return new WaitForSeconds(duracionDeAnimacion);
        // ────────────────────────────────────────────────

        // 3. Movemos al jugador
        interactor.transform.position = destino.position;

        // 4. Pequeña espera de seguridad para que la cámara de Cinemachine se asiente
        yield return new WaitForSeconds(tiempoEnNegro);

        // 5. Quitamos el negro
        GameManager.Instancia.QuitarBlackout();

        estaTeletransportando = false;
    }

    public string GetInteractText() => $"Teletransportarse a {destino.name}";
}