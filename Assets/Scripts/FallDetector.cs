using UnityEngine;

public class FallDetector : MonoBehaviour
{
    [SerializeField] private float fallThreshold = -10f;
    
    private void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            GameManager.Instance.Die(DeathCause.FellOffPlatform);
        }
    }
}