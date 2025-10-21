using UnityEngine;

public class CollectibleEffector : MonoBehaviour
{
    public virtual void OnEffect()
    {

    }

    private void Start()
    {
        Debug.Log("Start");
        if(!CollectibleManager.Instance.effectors.Contains(this))
            CollectibleManager.Instance.effectors.Add(this);
    }

    private void OnDestroy()
    {
        CollectibleManager.Instance.effectors.Remove(this);
    }


}
