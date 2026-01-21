using Unity.VisualScripting;
using UnityEditor.Recorder.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 2f;
    float cameraVerticalRotation = 0f;

    private float controlerInputX;
    private float controlerInputY;


    void Start()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }


    void Update()
    {
        
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        inputX += controlerInputX;
        inputY += controlerInputY;

        

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;


        player.Rotate(Vector3.up * inputX);
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }

    public void CameraInput(InputAction.CallbackContext context)
    {

            Vector2 read = context.ReadValue<Vector2>();
            controlerInputX = read.x * 2;
            controlerInputY = read.y;
           

    }
}
