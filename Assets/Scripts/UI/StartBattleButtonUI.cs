#region

using System;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class StartBattleButtonUI : MonoBehaviour
{
    private Button startBattleButton;

    private void Awake()
    {
        startBattleButton = GetComponent<Button>();

        startBattleButton.onClick.AddListener(() => { AllEnemiesController.Instance.StartBattle(); });
    }

    private void Start()
    {
        GameStageManager.Instance.OnGameStageChanged += GameStageManager_OnGameStageChanged;
    }

    private void GameStageManager_OnGameStageChanged(object sender, EventArgs e)
    {
        if (GameStageManager.Instance.IsWaiting())
            Show();
        else
            Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameStageManager.Instance.OnGameStageChanged -= GameStageManager_OnGameStageChanged;
    }
}