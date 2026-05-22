using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class MainMenuManager : MonoBehaviour
{

    private VisualElement _root;
    private Button _newGameButton;
    private Button _loadGameButton;
    private Button _optionsButton;
    private Button _exitButton;
    public AudioClip hoverSound;
    private AudioSource _audioSource;
    public AudioClip clickSound;

    public GameObject loadingScreen;


    void Start()
    {

        DontDestroyOnLoad(gameObject);

        _root = GetComponent<UIDocument>().rootVisualElement;

        _newGameButton = _root.Q<Button>("NewGameButton");
        _loadGameButton = _root.Q<Button>("LoadGameButton");
        _optionsButton = _root.Q<Button>("OptionsButton");
        _exitButton = _root.Q<Button>("ExitButton");

        _newGameButton.clicked += OnNewGameClicked;
        _loadGameButton.clicked += OnLoadGameClicked;
        _optionsButton.clicked += OnOptionsClicked;
        _exitButton.clicked += OnExitClicked;

        _audioSource = gameObject.AddComponent<AudioSource>();

        //AudiosHover
        _newGameButton.RegisterCallback<MouseEnterEvent>(evt => _audioSource.PlayOneShot(hoverSound));
        _loadGameButton.RegisterCallback<MouseEnterEvent>(evt => _audioSource.PlayOneShot(hoverSound));
        _optionsButton.RegisterCallback<MouseEnterEvent>(evt => _audioSource.PlayOneShot(hoverSound));
        _exitButton.RegisterCallback<MouseEnterEvent>(evt => _audioSource.PlayOneShot(hoverSound));
        //AudiosClick
        _newGameButton.RegisterCallback<ClickEvent>(evt => _audioSource.PlayOneShot(clickSound));
        _loadGameButton.RegisterCallback<ClickEvent>(evt => _audioSource.PlayOneShot(clickSound));
        _optionsButton.RegisterCallback<ClickEvent>(evt => _audioSource.PlayOneShot(clickSound));
        _exitButton.RegisterCallback<ClickEvent>(evt => _audioSource.PlayOneShot(clickSound));

        _loadGameButton.SetEnabled(SaveExists());

        if (loadingScreen != null) loadingScreen.SetActive(false);
    }

    private bool SaveExists()
    {
        return File.Exists(SaveSystem.SaveFileName());
    }

    private void StartGame()
    {
        if (loadingScreen != null) loadingScreen.SetActive(true);
        _root.style.display = DisplayStyle.None;
        StartCoroutine(LoadSceneAsync("MapaScene"));
    }

    private void OnNewGameClicked()
    {
    
        Debug.Log("New Game Clicked");
        if (SaveExists())
            File.Delete(SaveSystem.SaveFileName());
        StartGame();
        
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Esto inicia la carga en segundo plano
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Evitamos que la escena se active automáticamente al llegar al 100%
        operation.allowSceneActivation = false;

        // 2. Tiempo mínimo de espera (ejemplo: 3 segundos)
        float timer = 0f;
        float minWaitTime = 3f; 

        // 3. El bucle se mantiene mientras no termine de cargar O no pase el tiempo mínimo
        while (operation.progress < 0.9f || timer < minWaitTime)
        {
            timer += Time.deltaTime;
            
            // Debug para ver cómo progresa el tiempo en consola
            // Debug.Log($"Cargando... Tiempo transcurrido: {timer:F2}s");

            yield return null; 
        }

        // 4. Una vez cumplido el tiempo y la carga, activamos la escena
        Debug.Log("Tiempo mínimo cumplido, cambiando de escena...");
        operation.allowSceneActivation = true;
    }

    private void OnLoadGameClicked()
    {
        Debug.Log("Load Game Clicked");
        StartGame();
    }

    private void OnOptionsClicked()
    {
        Debug.Log("Options Clicked");
        SceneManager.LoadScene("OptionsScene"); //TODO
    }

    private void OnExitClicked()
    {
        Debug.Log("Exit Clicked");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Para que funcione en el editor
#endif
    }

}
