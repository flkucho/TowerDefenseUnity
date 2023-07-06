using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class ObjectPool : MonoBehaviour
{

    PooledObject prefab;
    [SerializeField] int reachedPoolSize;

    public int ReachedPoolSize
    {
        get { return reachedPoolSize; }
    }

    public List<PooledObject> availableObjects = new List<PooledObject>();

    public PooledObject GetObject()
    {
        PooledObject obj;
        int lastAvailableIndex = availableObjects.Count - 1;
        if (lastAvailableIndex >= 0)
        {
            obj = ReturnExistingObject(lastAvailableIndex);
        }
        else
        {
            obj = ReturnNewObject();
        }
        obj.inUse = true;
        return obj;
    }

    PooledObject ReturnNewObject()
    {
        PooledObject obj = Instantiate<PooledObject>(prefab);
        obj.transform.SetParent(transform, false);
        obj.Pool = this;
        obj.go = obj.gameObject;
        reachedPoolSize++;
        return obj;
    }

    PooledObject ReturnExistingObject(int lastAvailableIndex)
    {
        PooledObject obj = availableObjects[lastAvailableIndex];
        availableObjects.RemoveAt(lastAvailableIndex);
        obj.go.SetActive(true);
        return obj;
    }

    public static ObjectPool GetPool(PooledObject prefab)
    {
        GameObject obj;
        ObjectPool pool;
        if (Application.isEditor)
        {
            obj = GameObject.Find(prefab.name + " Pool");
            if (obj)
            {
                pool = obj.GetComponent<ObjectPool>();
                if (pool)
                {
                    return pool;
                }
            }
        }
        obj = new GameObject(prefab.name + " Pool");
        var poolsObj = GameObject.Find("Pools");
        if (!poolsObj)
        {
            poolsObj = new GameObject();
            poolsObj.name = "Pools";
        }
        obj.transform.parent = poolsObj.transform;
        pool = obj.AddComponent<ObjectPool>();
        pool.prefab = prefab;
        return pool;
    }

    public void AddObject(PooledObject obj)
    {
        obj.inUse = false;
        obj.go.SetActive(false);
        availableObjects.Add(obj);
    }

    public void ClearPool()
    {
        int count = availableObjects.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(availableObjects[i].go);
        }
        availableObjects.Clear();
    }

    public void ReturnChildrenToPool()
    {
        PooledObject[] objs = GetComponentsInChildren<PooledObject>();
        int count = objs.Length;
        for (int i = 0; i < count; i++)
        {
            if (objs[i].inUse)
            {
                objs[i].ReturnToPool();
            }
        }
    }


}