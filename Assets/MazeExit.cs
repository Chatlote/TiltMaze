using UnityEngine;

public class MazeExit : MonoBehaviour
{
    public GameObject exitBarrier;

  
    public static void OpenExit()
    {
        MazeExit exitInstance = FindObjectOfType<MazeExit>();
        if (exitInstance != null && exitInstance.exitBarrier != null)
        {
            exitInstance.exitBarrier.SetActive(false); 
            Debug.Log("Exit opened!");
        }
    }
}