using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float maxSpeed = 100;
    [SerializeField] private float JumpHight = 5.0f;
    [SerializeField] private float dampaning = 5.0f;
    private Rigidbody rb;

    private Vector3 movementImput;
    private Vector2 inputDirectionPrev;
    private Vector2 inputDirection;

    private int collisions;

    // Used by SmoothDamp to store current velocity for smoothing
    private Vector3 smoothVelocityRef = Vector3.zero;

    // Camera transform used to make movement camera-relative
    private Transform cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (Camera.main != null) cam = Camera.main.transform;
    }

    void FixedUpdate()
    {
        // Build a world-space target horizontal velocity from input relative to the camera (or fallback to world axes)
        Vector3 targetHorizontal;
        if (cam != null)
        {
            // Project camera forward/right onto XZ plane
            Vector3 camForward = cam.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = cam.right;
            camRight.y = 0f;
            camRight.Normalize();

            Vector3 moveDir = camRight * inputDirection.x + camForward * inputDirection.y;
            if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();
            targetHorizontal = moveDir * speed;
        }
        else
        {
            // Fallback: world-relative movement (existing behavior)
            targetHorizontal = new Vector3(inputDirection.x, 0f, inputDirection.y) * speed;
        }

        // Current full velocity
        var current = rb.linearVelocity;

        // If there's no input, target is zero horizontal velocity
        if (inputDirection.sqrMagnitude == 0f)
        {
            targetHorizontal = Vector3.zero;
        }

        Vector3 newHorizontal;

        // If grounded (collisions >= 1) or dampaning == 0 -> instant change
        if (collisions >= 1 || dampaning <= 0f)
        {
            newHorizontal = targetHorizontal;
            // reset the smooth ref so when we next start smoothing we don't get a jump
            smoothVelocityRef = Vector3.zero;
        }
        else
        {
            // SmoothDamp: smoothTime = dampaning (seconds). Larger dampaning -> slower convergence.
            newHorizontal = Vector3.SmoothDamp(
                new Vector3(current.x, 0f, current.z),
                targetHorizontal,
                ref smoothVelocityRef,
                dampaning,
                Mathf.Infinity,
                Time.fixedDeltaTime
            );
        }

        // Clamp horizontal speed to maxSpeed
        float horizontalMag = new Vector3(newHorizontal.x, 0f, newHorizontal.z).magnitude;
        if (horizontalMag > maxSpeed)
        {
            newHorizontal = newHorizontal.normalized * maxSpeed;
        }

        // Preserve current vertical velocity
        rb.linearVelocity = new Vector3(newHorizontal.x, current.y, newHorizontal.z);

        // Optional: rotate player to face movement direction
        // if (newHorizontal.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(newHorizontal);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            inputDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            inputDirection = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (collisions >= 1)
        {
            rb.AddForce(new Vector3(0, JumpHight, 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions += 1;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisions -= 1;
    }
}
