using UnityEngine;

public class TiltDirectionCalculator : MonoBehaviour

    //Teacher notes on this file.
{
    [Header("Tilt Settings")]
    //[Tooltip("Maximum angle in degrees before object is considered fully tilted")]
    //[Range(0, 90)]
    // public
    private  float maxTiltAngle = 45.0f;
    
    [Tooltip("Which direction is considered 'up' for the object in local space")]
    public Vector3 localUpDirection = Vector3.up;
    
    [Header("Debug")]
    public bool showDebugInfo = false;
    public bool drawDebugArrows = false;
    
    private Vector3 fallDirection;
    private float normalizedTiltValue;

    private void Start()
    {
        maxTiltAngle = GetComponent<TiltMaze>().maxTiltAngle;
    }

    void Update()
    {
        CalculateTilt();
        
        if (showDebugInfo)
        {
            DisplayDebugInfo();
        }
    }
    
    // Calculate the tilt direction and normalized tilt value
    void CalculateTilt()
    {
        // Convert local up direction to world space
        Vector3 worldUp = transform.TransformDirection(localUpDirection).normalized;
        
        // Get the global up direction
        Vector3 globalUp = Vector3.up;
        
        // Calculate the angle between our object's up and global up
        float angle = Vector3.Angle(worldUp, globalUp);
        
        // Calculate the fall direction (the direction the object is tilting)
        // Project global up onto the plane defined by our object's up, then negate it
        fallDirection = Vector3.ProjectOnPlane(globalUp, worldUp).normalized;
        
        // If the object is as good as perfectly level, default to no direction
        if (fallDirection.magnitude < 0.001f)
        {
            fallDirection = Vector3.zero;
        }
        else
        {
            fallDirection = -fallDirection; // Negate to get the direction to fall toward
        }
        
        // Calculate the normalized tilt value (0 = not tilted, 1 = tilted to max angle or beyond)
        // We can use this later to apply as much force as we thing makes sense
        normalizedTiltValue = Mathf.Clamp01(angle / maxTiltAngle);
    }
    
    /// Draw debug information to visualize the tilt and fall direction
    void DisplayDebugInfo()
    {
        if (drawDebugArrows)
        {
            // Draw arrow for current up direction
            Debug.DrawRay(transform.position, transform.TransformDirection(localUpDirection).normalized * 2f, Color.green);
            
            // Draw arrow for fall direction (scaled by tilt amount)
            if (fallDirection.magnitude > 0.001f)
            {
                Debug.DrawRay(transform.position, fallDirection * normalizedTiltValue * 2f, Color.red);
            }
            
            // Draw global up for reference
            Debug.DrawRay(transform.position, Vector3.up * 2f, Color.blue);
        }
        
        if (showDebugInfo)
        {
            string debugText = $"Tilt: {normalizedTiltValue:F2} | Direction: {fallDirection}";
            Debug.Log(debugText);
        }
    }
    
    /// Get the current direction the object would fall toward
    public Vector3 GetFallDirection()
    {
        return fallDirection;
    }
    
    public float GetNormalizedTilt()
    {
        return normalizedTiltValue;
    }
}