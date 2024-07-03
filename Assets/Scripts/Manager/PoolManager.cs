using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
        }
        else
        {
            Destroy(Instance);
        }
    }

    public void CreatePool(string tag, GameObject prefab, int poolSize)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(tag, objectPool);
        }
    }

    public GameObject GetFromPool(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            GameObject objectToReuse = poolDictionary[tag].Dequeue();
            objectToReuse.SetActive(true);
            poolDictionary[tag].Enqueue(objectToReuse);
            return objectToReuse;
        }

        Debug.LogWarning("Pool with tag " + tag + "doesn't exist");
        return null;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if(poolDictionary.ContainsKey(tag))
        {
            objectToReturn.SetActive(false);
            poolDictionary[tag].Enqueue(objectToReturn);
        }
        else
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
        }
    }
}
