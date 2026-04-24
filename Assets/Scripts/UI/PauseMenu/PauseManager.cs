using UnityEngine;
using UnityEngine.UIElements;

public class PauseManager : MonoBehaviour
{

     [Header("UI")]
    private Button _optionsButton;
    private Button _exitButton;
    private Button _resumeButton;
    private VisualElement _root;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _optionsButton = _root.Q<Button>("OptionsButton");
        _exitButton = _root.Q<Button>("ExitButton");
        _resumeButton = _root.Q<Button>("ResumeButton");

        _optionsButton.clicked += OnOptionsClicked;
        _exitButton.clicked += OnExitClicked;
        _resumeButton.clicked += OnResumeClicked; 
        
    }

    private void OnOptionsClicked()
    {
        Debug.Log("Options Clicked");
        // Aquí puedes cargar el menú de opciones o mostrarlo
    }

    private void OnExitClicked()
    {
        Debug.Log("Exit Clicked");
        SceneManager.LoadScene("MainMenuScene");
    }

    private void OnResumeClicked()
    {
        Debug.Log("Resume Clicked");
        // Aquí puedes implementar la lógica para reanudar el juego
    }

}
