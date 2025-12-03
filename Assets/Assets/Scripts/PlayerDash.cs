using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private NewPlayerMovement _playerMovement;
    [SerializeField] private float _dashForce = 50f;
    [SerializeField] private float _airDashForce = 25f;

    private bool _isDashing = false;
    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isDashing)
        {
            if (_playerMovement.collisions > 0)
            {
                _rb.AddForce(transform.forward * _dashForce * Time.deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                _rb.AddForce(transform.forward * _airDashForce * Time.deltaTime, ForceMode.VelocityChange);
            }
            _rb.AddForce(new Vector3(0, -_playerMovement.gravityScale * Time.deltaTime, 0));
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {

        if (context.started || context.performed)
        {
            _isDashing = true;
        }
        else if (context.canceled)
        {
            _isDashing = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isDashing)
        {
            _isDashing = false;
        }
    }
}
