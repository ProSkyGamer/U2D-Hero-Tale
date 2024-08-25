#region

using System;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class LeaveBattleButtonUI : MonoBehaviour
{
    public static event EventHandler OnBattleLeft;

    private Button leaveBattleButtonUI;

    private void Awake()
    {
        leaveBattleButtonUI = GetComponent<Button>();

        leaveBattleButtonUI.onClick.AddListener(() => { OnBattleLeft?.Invoke(this, EventArgs.Empty); });
    }

    private void Start()
    {
        GameStageManager.Instance.OnGameStageChanged += GameStageManager_OnGameStageChanged;

        Hide();
    }

    private void GameStageManager_OnGameStageChanged(object sender, EventArgs e)
    {
        if (GameStageManager.Instance.IsFighting())
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