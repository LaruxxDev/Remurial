using UnityEngine;

public class DestruirPared : MonoBehaviour, IInteractable
{
    [Header("Pared a destruir")]
    [SerializeField] private GameObject pared;

    [Header("Configuración del martillo")]
    [SerializeField] private string itemTag = "Martillo";
    [SerializeField] private string itemNameContains = "martillo";
    [SerializeField] private PlayerGeneral PLAYER;

    public bool isInspectable => false;

    public void Interact(GameObject interactor)
    {
        if (interactor == null)
        {
            Debug.Log("Necesitas un martillo para destruir esta pared.");
            return;
        }

        if (PLAYER == null)
        {
            Debug.Log("Necesitas un martillo para destruir esta pared.");
            return;
        }

        if (PLAYER.heldPosition != null && PLAYER.heldPosition.childCount > 0)
        {
            GameObject objetoEnMano = PLAYER.heldPosition.GetChild(0).gameObject;
            Debug.Log($"Intentando usar objeto en mano: {objetoEnMano.name}");

            if (UseItem(objetoEnMano))
            {
                Destruir();
                objetoEnMano.SetActive(false); // Simula que el martillo se rompe al usarlo
                
                return;
            }
        }

        Debug.Log("Necesitas un martillo para destruir esta pared.");
    }

    public bool UseItem(GameObject item)
    {
        if (item == null) return false;

        string lowerName = item.name.ToLower();
        bool isHammerByTag = item.CompareTag(itemTag);
        bool isHammerByName = lowerName.Contains(itemNameContains.ToLower());

        return isHammerByTag || isHammerByName;
    }

    public string GetInteractText() => "Necesitas un martillo";

    private void Destruir()
    {
        if (pared != null)
        {
            pared.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
