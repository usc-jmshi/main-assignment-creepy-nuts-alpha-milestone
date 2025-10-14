using UnityEngine;

public class CloneBehavior : MonoBehaviour
{
    private float timetoDisappear = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeToDelete();
        destroyClone();
    }

    public void timeToDelete()
    {
        timetoDisappear -= Time.deltaTime;
    }

    public void destroyClone()
    {
        if (timetoDisappear <= 0f){
            Destroy(gameObject);
        }
    }
}
