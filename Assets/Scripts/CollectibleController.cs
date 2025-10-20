using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collectible>() != null)
        {
            other.GetComponent<Collectible>().OnCollect();
        }
    }
}
