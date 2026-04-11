using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    private VisualElement _root;
    private Button _newGameButton;
    private Button _loadGameButton;
    private Button _optionsButton;
    private Button _exitButton;

    void Start()
    {

        _root = GetComponent<UIDocument>().rootVisualElement;

        _newGameButton = _root.Q<Button>("NewGameButton");
        _loadGameButton = _root.Q<Button>("LoadGameButton");
        _optionsButton = _root.Q<Button>("OptionsButton");
        _exitButton = _root.Q<Button>("ExitButton");

        _newGameButton.clicked += OnNewGameClicked;
        _loadGameButton.clicked += OnLoadGameClicked;
        _optionsButton.clicked += OnOptionsClicked;
        _exitButton.clicked += OnExitClicked;
        
    }

    private void OnNewGameClicked()
    {
        Debug.Log("New Game Clicked");
        _root.style.display = DisplayStyle.None; // Oculta el menú principal
        SceneManager.LoadScene("SampleSceneUI"); // TODO: Cambia "SampleSceneUI" por el nombre de la escena definitiva
        _root.style.display = DisplayStyle.None;
    }

    private void OnLoadGameClicked()
    {
        Debug.Log("Load Game Clicked");
        // TODO: Implementar lógica de carga de partida 
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
    }

}
