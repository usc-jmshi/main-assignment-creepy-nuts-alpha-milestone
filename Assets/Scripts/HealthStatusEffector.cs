using UnityEngine;
using UnityEngine.UI;

public class HealthStatusEffector : CollectibleEffector
{
    public Slider s;
    public override void OnEffect()
    {
        float percent = CollectibleManager.Instance.health / CollectibleManager.Instance.maxHP;
        s.value = 1-percent;
    }
}
