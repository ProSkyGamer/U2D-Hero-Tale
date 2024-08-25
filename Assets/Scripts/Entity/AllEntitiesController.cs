#region

using System;
using UnityEngine;

#endregion

public class AllEntitiesController : MonoBehaviour
{
    public class EntityComponents
    {
        public Transform entityTransform;
        public EntityController entityController;
        public EntityHealthController entityHealthController;
    }

    public static AllEntitiesController Instance { get; private set; }

    private EntityComponents currentPlayerEntityComponents;
    private EntityComponents currentEnemyEntityComponents;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        var newPlayerEntityTransform = PlayerController.Instance.transform;
        var newPlayerEntityController = PlayerController.Instance;
        var newPlayerEntityHealthController = PlayerController.Instance.gameObject.GetComponent<EntityHealthController>();

        var newCurrentPlayerEntityComponents = new EntityComponents
        {
            entityTransform = newPlayerEntityTransform,
            entityController = newPlayerEntityController,
            entityHealthController = newPlayerEntityHealthController
        };

        currentPlayerEntityComponents = newCurrentPlayerEntityComponents;

        AllEnemiesController.Instance.OnNewEnemySpawned += AllEnemiesController_OnNewEnemySpawned;
    }

    private void AllEnemiesController_OnNewEnemySpawned(object sender, EventArgs e)
    {
        currentEnemyEntityComponents = AllEnemiesController.Instance.GetCurrentFightingEnemyComponents();
    }

    public EntityComponents GetPlayerEntityComponents()
    {
        return currentPlayerEntityComponents;
    }

    public EntityComponents GetEnemyEntityComponents()
    {
        return currentEnemyEntityComponents;
    }

    private void OnDestroy()
    {
        AllEnemiesController.Instance.OnNewEnemySpawned -= AllEnemiesController_OnNewEnemySpawned;
    }
}