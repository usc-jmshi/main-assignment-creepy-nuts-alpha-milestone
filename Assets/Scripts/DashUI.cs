using UnityEngine;
using UnityEngine.UIElements;

public class DashUI : MonoBehaviour
{
    private VisualElement dashBarFill;
    private Label dashText;

    [SerializeField] private PlayerController player;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        dashBarFill = root.Q<VisualElement>("dash-bar-fill");
        dashText = root.Q<Label>("dash-text");
    }

    private void Update()
    {
        if (player == null) return;

        // Update lightning icons
        dashText.text = GetDashIcons(player.DashesLeft);

        // Update bar fill width
        dashBarFill.style.width = Length.Percent(player.DashRefillPercentage * 100f);
    }

    private string GetDashIcons(int count)
    {
        // You can use ⚡, ●, or any symbol you prefer
        return new string('+', Mathf.Clamp(count, 0, 3));
    }
}