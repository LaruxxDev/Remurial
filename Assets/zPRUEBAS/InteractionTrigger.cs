using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private PlayerCollision player;

    private void OnTriggerEnter(Collider other) => player.OnInteractionEnter(gameObject.GetComponent<Collider>());
    private void OnTriggerExit(Collider other) => player.OnInteractionExit(gameObject.GetComponent<Collider>());
}
