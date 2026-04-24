using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OpcionesManager : MonoBehaviour
{

    [Header("Referencia al Global Volume")]
    [SerializeField] private Volume postProcessVolume;


    [Header("Audio")]
    //[SerializeField] private AudioSource audioSource;

    [Header("UI")]
    private ColorAdjustments _colorAdjustments;
    private Slider _volumeSlider;
    private Slider _brilloSlider;
    private Button _exitButton;
    private Button _resumeButton;
    private VisualElement _root;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _volumeSlider = _root.Q<Slider>("VolumeSlider");
        _brilloSlider = _root.Q<Slider>("BrilloSlider");
        _exitButton = _root.Q<Button>("ExitButton");
        _resumeButton = _root.Q<Button>("ResumeButton");

        _volumeSlider.RegisterValueChangedCallback(OnVolumeChanged);
        _brilloSlider.RegisterValueChangedCallback(OnBrilloChanged);
        _exitButton.clicked += OnExitClicked;
        _resumeButton.clicked += OnResumeClicked;   

        if (postProcessVolume != null && postProcessVolume.profile.TryGet<ColorAdjustments>(out var ca))
        {
            _colorAdjustments = ca;
        }
        else
        {
            Debug.LogError("Asigna el Global Volume al inspector y añade el efecto Color Adjustments.");
        }
    }

    private void OnVolumeChanged(ChangeEvent<float> evt)
    {
        Debug.Log("Volume Changed: " + evt.newValue);
        // audioSource.volume = evt.newValue;
    }

    private void OnBrilloChanged(ChangeEvent<float> evt)
    {
        UpdateBrillo(evt.newValue);
    }

    private void UpdateBrillo(float valor)
    {
        Debug.Log("Brillo actualizado a: " + valor);
        if (_colorAdjustments != null)
        {
            // Modificamos el valor de exposición directamente
            _colorAdjustments.postExposure.value = valor;
            Debug.Log("Brillo actual: " + valor);
        }

    }

    private void OnExitClicked()
    {
        Debug.Log("Exit Clicked");
        SceneManager.LoadScene("MainMenuScene");
        
    }

    private void OnResumeClicked()
    {
        Debug.Log("Resume Clicked");
        this.gameObject.SetActive(false);
    }

}
