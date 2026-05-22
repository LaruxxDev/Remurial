using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private PlayerCollision player;


    private void Start()
    {
        if (player == null)
            player = FindAnyObjectByType<PlayerCollision>();    
    }

    private void OnTriggerEnter(Collider other) => player.OnInteractionEnter(other);
    private void OnTriggerExit(Collider other) => player.OnInteractionExit(other);

    private void Update()
    {
        if (player.currentInteractable != null && player.interactableItem == null)
            player.ClearInteractable();
    }
}
