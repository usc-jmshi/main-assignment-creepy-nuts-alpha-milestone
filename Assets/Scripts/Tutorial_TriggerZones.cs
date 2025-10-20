using UnityEngine;

public class Tutorial_TriggerZones : MonoBehaviour
{
    private float currentZPos;
    public GameObject spawnPoint;



    private void OnTriggerEnter(Collider other)
    {
        currentZPos = transform.position.z;
        spawnPoint.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, currentZPos);

    }
    
    
    
}
