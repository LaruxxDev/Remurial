using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : MonoBehaviour
{
    // Brain
    static CinemachineBrain _brain;
    public static CinemachineBrain brain
    {
        get
        {
            if (_brain == null)
                _brain = Camera.main.GetComponent<CinemachineBrain>();
            return _brain;
        }
    }


    // Camera
    static List<CinemachineCamera> cameras = new List<CinemachineCamera>();
    public static CinemachineCamera activeCamera = null;


    // Blend Style
    public enum BlendStyle { Instant, Linear }



    // Cambia la cámara y el Blend Style
    public static void SwitchCamera(CinemachineCamera newCamera, BlendStyle style = BlendStyle.Linear, float duration = 1f)
    {
        // Define el translado de la cámara
        SetBlendStyle(style, duration);

        // Pone la cámara seleccionada como la prioritaria
        newCamera.Priority = 10;
        activeCamera = newCamera;

        // Desactiva todas las demás
        foreach (CinemachineCamera cam in cameras)
        {
            if (cam != newCamera)
            {
                cam.Priority = 0;
            }
        }
    }


    // Cambia el Blend Style
    private static void SetBlendStyle(BlendStyle style, float duration)
    {
        switch (style)
        {
            case BlendStyle.Instant:
                brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0f);
                break;

            case BlendStyle.Linear:
                brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Linear, duration);
                break;

            default:
                Debug.LogError("BlendStyle not allowed");
                break;
        }
    }


    public static bool IsActiveCamera(CinemachineCamera camera)
    {
        return camera == activeCamera;
    }


    public static void Register(CinemachineCamera camera)
    {
        cameras.Add(camera);
    }

    public static void Unregister(CinemachineCamera camera)
    {
        cameras.Remove(camera);
    }
}
