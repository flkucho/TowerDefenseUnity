using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveController : MonoBehaviour
{
    public delegate void WaveEventhandler(int KilledEnemies);
    public static event WaveEventhandler OnWaveEnded;
    [SerializeField]
    private GameObject spawnPosition, deadZone;
    [SerializeField]
    private Path path;

    public List<Enemy> monsterPrefabs, enemies;
    private List<Wave> currentEnemies;

    private bool isSpawningEnemies;
    private float timeBetweenEnemies;
    private float elapsedT = 0;

    private int earnedMoney = 0;
    private int currentEnemyIndex;
    private int currentEnemyCount;
    void Start()
    {
        Enemy.OnEnemyDied += Enemy_OnEnemyDied;
        Enemy.OnEnemyReachedFinalPoint += Enemy_OnEnemyReachedFinalPoint;
    }
    // Handle when an enemy reaches the final point (base)
    private void Enemy_OnEnemyReachedFinalPoint(Enemy enemy)
    {
        if (GameManager.Instance.PlayerHP > 0)
            GameManager.Instance.DamagePlayer();
        enemies.Remove(enemy);
        enemy.ReturnToPool();
    }
    // Handle when an enemy dies
    private void Enemy_OnEnemyDied(Enemy enemy)
    {
        earnedMoney += enemy.Reward;
        enemies.Remove(enemy);
        enemy.ReturnToPool();
    }


    // Update is called once per frame
    void Update()
    {
        //Move enemies along their path
        foreach (var item in enemies)
        {
            item.Move();
        }
        //Spawn enemies
        if (isSpawningEnemies)
        {
            elapsedT += Time.deltaTime;
            if (elapsedT >= timeBetweenEnemies)
            {
                elapsedT = 0;
                SpawnEnemy();
            }
        }
    }
    // Start a wave with the specified wave data
    public void StartWave(Wave_SO waveData)
    {
        earnedMoney = 0;
        currentEnemyIndex = 0;
        currentEnemies = waveData.enemies;
        timeBetweenEnemies = waveData.timeBetweenEnemies;
        isSpawningEnemies = true;
    }
    // Spawn the next enemy in the wave
    private void SpawnEnemy()
    {

        if (currentEnemyIndex >= currentEnemies.Count)
        {
            // No more enemies to spawn, send notification event to pay bonus money to player for the enemies killed
            isSpawningEnemies = false;
            OnWaveEnded?.Invoke(earnedMoney);
            return;
        }

        Enemy_SO.EnemyType enemyType = currentEnemies[currentEnemyIndex].type;

        if (currentEnemyCount < currentEnemies[currentEnemyIndex].count)
        {
            Enemy newEnemy = monsterPrefabs[(int)enemyType].GetPooledInstance<Enemy>();
            newEnemy.SetPath(path);
            newEnemy.SetPosition(spawnPosition.transform.position);
            newEnemy.SetSpeedModifier(currentEnemies[currentEnemyIndex].speedModifier);
            enemies.Add(newEnemy);
            currentEnemyCount++;
        }
        else
        {
            // Finished spawning the current enemy type, move to the next enemy type
            currentEnemyCount = 0;
            currentEnemyIndex++;
        }
    }
}

