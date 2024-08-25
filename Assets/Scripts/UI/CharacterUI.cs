#region

using System;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class CharacterUI : MonoBehaviour
{
    #region Events

    public static event EventHandler OnCharacterUIOpen;
    public static event EventHandler OnCharacterUIClose;

    #endregion

    #region Variables & References

    [SerializeField] private Button closeButton;
    [SerializeField] private PlayerStatsUI playerStatsUI;

    private bool isFirstUpdate;

    private bool isSubscribed;

    #endregion

    #region Initialization & Subscribed events

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        isFirstUpdate = true;

        GameInput.Instance.OnOpenCharacterInfoAction += GameInput_OnOpenCharacterInfoAction;
        isSubscribed = true;

        GameStageManager.Instance.OnGameStageChanged += GameStageManager_OnGameStageChanged;
    }

    private void GameStageManager_OnGameStageChanged(object sender, EventArgs e)
    {
        if (GameStageManager.Instance.IsLost())
        {
            gameObject.SetActive(false);

            GameInput.Instance.OnOpenCharacterInfoAction -= GameInput_OnOpenCharacterInfoAction;
            GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;
            isSubscribed = false;
        }
    }

    private void GameInput_OnOpenCharacterInfoAction(object sender, EventArgs e)
    {
        if (gameObject.activeSelf) return;

        Show();
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Hide();
        }
    }

    #endregion

    #region Tab Visual

    private void Show()
    {
        gameObject.SetActive(true);

        OnCharacterUIOpen?.Invoke(this, EventArgs.Empty);

        playerStatsUI.UpdateVisual();

        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void Hide()
    {
        gameObject.SetActive(false);

        OnCharacterUIClose?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        Hide();

        GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;
    }

    #endregion

    public void OnDestroy()
    {
        GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;

        OnCharacterUIOpen = null;
        OnCharacterUIClose = null;
    }
}