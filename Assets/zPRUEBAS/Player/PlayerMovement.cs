using UnityEngine;
using System.Collections;

public class PlayerMovement
{
    private Rigidbody _rigidbody;
    private PlayerConfiguration _config;
    private PlayerGeneral _player;

    private Vector2 _moveInput;
    public Vector2 MoveInput => _moveInput;
    
    public PlayerMovement(Rigidbody rigidbody, PlayerConfiguration config, PlayerGeneral player)
    {
        _rigidbody = rigidbody;
        _config    = config;
        _player    = player;
    }

    // ── API pública que llaman los estados ──────────────────

    public void SetMoveInput(Vector2 input) => _moveInput = input;

    public void MoveThirdPerson()
    {
        float turn = _moveInput.x * _config.TurnSpeed  * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);

        Vector3 moveDirection = _player.transform.forward * _moveInput.y * _config.MoveSpeed;
        _rigidbody.linearVelocity = new Vector3(
            moveDirection.x,
            _rigidbody.linearVelocity.y,
            moveDirection.z
        );
    }

    public void MoveFirstPerson()
    {
        if (_player.mainCamera == null) return;

        Vector3 forward = _player.mainCamera.forward;
        Vector3 right   = _player.mainCamera.right;
        forward.y = 0f;
        right.y   = 0f;
        forward.Normalize();
        right.Normalize();

        float speed = _config.CameraMoveSpeed;
        Vector3 moveDirection = (forward * _moveInput.y + right * _moveInput.x).normalized;
        _rigidbody.linearVelocity = new Vector3(
            moveDirection.x * speed,
            _rigidbody.linearVelocity.y,
            moveDirection.z * speed
        );

        // Rotar cuerpo hacia donde mira la cámara
        Vector3 camForwardFlat = _player.mainCamera.forward;
        camForwardFlat.y = 0f;

        if (camForwardFlat.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(camForwardFlat);
            _rigidbody.MoveRotation(Quaternion.Slerp(
                _rigidbody.rotation,
                targetRotation,
                Time.fixedDeltaTime * 15f
            ));
        }
    }

    public void StopMovement()
    {
        _rigidbody.linearVelocity = new Vector3(0f, _rigidbody.linearVelocity.y, 0f);
    }

    // ── Flash ────────────────────────────────────────────────

    public void Flash()
    {
        _player.StartCoroutine(FlashRoutine(0.2f));
    }

    private IEnumerator FlashRoutine(float delay)
    {
        _player.flashObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        _player.flashObject.SetActive(false);
    }
}