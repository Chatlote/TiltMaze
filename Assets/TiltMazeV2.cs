using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltMazeV2 : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float maxRotationAngle = 45f; 

    private Vector3 previousMousePosition;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
        
        float rotationAmount = mouseDelta.x * rotationSpeed * Time.deltaTime;

        float currentRotation = transform.eulerAngles.z;
        float targetRotation = Mathf.Clamp(currentRotation + rotationAmount, -maxRotationAngle, maxRotationAngle );

        rb.angularVelocity = new Vector3(0, 0, rotationAmount);

        previousMousePosition = Input.mousePosition;
    }
}