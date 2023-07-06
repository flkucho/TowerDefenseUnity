using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "TowerDefense/EnemyData")]
public class Enemy_SO : ScriptableObject
{
    public enum EnemyType
    {
        Normal,
        Air,
        Tank
    }
    public EnemyType enemyType; 
    public int Health;
    public int Speed;
    public int reward;
}
