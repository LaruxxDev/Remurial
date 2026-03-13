using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    static List<CinemachineCamera> cameras = new List<CinemachineCamera>();
    public static CinemachineCamera activeCamera = null;


    public static bool IsActiveCamera(CinemachineCamera camera)
    {
        return camera == activeCamera;
    }

    public static void SwitchCamera(CinemachineCamera newCamera)
    {
        newCamera.Priority = 10;
        activeCamera = newCamera;

        foreach (CinemachineCamera cam in cameras)
        {
            if (cam != newCamera)
            {
                cam.Priority = 0;
            }
        }
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
