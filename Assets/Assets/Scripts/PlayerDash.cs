using System.Collections;
using DG.Tweening.Core.Easing;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private NewPlayerMovement _playerMovement;
    [SerializeField] private float _dashForce = 50f;
    [SerializeField] private float _airDashForce = 25f;
    [SerializeField] private float _slideForce = 25f;
    [SerializeField] private float _slamForce = 25f;
    [SerializeField] private float _pushDash = 25f;

    [SerializeField] private float _InitialDashPush = 10f;
    [SerializeField] private float _InitialAirDashPush = 5f;
    [SerializeField] private float _InitialSlidePush = 10f;

    private bool _isDashing = false;
    private bool _isSliding = false;

    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private Vector3 _forwardHold;
    private float _timer;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        // Debug.Log(_playerMovement.allowMovement);

        if (_isSliding == false && _capsuleCollider.height != 2) 
        {
            _capsuleCollider.height = 2f; 
        }

        if (_isDashing) { Dash(); }
        if (_isSliding) { Slide(); }
    }

    public void OnDash(InputAction.CallbackContext context)
    {

        if (context.started || context.performed)
        {
            Vector2 input = _playerMovement.inputDirection;

            Vector3 lookDirection = transform.right * input.x + transform.forward * input.y;


            if (lookDirection.sqrMagnitude < 0.001f)
            {
                lookDirection = transform.forward;
            }

            lookDirection.y = 0f;
            _forwardHold = lookDirection.normalized;

            if (_playerMovement.collisions > 0)
            {
                _rb.AddForce(_forwardHold * _InitialDashPush, ForceMode.Impulse);
            }
            else
            {
                _rb.AddForce(_forwardHold * _InitialAirDashPush, ForceMode.Impulse);
                _playerMovement.usedGravity = false;
            }

            _timer = 0f;
            _playerMovement.allowMovement = false;
            _rb.linearVelocity = new Vector3(0, 0, 0);
            _isDashing = true;
        }
    }

    public void OnSlide(InputAction.CallbackContext context)
    {

        if (context.started && _playerMovement.collisions > 0 || context.performed && _playerMovement.collisions > 0)
        {

            _capsuleCollider.height = 0.5f;

            Vector2 input = _playerMovement.inputDirection;

            Vector3 lookDirection = transform.right * input.x + transform.forward * input.y;


            if (lookDirection.sqrMagnitude < 0.001f)
            {
                lookDirection = transform.forward;
            }

            lookDirection.y = 0f;
            _forwardHold = lookDirection.normalized;

            _rb.AddForce(_forwardHold * _InitialSlidePush, ForceMode.Impulse);

            _playerMovement.allowMovement = false;
            _rb.linearVelocity = new Vector3(0, 0, 0);
            _isSliding = true;
        }

        else if (context.started && _playerMovement.collisions == 0 || context.performed && _playerMovement.collisions == 0) 
        {
            //Debug.Log("help");
            _rb.linearVelocity = Vector3.zero;
            _rb.AddForce(Vector3.down * _slamForce / 100, ForceMode.Impulse);
            _rb.AddForce(Vector3.down * _slamForce * Time.deltaTime, ForceMode.VelocityChange);
        }

        else if (context.canceled)
        {
            
            _isSliding = false;
            _playerMovement.allowMovement = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isDashing)
        {
            _playerMovement.allowMovement = true;
            _isDashing = false;
        }

        if (_isSliding)
        {
            //transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
            _isSliding = false;
            _playerMovement.allowMovement = true;
        }
        else
        {
            //transform.localScale = new Vector3(transform.localScale.x, 2f, transform.localScale.z);
        }
    }

    private void Dash()
    {
        _timer += Time.deltaTime;
        if (_timer >= 0.2f)
        {
            _rb.linearVelocity = new Vector3(0, 0, 0);
            if (_playerMovement.collisions == 0)
            {
                _rb.AddForce(_forwardHold * _pushDash, ForceMode.Impulse);
            }
            _playerMovement.allowMovement = true;
            _playerMovement.usedGravity = true;
            _isDashing = false;
        }

        if (_playerMovement.collisions > 0)
        {
            _rb.AddForce(_forwardHold * _dashForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {
            _rb.AddForce(_forwardHold * _airDashForce * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    private void Slide()
    {


        if (_playerMovement.collisions > 0)
        {
            _rb.AddForce(_forwardHold * _slideForce * Time.deltaTime, ForceMode.VelocityChange);
        }
       
    }
}
