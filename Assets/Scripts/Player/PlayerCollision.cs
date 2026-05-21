using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    #region Ground
    [Header("Ground")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDistance;
    [SerializeField] bool groundGizmoz;

    public bool GROUND => Physics.Raycast(groundCheck.position, -transform.up, -groundDistance, groundLayer);
    #endregion

    #region Interaction
    [Header("Interaction")]
    public IInteractable currentInteractable { get; private set; }
    public GameObject interactableItem { get; private set; }

    public bool INTERACT => currentInteractable != null;


    public void OnInteractionEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            interactableItem = other.tag == "Tp" ? this.gameObject : other.gameObject;

            currentInteractable = interactable;

            if (HudManager.Instance != null)
                HudManager.Instance.MostrarMensaje(currentInteractable.GetInteractText());
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (currentInteractable == null)
        {
            ClearInteractable();
            return;
        }

        if (other.TryGetComponent<IInteractable>(out var interactable) && interactable == currentInteractable)
        {
            ClearInteractable();
        }
    }

    public void ClearInteractable()
    {
        currentInteractable = null;
        interactableItem = null;

        //if (HudManager.Instance != null)
        //    HudManager.Instance.OcultarMensaje();
    }

    public void DestroyInteractable()
    {
        Destroy(interactableItem);
        ClearInteractable();
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (groundGizmoz)
        {
            if (GROUND) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Gizmos.DrawRay(groundCheck.position, -transform.up * -groundDistance);
        }
    }
}
