#region

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class PlayerAttackingTypeButtonsChangeUI : MonoBehaviour
{
    [SerializeField] private PlayerChangeAttackTypeButtonSingleUI playerChangeAttackTypeButtonSinglePrefab;

    private void Awake()
    {
        playerChangeAttackTypeButtonSinglePrefab.gameObject.SetActive(false);
    }

    private void Start()
    {
        BasePlayerAttackController.OnPlayerAttackControllersChanged += BasePlayerAttackController_OnPlayerAttackControllersChanged;

        GameStageManager.Instance.OnGameStageChanged += GameStageManager_OnGameStageChanged;
    }

    private void GameStageManager_OnGameStageChanged(object sender, EventArgs e)
    {
        if (GameStageManager.Instance.IsFighting())
            Show();
        else
            Hide();
    }

    private void BasePlayerAttackController_OnPlayerAttackControllersChanged(object sender,
        BasePlayerAttackController.OnPlayerAttackControllersChangedEventArgs e)
    {
        InitializePlayerChangeAttackTypeButtons(e.baseEntityAttackTypeControllers, e.startingEntityAttackControllerType);
    }

    private void InitializePlayerChangeAttackTypeButtons(List<BaseEntityAttackTypeController> baseEntityAttackTypeControllers,
        BaseEntityAttackTypeController startingAttackTypeController)
    {
        ClearAllButtons();

        foreach (var baseEntityAttackTypeController in baseEntityAttackTypeControllers)
        {
            var newChangeAttackTypeButtonTransform = Instantiate(playerChangeAttackTypeButtonSinglePrefab, transform);
            newChangeAttackTypeButtonTransform.gameObject.SetActive(true);
            var newChangeAttackTypeButtonSingleUI =
                newChangeAttackTypeButtonTransform.gameObject.GetComponent<PlayerChangeAttackTypeButtonSingleUI>();
            if (baseEntityAttackTypeController == startingAttackTypeController)
                newChangeAttackTypeButtonTransform.GetComponent<Button>().interactable = false;

            newChangeAttackTypeButtonSingleUI.InitializeButton(baseEntityAttackTypeController);
        }
    }

    private void ClearAllButtons()
    {
        var allTransformsToRemove = gameObject.GetComponents<Transform>();

        foreach (var transformToRemove in allTransformsToRemove)
        {
            if (transformToRemove == transform || transformToRemove == playerChangeAttackTypeButtonSinglePrefab.transform) continue;

            Destroy(transformToRemove.gameObject);
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
        BasePlayerAttackController.OnPlayerAttackControllersChanged -= BasePlayerAttackController_OnPlayerAttackControllersChanged;

        GameStageManager.Instance.OnGameStageChanged -= GameStageManager_OnGameStageChanged;
    }
}