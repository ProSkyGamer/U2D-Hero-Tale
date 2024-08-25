#region

using System;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class AttackingPhaseTimerUI : MonoBehaviour
{
    [SerializeField] private Image attackingCircleTimerImage;
    [SerializeField] private Image preparingCircleTimerImage;
    [SerializeField] private Image currentPhaseImage;
    [SerializeField] private Sprite attackingPhaseSprite;
    [SerializeField] private Sprite preparingPhaseSprite;
    [SerializeField] private Sprite changingWeaponPhaseSprite;
    [SerializeField] private EntityController trackingEntityController;

    private void Start()
    {
        if (trackingEntityController == null)
        {
            Hide();
            return;
        }

        trackingEntityController.OnEntityAttackingTimerChanged += TrackingEntityController_OnEntityAttackingTimerChanged;

        GameStageManager.Instance.OnGameStageChanged += GameStageManager_OnGameStageChanged;
    }

    private void GameStageManager_OnGameStageChanged(object sender, EventArgs e)
    {
        if (GameStageManager.Instance.IsFighting())
            Show();
        else
            Hide();
    }

    private void TrackingEntityController_OnEntityAttackingTimerChanged(object sender, EntityController.OnEntityAttackingTimerChangedEventArgs e)
    {
        var currentTimerPercent = (e.currentTimerFull - e.currentTimerLeft) / e.currentTimerFull;

        if (e.isChangingWeapon)
        {
            attackingCircleTimerImage.gameObject.SetActive(false);
            preparingCircleTimerImage.gameObject.SetActive(true);
            currentPhaseImage.sprite = changingWeaponPhaseSprite;
            preparingCircleTimerImage.fillAmount = currentTimerPercent;
        }
        else if (e.isAttackingPhase)
        {
            attackingCircleTimerImage.gameObject.SetActive(true);
            preparingCircleTimerImage.gameObject.SetActive(false);
            currentPhaseImage.sprite = attackingPhaseSprite;
            attackingCircleTimerImage.fillAmount = currentTimerPercent;
        }
        else
        {
            attackingCircleTimerImage.gameObject.SetActive(false);
            preparingCircleTimerImage.gameObject.SetActive(true);
            currentPhaseImage.sprite = preparingPhaseSprite;
            preparingCircleTimerImage.fillAmount = currentTimerPercent;
        }
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
        trackingEntityController.OnEntityAttackingTimerChanged -= TrackingEntityController_OnEntityAttackingTimerChanged;
        GameStageManager.Instance.OnGameStageChanged -= GameStageManager_OnGameStageChanged;
    }
}