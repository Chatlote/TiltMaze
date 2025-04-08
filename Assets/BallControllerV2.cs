using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerV2 : MonoBehaviour
{
    public TiltDirectionCalculator TiltDirectionChecker;

    public float MaxFallingForce = 1.0f;
    
    private Rigidbody myRigidbody = null;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        myRigidbody.AddForce(TiltDirectionChecker.GetFallDirection() * TiltDirectionChecker.GetNormalizedTilt() * MaxFallingForce);
        
        // Testing purposes can be removed
        //transform.position += TiltDirectionChecker.GetFallDirection() * Time.deltaTime;
    }
}
