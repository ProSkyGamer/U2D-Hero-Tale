#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

[CreateAssetMenu]
public class AllEnemiesSO : ScriptableObject
{
    [Serializable]
    public class Enemy
    {
        public EnemySO enemySO;
        [Range(0, 1f)] public float enemySpawnChance;
    }

    public List<Enemy> allEnemies;
}