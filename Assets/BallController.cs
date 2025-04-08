using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("References")]
    public Transform maze;  // Reference to the maze transform
    
    [Header("Ball Physics Settings")]
    [Tooltip("How strongly the ball responds to maze tilt")]
    public float gravity = 9.8f;
    [Tooltip("How much the ball slows down over time")]
    public float friction = 0.95f;
    [Tooltip("Maximum velocity to prevent excessive speed")]
    public float maxVelocity = 10f;
    
    [Header("Debug")]
    public bool showDebugInfo = false;
    
    // Internal variables
    private Vector3 velocity;
    private Vector3 acceleration;
    
    void Start()
    {
        if (maze == null)
        {
            Debug.LogError("Maze reference not set. Please assign the maze GameObject in the inspector.");
            enabled = false;
        }
        
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }
    
    void Update()
    {
        // Get the up direction of the maze (the normal)
        Vector3 mazeUp = maze.up;
        
        // Calculate the tilt direction (projection of world down onto maze plane)
        Vector3 worldDown = Vector3.down;
        Vector3 tiltDirection = worldDown - Vector3.Dot(worldDown, mazeUp) * mazeUp;
        tiltDirection.Normalize();
        
        // Calculate tilt angle - how much is the maze tilted from horizontal
        float tiltAngle = Vector3.Angle(mazeUp, Vector3.up);
        
        // More tilt = more acceleration
        float accelerationMagnitude = gravity * Mathf.Sin(tiltAngle * Mathf.Deg2Rad);
        
        // Apply acceleration in the tilt direction
        acceleration = tiltDirection * accelerationMagnitude;
        
        // Update velocity
        velocity += acceleration * Time.deltaTime;
        
        // Apply friction
        velocity *= friction;
        
        // Clamp velocity
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }
        
        // Move the ball
        transform.position += velocity * Time.deltaTime;
        
        // Optional debug visualization
        if (showDebugInfo)
        {
            Debug.DrawRay(transform.position, tiltDirection, Color.red);
            Debug.DrawRay(transform.position, velocity, Color.green);
        }
        
        // Keep the ball constrained to the maze surface (optional)
        ConstrainBallToMaze();
    }
    
    private void ConstrainBallToMaze()
    {
        // This is a simplified approach - you may need a more robust method
        // depending on your maze geometry
        
        // 1. Cast a ray down from the ball to the maze
        RaycastHit hit;
        float rayDistance = 10f;  // Adjust based on your maze scale
        
        if (Physics.Raycast(transform.position + Vector3.up * 5f, Vector3.down, out hit, rayDistance))
        {
            // Check if we hit the maze or a part of it
            if (hit.transform == maze || hit.transform.IsChildOf(maze))
            {
                // Set ball height to be just above the maze surface
                float ballRadius = GetComponent<SphereCollider>() ? 
                    GetComponent<SphereCollider>().radius * transform.localScale.y : 0.5f;
                
                Vector3 newPosition = transform.position;
                newPosition.y = hit.point.y + ballRadius;
                transform.position = newPosition;
            }
        }
    }
}