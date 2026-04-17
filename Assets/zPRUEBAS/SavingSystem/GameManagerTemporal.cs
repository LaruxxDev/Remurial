using UnityEngine;
using System.Collections;

[DefaultExecutionOrder(100)]
public class GameManagerTemporal : MonoBehaviour
{
    public static GameManagerTemporal instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
