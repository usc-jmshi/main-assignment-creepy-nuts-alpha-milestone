using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button tutorialButton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Hook up button listeners
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayGame);

        if (tutorialButton != null)
            tutorialButton.onClick.AddListener(OnOpenTutorial);
    }

    private void OnPlayGame()
    {
        // Load scene at index 1 (Main Game)
        SceneManager.LoadScene(1);
    }
    
    private void OnApplicationQuit()
    {
        if (DeathAnalytics.Instance != null)
            DeathAnalytics.Instance.RecordDeath(DeathCause.Quit);
    }

    private void OnOpenTutorial()
    {
        SceneManager.LoadScene(2);
    }
}