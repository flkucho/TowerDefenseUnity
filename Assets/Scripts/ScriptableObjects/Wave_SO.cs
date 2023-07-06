using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "TowerDefense/WaveData")]
public class Wave_SO : ScriptableObject
{
    public float timeBetweenEnemies;
    public List<Wave> enemies;
}

[Serializable]
public class Wave
{
    public float speedModifier;
    public int count;
    public Enemy_SO.EnemyType type;

    public Wave(int count, Enemy_SO.EnemyType type, float spdMod = 1)
    {
        this.count = count;
        this.type = type;
        this.speedModifier = spdMod;
    }
}
