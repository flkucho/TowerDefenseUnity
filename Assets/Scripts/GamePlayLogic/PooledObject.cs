using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [HideInInspector] public ObjectPool Pool;
    [HideInInspector] public bool inUse = true;
    [HideInInspector] public GameObject go;

    static Dictionary<PooledObject, ObjectPool> poolTable = new Dictionary<PooledObject, ObjectPool>();

    [System.NonSerialized] ObjectPool poolInstanceForPrefab;
    bool objectHasPool = false;

    public virtual void ReturnToPool()
    {
        if (Pool)
        {
            Pool.AddObject(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public T GetPooledInstance<T>() where T : PooledObject
    {
        if (!poolInstanceForPrefab)
        {
            if (!TableHasPoolForObject(this))
            {
                poolInstanceForPrefab = ObjectPool.GetPool(this);
                AddPooledObjectToTable(this, poolInstanceForPrefab);
            }
            else
            {
                poolInstanceForPrefab = poolTable[this];
            }
        }
        return (T)poolInstanceForPrefab.GetObject();
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    static bool TableHasPoolForObject(PooledObject prefab)
    {
        return poolTable.ContainsKey(prefab);
    }

    static void AddPooledObjectToTable(PooledObject prefab, ObjectPool pool)
    {
        poolTable.Add(prefab, pool);
    }
    private void OnDestroy()
    {
        poolTable.Clear();
    }
}