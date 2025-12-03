using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerMovement : MonoBehaviour
{
    [SerializeField] private float _jumpHight = 5.0f;
    [SerializeField] private float _movementSpeed = 10.0f;
    [SerializeField] private float _airMovementSpeed = 5.0f;
    public float gravityScale = 1.0f;

    private Vector2 _inputDirection;
    private Rigidbody _rb;
    public int collisions = 0;
    private bool _jumping;

    private float _jumpTimer;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {

        _rb.AddForce(new Vector3(0, -gravityScale * Time.deltaTime, 0));


        _jumpTimer += Time.deltaTime;

        if (collisions >= 1 && _jumping && _jumpTimer >= 0.2)
        {
            _jumpTimer = 0;
            _rb.AddForce(Vector3.up * _jumpHight, ForceMode.Impulse);
        }

        if (collisions == 0)
        {
            _rb.AddForce(transform.forward * _inputDirection.y * _airMovementSpeed * Time.deltaTime);
            _rb.AddForce(transform.right * _inputDirection.x * _airMovementSpeed * Time.deltaTime);

            _rb.linearDamping = 2;
        }
        else
        {

            _rb.AddForce(transform.forward * _inputDirection.y * _movementSpeed * Time.deltaTime);
            _rb.AddForce(transform.right * _inputDirection.x * _movementSpeed * Time.deltaTime);
            _rb.linearDamping = 5;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            _inputDirection = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            _jumping = true;
        }
        else if (context.canceled)
        {
            _jumping = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisions--;
    }
}
