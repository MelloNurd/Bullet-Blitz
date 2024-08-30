using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolScript : MonoBehaviour
{
    public static BulletPoolScript SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int startAmount;
    int count;

    void Awake()
    {
        SharedInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < startAmount; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.name = "Bullet" + count++;
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public GameObject CreateNewObject()
    {
        GameObject tmp = Instantiate(objectToPool);
        tmp.name = "Bullet" + count++;
        tmp.SetActive(false);
        pooledObjects.Add(tmp);

        return tmp;
    }

    public void ClearAllObjects()
    {
        for(int i = 0; i < count; i++)
        {
            pooledObjects[i].SetActive(false);
        }
    }
}
