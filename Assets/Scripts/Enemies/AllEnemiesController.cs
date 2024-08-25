#region

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#endregion

public class AllEnemiesController : MonoBehaviour
{
    public event EventHandler<OnNewLootAppearedEventArgs> OnNewLootAppeared;

    public class OnNewLootAppearedEventArgs : EventArgs
    {
        public List<InventoryObject> enemyLoot;
    }

    public event EventHandler OnNewEnemySpawned;
    public event EventHandler OnEnemyDefeated;

    public static AllEnemiesController Instance { get; private set; }

    [SerializeField] private AllEnemiesSO allEnemies;

    private EnemySO currentFightingEnemySO;
    private AllEntitiesController.EntityComponents currentFightingEnemyComponents;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        GameStageManager.Instance.OnGameStageChanged += GameStageManager_OnGameStageChanged;
    }

    private void GameStageManager_OnGameStageChanged(object sender, EventArgs e)
    {
        if (GameStageManager.Instance.IsWaiting())
            KillEnemies();
    }

    public void StartBattle()
    {
        var allEnemiesSpawnChance = 0;

        foreach (var enemy in allEnemies.allEnemies)
        {
            allEnemiesSpawnChance += (int)(enemy.enemySpawnChance * 100);
        }

        var randomEnemyIndex = Random.Range(0, allEnemiesSpawnChance);

        Debug.Log($"{allEnemiesSpawnChance} {randomEnemyIndex}");

        foreach (var enemy in allEnemies.allEnemies)
        {
            randomEnemyIndex -= (int)(enemy.enemySpawnChance * 100);

            if (randomEnemyIndex > 0) continue;

            Debug.Log($"{randomEnemyIndex} {(int)(enemy.enemySpawnChance * 100)}");

            currentFightingEnemySO = enemy.enemySO;
            break;
        }

        Debug.Log($"{currentFightingEnemySO.name}");

        var newFightingEnemy = Instantiate(currentFightingEnemySO.enemyPrefab);
        var newFightingEnemyController = newFightingEnemy.GetComponent<EnemyController>();
        var newFightingEnemyEntityHealthController = newFightingEnemy.GetComponent<EntityHealthController>();
        newFightingEnemyController.InitializeEntitySO(currentFightingEnemySO);
        newFightingEnemyController.OnEntityDead += NewFightingEnemyController_OnEntityDead;

        currentFightingEnemyComponents = new AllEntitiesController.EntityComponents
        {
            entityTransform = newFightingEnemy,
            entityController = newFightingEnemyController,
            entityHealthController = newFightingEnemyEntityHealthController
        };

        OnNewEnemySpawned?.Invoke(this, EventArgs.Empty);
    }

    private void NewFightingEnemyController_OnEntityDead(object sender, EventArgs e)
    {
        var droppedLoot = new List<InventoryObject>();

        foreach (var enemyLootSingle in currentFightingEnemySO.enemyLoot)
        {
            var maxChance = 100;
            var isDropThisLoot = Random.Range(0, maxChance) > maxChance - (int)(enemyLootSingle.lootDropChance * 100);

            if (!isDropThisLoot) continue;

            droppedLoot.Add(enemyLootSingle.lootInventoryObjectPrefab);
        }

        if (droppedLoot.Count > 0)
            OnNewLootAppeared?.Invoke(this, new OnNewLootAppearedEventArgs
            {
                enemyLoot = droppedLoot
            });

        OnEnemyDefeated?.Invoke(this, EventArgs.Empty);
    }

    private void KillEnemies()
    {
        if (currentFightingEnemyComponents == null) return;

        Destroy(currentFightingEnemyComponents.entityTransform.gameObject);
        currentFightingEnemyComponents = null;
        currentFightingEnemySO = null;
    }

    public AllEntitiesController.EntityComponents GetCurrentFightingEnemyComponents()
    {
        return currentFightingEnemyComponents;
    }

    private void OnDestroy()
    {
        GameStageManager.Instance.OnGameStageChanged -= GameStageManager_OnGameStageChanged;
    }
}