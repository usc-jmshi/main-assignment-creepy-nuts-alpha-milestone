using UnityEngine;
using UnityEngine.UIElements;

public class LegendUI : MonoBehaviour
{
    private VisualElement legendContainer;
    private bool isVisible = true;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        legendContainer = root.Q<VisualElement>("legend-container");
        ShowLegend(); // Display when game starts
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isVisible = !isVisible;
            legendContainer.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public void ShowLegend()
    {
        isVisible = true;
        if (legendContainer != null)
            legendContainer.style.display = DisplayStyle.Flex;
    }

    public void HideLegend()
    {
        isVisible = false;
        if (legendContainer != null)
            legendContainer.style.display = DisplayStyle.None;
    }
}