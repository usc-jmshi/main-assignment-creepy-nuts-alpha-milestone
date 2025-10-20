using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [SerializeField]
    private DeathWall _deathWall;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController _))
        {
            _deathWall.Moving = true;
        }
    }
}
