using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltMazeV2 : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed at which the maze rotates
    public float maxRotationAngle = 45f; // Maximum rotation angle (in degrees)

    private Vector3 previousMousePosition;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Assuming there's a Rigidbody attached to the maze
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        // Get mouse delta
        Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
        
        // Calculate rotation based on the mouse delta
        float rotationAmount = mouseDelta.x * rotationSpeed * Time.deltaTime;

        // Get the current rotation angle and clamp it
        float currentRotation = transform.eulerAngles.z;
        float targetRotation = Mathf.Clamp(currentRotation + rotationAmount, -maxRotationAngle, maxRotationAngle );

        // Apply torque (angular velocity)
        rb.angularVelocity = new Vector3(0, 0, rotationAmount);

        // Update the previous mouse position for the next frame
        previousMousePosition = Input.mousePosition;
    }
}