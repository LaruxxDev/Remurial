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


    #region General
    private Vector3 MoveDirection(Vector2 inputVec, Transform cameraTransform)
    {
        // Forward
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // Right
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        // Movement via Camera
        Vector3 moveDirection = (cameraForward * inputVec.y) + (cameraRight * inputVec.x);
        moveDirection.Normalize();

        return moveDirection;
    }

    public void PlayerRotation()
    {
        Vector3 velocityWithoutY = new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z);
        if (velocityWithoutY != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocityWithoutY.normalized, Vector3.up);
            Quaternion newRotation = Quaternion.Euler(0f, Quaternion.Lerp(rigidbody.rotation, targetRotation, PlayerConfiguration.TURNSPEED * Time.deltaTime).eulerAngles.y, 0f);
            rigidbody.MoveRotation(newRotation);
        }
    }
    #endregion

    #region Movement
    public void VelocityMovement(Vector2 inputVec, Transform cameraTransform)
    {
        // Movement with Speed Modifier
        Vector3 movement = MoveDirection(inputVec, cameraTransform) * PlayerConfiguration.MOVESPEED;

        rigidbody.linearVelocity = movement;

        // Rotation
        PlayerRotation();
    }

    public void VelocityIdle()
    {
        rigidbody.linearVelocity = Vector3.zero;
    }
    #endregion
}