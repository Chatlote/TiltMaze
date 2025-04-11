using UnityEngine;
//Note to self: Have the maze wall disappear, wanna make it a proper door on a later version, possibly add animation
public class MazeExit : MonoBehaviour
{
    public GameObject exitBarrier; 

    public static void OpenExit()
    {
        MazeExit instance = FindObjectOfType<MazeExit>();
        if (instance != null && instance.exitBarrier != null)
        {
            instance.exitBarrier.SetActive(false);
            Debug.Log("Maze exit opened!");
        }
    }
}