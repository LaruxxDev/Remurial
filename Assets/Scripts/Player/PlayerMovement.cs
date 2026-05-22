using UnityEngine;
using System.Collections;

public class PlayerMovement
{
    Rigidbody playerRigidbody;
    PlayerConfiguration PlayerConfiguration;
    PlayerGeneral PLAYER;

    public PlayerMovement(Rigidbody rigidbody, PlayerConfiguration playerConfiguration, PlayerGeneral player)
    {
        this.playerRigidbody = rigidbody;
        this.PlayerConfiguration = playerConfiguration;
        this.PLAYER = player;
    }

    #region Movement

    public void VelocityMovement(Vector2 inputVec, Transform cameraTransform)
    {
        Vector3 movement = playerRigidbody.transform.forward * inputVec.y * PlayerConfiguration.MOVESPEED;

        playerRigidbody.linearVelocity = new Vector3(
            movement.x,
            playerRigidbody.linearVelocity.y,
            movement.z
        );

        float rotation = inputVec.x * PlayerConfiguration.TURNSPEED * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);

        playerRigidbody.MoveRotation(playerRigidbody.rotation * deltaRotation);
    }

    public void VelocityCamera(Vector2 inputVec, Transform cameraTransform)
    {
        Vector3 movement = playerRigidbody.transform.forward * inputVec.y * PlayerConfiguration.CAMERAMOVESPEED;

        playerRigidbody.linearVelocity = new Vector3(
            movement.x,
            playerRigidbody.linearVelocity.y,
            movement.z
        );

        float rotation = inputVec.x * PlayerConfiguration.TURNSPEED * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);

        playerRigidbody.MoveRotation(playerRigidbody.rotation * deltaRotation);
    }

    public void VelocityIdle()
    {
        playerRigidbody.linearVelocity = Vector3.zero;
    }

    #endregion

    #region Actions
/*
    public void Flash()
    {
        PLAYER.StartCoroutine(FlashRoutine(0.2f));
    }

    private IEnumerator FlashRoutine(float delay)
    {
        PLAYER.flashObject.SetActive(true);

        yield return new WaitForSeconds(delay);

        PLAYER.flashObject.SetActive(false);
    }*/
    public void Flash()
    {
        PLAYER.StartCoroutine(FlashRoutine());
    }
    private IEnumerator FlashRoutine()
    {
        if (PlayerConfiguration.flashLight == null) yield break;
        if (PlayerConfiguration.flashArea != null)
            PlayerConfiguration.flashArea.SetActive(true);
        float tiempoPasado = 0f;
        Debug.Log("Flash terminado"+tiempoPasado);
        // Mientras el tiempo que ha pasado sea menor a la duracin que queremos...
        while (tiempoPasado < PlayerConfiguration.flashDuration)
        {
            tiempoPasado += Time.deltaTime;

            // Lerp mezcla dos valores. Va de intensidadMaximaFlash a 0 a lo largo del tiempo.
            PlayerConfiguration.flashLight.intensity = Mathf.Lerp(PlayerConfiguration.flashMaxIntensity, 0f, tiempoPasado / PlayerConfiguration.flashDuration);

            // Esperamos al siguiente frame para seguir bajando la intensidad
            yield return null;
        }
        Debug.Log("Flash terminado"+tiempoPasado);
        // Pasado ese tiempo, apagamos el efecto visual de golpe
        if (PlayerConfiguration.flashArea != null)
            PlayerConfiguration.flashArea.SetActive(false);

        // Potencia reseteada
        PlayerConfiguration.flashLight.intensity = PlayerConfiguration.flashMaxIntensity;
    }

    #endregion
}