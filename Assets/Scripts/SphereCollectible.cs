using UnityEngine;

public class SphereCollectible : MonoBehaviour
{
    [Header("Effects")]
    public ParticleSystem collectEffect; 
    public float destroyDelay = 1f; 

    [Header("Story")]
    [TextArea(3, 5)] public string storyText;

    public static int spheresCollected = 0;
    public PopupManager popupManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectEffect != null)
            {
                //note to self: improve particles effects at later date
                ParticleSystem particles = Instantiate(
                    collectEffect,
                    transform.position,
                    Quaternion.identity
                );
                particles.Play();
                Destroy(particles.gameObject, particles.main.duration);
            }

            if (popupManager != null)
                popupManager.ShowPopup(storyText);

            spheresCollected++;

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            Destroy(gameObject, destroyDelay);

            if (spheresCollected >= 3)
                MazeExit.OpenExit();
        }
    }
}