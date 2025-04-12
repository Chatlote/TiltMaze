using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject storyPopup;
    public Text storyText;
    public Button continueButton;
    public Text buttonText; 

    [Header("Messages")]
    [TextArea(3, 5)] public string introMessage;
    [TextArea(3, 5)] public string outroMessage;

    [Header("Button Texts")]
    public string introButtonText = "Wake up. Now.";
    public string collectibleButtonText = "Keep going.";
    public string outroButtonText = "Wake up. Again.";

    [Header("Visual Settings")]
    public Color introButtonColor = Color.cyan;
    public Color normalButtonColor = Color.white;
    public Color outroButtonColor = Color.green;

    private void Start()
    {
        continueButton.onClick.AddListener(ClosePopup);

        ShowPopup(introMessage, true);
    }

    public void ShowPopup(string message, bool isIntro = false)
    {
        storyText.text = message;

        if (isIntro)
        {
            buttonText.text = introButtonText;
            buttonText.color = introButtonColor;
        }
        else
        {
            buttonText.text = collectibleButtonText;
            buttonText.color = normalButtonColor;
        }

        storyPopup.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ShowOutroPopup()
    {
        storyText.text = outroMessage;
        buttonText.text = outroButtonText;
        buttonText.color = outroButtonColor;

        storyPopup.SetActive(true);
        Time.timeScale = 0f;

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(RestartGame);
    }

    private void ClosePopup()
    {
        storyPopup.SetActive(false);
        Time.timeScale = 1f; 
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Hover info.
    public void OnButtonHover()
    {
        continueButton.transform.localScale = Vector3.one * 1.1f;
    }

    public void OnButtonExit()
    {
        continueButton.transform.localScale = Vector3.one;
    }
}