using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PooledObject
{
    public delegate void EnemyEventHanler(Enemy enemy);
    public static event EnemyEventHanler OnEnemyReachedFinalPoint;
    public static event EnemyEventHanler OnEnemyDied;

    [SerializeField]
    Enemy_SO enemyData;

    public int health;
    public float speed = 1;
    float speedModifier;


    List<Vector3> path = new List<Vector3>();
    private int currentPointIndex = 0;

    internal int Reward { get => enemyData.reward; }

    public bool IsAlive()
    {
        return health > 0 && gameObject.activeInHierarchy;
    }
    // Add variation to the enemy path to make it more apealing 
    private void AddPathVariation()
    {
        for (int i = 0; i < path.Count; i++)
        {
            var point = Random.insideUnitSphere * .25f;
            path[i] += new Vector3(point.x, 0, point.y);
        }
    }

    // Reduce enemy health by the damage amount
    public void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnEnemyDied?.Invoke(this);
        }
    }

    public void SetPath(Path path)
    {
        foreach (var item in path.points)
        {
            this.path.Add(item);
        }
        AddPathVariation();
        health = enemyData.Health;
        speed = enemyData.Speed;
    }
    public void Move()
    {
        if (path == null || path.Count == 0)
            return;

        Vector3 targetPoint = path[currentPointIndex];
        Vector3 moveDirection = (targetPoint - transform.position).normalized;
        transform.position += moveDirection * (speed * speedModifier) * Time.deltaTime;

        float distanceToTarget = Vector3.Distance(transform.position, targetPoint);
        if (distanceToTarget < 0.05f)
        {
            currentPointIndex++;
            if (currentPointIndex >= path.Count)
            {
                currentPointIndex = path.Count - 1;
                OnEnemyReachedFinalPoint?.Invoke(this);
            }

        }
    }
    public override void ReturnToPool()
    {
        path.Clear();
        currentPointIndex = 0;
        base.ReturnToPool();
    }

    public void SetSpeedModifier(float speedModifier)
    {
        this.speedModifier = speedModifier;
    }
}
