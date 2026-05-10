using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private PlayerCollision player;

    private void OnTriggerEnter(Collider other) => player.OnInteractionEnter(other);
    private void OnTriggerExit(Collider other) => player.OnInteractionExit(other);
}
