using UnityEngine;

public interface Collectible
{
    public void OnCollect();

    public void AfterCollect();

    public void OnEffect();

    public void OnSpawn();

    public void OnChange();
}
