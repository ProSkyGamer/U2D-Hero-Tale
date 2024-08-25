#region

using System;
using UnityEngine;

#endregion

public class GameStageManager : MonoBehaviour
{
    private enum GameStages
    {
        Waiting,
        Battle,
        Pause,
        Lost
    }

    public static GameStageManager Instance { get; private set; }

    public event EventHandler OnGameStageChanged;

    [SerializeField] private GameStages startingGameStage;
    private GameStages currentGameStage;
    private GameStages previousGameStage;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        AllEnemiesController.Instance.OnNewEnemySpawned += AllEnemiesController_OnNewEnemySpawned;
        AllEnemiesController.Instance.OnEnemyDefeated += AllEnemiesController_OnEnemyDefeated;
        PlayerController.Instance.OnEntityDead += PlayerController_OnEntityDead;

        LeaveBattleButtonUI.OnBattleLeft += LeaveBattleButtonUI_OnBattleLeft;

        CharacterUI.OnCharacterUIOpen += CharacterUI_OnCharacterUIOpen;
        CharacterUI.OnCharacterUIClose += CharacterUI_OnCharacterUIClose;
    }

    private void CharacterUI_OnCharacterUIOpen(object sender, EventArgs e)
    {
        previousGameStage = currentGameStage;
        ChangeGameStage(GameStages.Pause);
    }

    private void CharacterUI_OnCharacterUIClose(object sender, EventArgs e)
    {
        ChangeGameStage(previousGameStage);
    }

    private void LeaveBattleButtonUI_OnBattleLeft(object sender, EventArgs e)
    {
        ChangeGameStage(GameStages.Waiting);
    }

    private void PlayerController_OnEntityDead(object sender, EventArgs e)
    {
        ChangeGameStage(GameStages.Lost);
    }

    private void AllEnemiesController_OnEnemyDefeated(object sender, EventArgs e)
    {
        ChangeGameStage(GameStages.Waiting);
    }

    private void AllEnemiesController_OnNewEnemySpawned(object sender, EventArgs e)
    {
        ChangeGameStage(GameStages.Battle);
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            ChangeGameStage(startingGameStage);
        }
    }

    private void ChangeGameStage(GameStages newGameStage)
    {
        currentGameStage = newGameStage;

        OnGameStageChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsFighting()
    {
        return currentGameStage == GameStages.Battle;
    }

    public bool IsWaiting()
    {
        return currentGameStage == GameStages.Waiting;
    }

    public bool IsLost()
    {
        return currentGameStage == GameStages.Lost;
    }

    public bool IsPaused()
    {
        return currentGameStage == GameStages.Pause;
    }

    private void OnDestroy()
    {
        AllEnemiesController.Instance.OnNewEnemySpawned -= AllEnemiesController_OnNewEnemySpawned;
        AllEnemiesController.Instance.OnEnemyDefeated -= AllEnemiesController_OnEnemyDefeated;
        PlayerController.Instance.OnEntityDead -= PlayerController_OnEntityDead;

        LeaveBattleButtonUI.OnBattleLeft -= LeaveBattleButtonUI_OnBattleLeft;

        CharacterUI.OnCharacterUIOpen -= CharacterUI_OnCharacterUIOpen;
        CharacterUI.OnCharacterUIClose -= CharacterUI_OnCharacterUIClose;
    }
}