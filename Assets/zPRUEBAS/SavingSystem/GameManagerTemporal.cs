using UnityEngine;
using System.Collections;

[DefaultExecutionOrder(100)]
public class GameManagerTemporal : MonoBehaviour
{
    public static GameManagerTemporal Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitUntil(() => player != null);
        yield return new WaitForFixedUpdate();

        SaveSystem.Initialize();
    }


    [Header("Save Locations")]
    [SerializeField] private PlayerConfiguration player;
    public PlayerConfiguration PLAYER
    {
        get => player;
        set => player = value;
    }
}
