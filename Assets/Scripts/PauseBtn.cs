using UnityEngine;
using UnityEngine.UIElements;

public class PauseBtn : MonoBehaviour
{
    private Button pauseButton;
    private PauseScreen pauseUI; // updated reference

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        pauseButton = root.Q<Button>("pause-button");
        pauseUI = FindObjectOfType<PauseScreen>(); // updated here too

        if (pauseButton != null)
        {
            pauseButton.text = "⏸️";
            pauseButton.clicked += OnPauseClicked;
        }
    }

    private void OnPauseClicked()
    {
        if (pauseUI == null) return;

        var pauseMethod = pauseUI.GetType().GetMethod("Pause", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        pauseMethod.Invoke(pauseUI, null);
    }
}