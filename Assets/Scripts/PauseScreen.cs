using UnityEngine;
using UnityEngine.UIElements;

public class PauseScreen : MonoBehaviour
{
    private VisualElement pauseContainer;
    private Label pauseTitle;
    private Button resumeButton;
    private bool isPaused = false;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        pauseContainer = root.Q<VisualElement>("pause-container");
        pauseTitle = root.Q<Label>("pause-title");
        resumeButton = root.Q<Button>("resume-button");

        if (pauseTitle != null)
            pauseTitle.text = "Paused";
        if (resumeButton != null)
            resumeButton.text = "Resume";

        if (resumeButton != null)
            resumeButton.clicked += Resume;

        if (pauseContainer != null)
            pauseContainer.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        // Only trigger pause with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    private void Pause()
    {
        if (pauseContainer == null) return;
        isPaused = true;
        pauseContainer.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
        UnityEngine.Cursor.lockState = CursorLockMode.None;  // unlock mouse
        UnityEngine.Cursor.visible = true;
    }

    private void Resume()
    {
        if (pauseContainer == null) return;
        isPaused = false;
        pauseContainer.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;  // re-lock mouse
        UnityEngine.Cursor.visible = false;
    }
}