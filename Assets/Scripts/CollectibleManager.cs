using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }
    public List<CollectibleEffector> effectors;
    public List<GameObject> cObjects;

    public float maxHP = 100;
    public float health;

    void Init()
    {
        health = maxHP;
    }

    private void Awake()
    {
        Instance = this;
        Init();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var effector in effectors)
        {
            effector.OnEffect();
        }
    }

    public void CreateCollectible(int index, Vector3 position, Quaternion rotation)
    {
        if(index < cObjects.Count)
        {
            Instantiate(cObjects[index], position, rotation);
        }
            
    }

    public void CreateCollectible(int index, Vector3 position)
    {
        CreateCollectible(index, position, Quaternion.identity);
    }
}
