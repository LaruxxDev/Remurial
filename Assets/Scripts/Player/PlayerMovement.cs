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
        // Movement
        Vector3 movement = playerRigidbody.transform.forward * inputVec.y * PlayerConfiguration.MOVESPEED;

        playerRigidbody.linearVelocity = new Vector3(
            movement.x,
            playerRigidbody.linearVelocity.y,
            movement.z
        );

        // Rotation
        float rotation = inputVec.x * PlayerConfiguration.TURNSPEED * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);

        playerRigidbody.MoveRotation(playerRigidbody.rotation * deltaRotation);
    }


    public void VelocityCamera(Vector2 inputVec, Transform cameraTransform)
    {
        // Movement
        Vector3 forward = cameraTransform.forward * inputVec.y;
        Vector3 right = cameraTransform.right * inputVec.x;
        Vector3 movement = (forward + right).normalized * PlayerConfiguration.CAMERAMOVESPEED;

        playerRigidbody.linearVelocity = new Vector3(
            movement.x,
            playerRigidbody.linearVelocity.y,
            movement.z
        );

        // Rotation
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

    public void Flash()
    {
        PLAYER.StartCoroutine(FlashRoutine(0.2f));
    }

    private IEnumerator FlashRoutine(float delay)
    {
        PLAYER.flashObject.SetActive(true);

        yield return new WaitForSeconds(delay);

        PLAYER.flashObject.SetActive(false);
    }

    #endregion
}