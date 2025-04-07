using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Tilt Settings")]
    public float maxTiltAngle = 25f;
    public float tiltSmoothness = 8f;
    public float mouseSensitivity = 0.2f;
    public float returnSpeed = 5f;

    private Vector3 targetTilt;
    private Vector2 mouseStartPos;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate; 
            rb.sleepThreshold = 0f; 
        }
    }

    void FixedUpdate()
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
                mouseDelta.x * mouseSensitivity,
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
        Quaternion targetRot = Quaternion.Euler(targetTilt.x, targetTilt.y, 0);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            tiltSmoothness * Time.deltaTime
        );
    }
}
