using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// New version of Ball Controller 
public class BallController : MonoBehaviour
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
    }
}



