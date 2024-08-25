#region

using System;
using TMPro;
using UnityEngine;

#endregion

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHPStatText;
    [SerializeField] private TextMeshProUGUI playerAtkStatText;
    [SerializeField] private TextMeshProUGUI playerDefStatText;
    [SerializeField] private TextMeshProUGUI playerAttackPreparationSpeedStatText;
    [SerializeField] private string baseStringFormat = "{0} + {1}";
    [SerializeField] private string playerAttackPreparationSpeedStringFormat = "{0} - {1}";

    private void Start()
    {
        PlayerController.Instance.OnAnyStatChanged += PlayerController_OnAnyStatChanged;
    }

    private void PlayerController_OnAnyStatChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        var basePlayerHP = PlayerController.Instance.GetBaseHP();
        var addedPlayerHP = PlayerController.Instance.GetMaxHP() - basePlayerHP;

        var playerHPStatString = string.Format(baseStringFormat, basePlayerHP, addedPlayerHP);

        var basePlayerAtk = PlayerController.Instance.GetBaseAtk();
        var addedPlayerAtk = PlayerController.Instance.GetCurrentAtk() - basePlayerAtk;

        var playerAtkStatString = string.Format(baseStringFormat, basePlayerAtk, addedPlayerAtk);

        var basePlayerDef = PlayerController.Instance.GetBaseDef();
        var addedPlayerDef = PlayerController.Instance.GetCurrentDef() - basePlayerDef;

        var playerDefStatString = string.Format(baseStringFormat, basePlayerDef, addedPlayerDef);

        var basePlayerAtkPrepTime = PlayerController.Instance.GetBaseAttackPreparationTime();
        var addedPlayerAtkPrepTime = basePlayerAtkPrepTime - PlayerController.Instance.GetAttackPreparationTime();

        var playerAtkPrepTimeStatString = string.Format(playerAttackPreparationSpeedStringFormat, basePlayerAtkPrepTime, addedPlayerAtkPrepTime);

        playerHPStatText.text = playerHPStatString;
        playerAtkStatText.text = playerAtkStatString;
        playerDefStatText.text = playerDefStatString;
        playerAttackPreparationSpeedStatText.text = playerAtkPrepTimeStatString;
    }
}