using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "TowerDefense/TowerData")]
public class Tower_SO : ScriptableObject
{
    public enum TowerType
    {
        Arrow,
        Bomb,
        Magic,
    }
    public TowerType Type;
    public int cost;
    public int damage;
    //public int effectValue;
    public float attackRadius;
    public float speed;
    public LayerMask targetLayer;
    public string targetTag;
}
