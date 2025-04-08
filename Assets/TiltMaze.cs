using UnityEngine;

public class TiltMaze : MonoBehaviour
{
    [Header("Tilt Settings")]
    public float maxTiltAngle = 25f;
    public float tiltSmoothness = 8f;
    public float mouseSensitivity = 0.2f;
    public float returnSpeed = 5f;

    private Vector3 initialRotation;
    private Vector3 targetTilt;
    private Vector2 mouseStartPos;

    void Start()
    {
        initialRotation = transform.eulerAngles;
        SetupColliders();
    }

    void Update()
    {
        HandleTiltInput();
        ApplyTilt();
    }

    void HandleTiltInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mouseDelta = (Vector2)Input.mousePosition - mouseStartPos;

            targetTilt.x = Mathf.Clamp(
                mouseDelta.y * mouseSensitivity,
                -maxTiltAngle,
                maxTiltAngle
            );

            targetTilt.y = Mathf.Clamp(
                -mouseDelta.x * mouseSensitivity,
                -maxTiltAngle,
                maxTiltAngle
            );
        }
        else
        {
            targetTilt = Vector3.Lerp(targetTilt, Vector3.zero, returnSpeed * Time.deltaTime);
        }
    }

    void ApplyTilt()
    {
        Quaternion targetRot = Quaternion.Euler(initialRotation) *
                             Quaternion.Euler(targetTilt.x, 0, targetTilt.y);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            tiltSmoothness * Time.deltaTime
        );
    }


    void SetupColliders()
    {
        if (!GetComponent<Collider>())
        {
            var collider = gameObject.AddComponent<MeshCollider>();
            collider.convex = true; 
        }

        foreach (Transform child in transform)
        {
            if (!child.GetComponent<Collider>() && child.GetComponent<Renderer>())
            {
                var meshCollider = child.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true; 
                meshCollider.sharedMesh = child.GetComponent<MeshFilter>().sharedMesh; 
            }
        }
    }
}
