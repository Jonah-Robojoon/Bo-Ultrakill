using Unity.VisualScripting;
using UnityEngine;

public class DampTesting : MonoBehaviour
{
    private Camera _cam;
    private Vector3 _startLocalPos;

    [Header("Settings")]
    public float parallaxStrength = 0.5f;
    public float smooth = 5f;
    public Vector3 parallaxClamp = new Vector3(0.3f, 0.3f, 0.3f);

    void Start()
    {
        _cam = Camera.main;
        _startLocalPos = transform.localPosition;
    }

    void Update()
    {
        
        float xOffset = _cam.transform.rotation.eulerAngles.x;
        float yOffset = _cam.transform.rotation.eulerAngles.y;

        
        Vector3 parallaxOffset = new Vector3(yOffset, xOffset, 0f) * parallaxStrength / 100f;

        
        parallaxOffset = Vector3.Min(parallaxOffset, parallaxClamp);
        parallaxOffset = Vector3.Max(parallaxOffset, -parallaxClamp);

        
        Vector3 targetPos = _startLocalPos + parallaxOffset;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smooth);
    }
}
