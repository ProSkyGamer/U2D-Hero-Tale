#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

[CreateAssetMenu]
public class EnemySO : EntitySO
{
    [Serializable]
    public class Loot
    {
        [Range(0, 1f)] public float lootDropChance = .5f;
        public InventoryObject lootInventoryObjectPrefab;
    }

    public List<Loot> enemyLoot;
    public Transform enemyPrefab;
}