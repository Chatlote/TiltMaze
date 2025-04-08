using UnityEngine;
using UnityEngine.UI;

public class SphereCollectible : MonoBehaviour
{
    public GameObject collectTextPrefab; 
    public float displayTime = 2f; 
    public static int spheresCollected = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spheresCollected++;

            if (collectTextPrefab != null)
            {
                GameObject textPopup = Instantiate(collectTextPrefab, FindObjectOfType<Canvas>().transform);
                Destroy(textPopup, displayTime);
            }

            gameObject.SetActive(false); 

            if (spheresCollected >= 3)
            {
                MazeExit.OpenExit();
            }
        }
    }
}