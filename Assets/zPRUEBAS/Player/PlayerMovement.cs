using UnityEngine;


public class PlayerMovement
{
    Rigidbody rigidbody;
    PlayerConfiguration PlayerConfiguration;

    public PlayerMovement(Rigidbody rigidbody, PlayerConfiguration playerConfiguration)
    {
        this.rigidbody = rigidbody;
        this.PlayerConfiguration = playerConfiguration;
    }

    #region Movement
    public void VelocityMovement(Vector2 inputVec, Transform cameraTransform)
    {
        // Forward Axis
        Vector3 movement = rigidbody.transform.forward * inputVec.y * PlayerConfiguration.MOVESPEED;

        rigidbody.linearVelocity = new Vector3(movement.x, rigidbody.linearVelocity.y, movement.z);

        // Right Axis
        float rotation = inputVec.x * PlayerConfiguration.TURNSPEED * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);

        rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
    }

    public void VelocityIdle()
    {
        rigidbody.linearVelocity = Vector3.zero;
    }
    #endregion
}