using UnityEngine;

public class FogRechargeCollector : MonoBehaviour, Collectible
{
    public float rechargeAmount = 2;
    public void AfterCollect()
    {
    }

    public void OnChange()
    {
    }

    public void OnCollect()
    {
        CollectibleManager.Instance.health += rechargeAmount;
    }

    public void OnEffect()
    {
    }

    public void OnSpawn()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
