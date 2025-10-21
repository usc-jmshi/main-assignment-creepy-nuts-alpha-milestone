using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [SerializeField]
    private DeathWall _deathWall;

    [SerializeField]
    private LegendUI legendUI;

    private void Start()
    {
        if (legendUI != null)
        {
            legendUI.ShowLegend(); // Display the legend when game starts
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController _))
        {
            _deathWall.Moving = true;
            if (legendUI != null)
            {
                legendUI.HideLegend(); // Hide the legend when leaving the start zone
            }
        }
    }
}
