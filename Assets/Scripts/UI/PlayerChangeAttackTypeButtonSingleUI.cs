#region

using System;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class PlayerChangeAttackTypeButtonSingleUI : MonoBehaviour
{
    public static event EventHandler<OnChangedAttackTypeEventArgs> OnChangedAttackType;

    public class OnChangedAttackTypeEventArgs : EventArgs
    {
        public BaseEntityAttackTypeController switchingAttackType;
    }

    [SerializeField] private Image attackingTypeImage;

    private BaseEntityAttackTypeController switchingAttackType;

    private Button switchingAttackTypeButton;

    public void InitializeButton(BaseEntityAttackTypeController newSwitchingAttackType)
    {
        if (switchingAttackType != null) return;

        OnChangedAttackType += PlayerChangeAttackTypeButtonSingleUI_OnChangedAttackType;

        switchingAttackType = newSwitchingAttackType;

        attackingTypeImage.sprite = newSwitchingAttackType.GetAttackingTypeSprite();

        switchingAttackTypeButton = GetComponent<Button>();

        switchingAttackTypeButton.onClick.AddListener(() =>
        {
            OnChangedAttackType?.Invoke(this, new OnChangedAttackTypeEventArgs
            {
                switchingAttackType = switchingAttackType
            });
        });
    }

    private void PlayerChangeAttackTypeButtonSingleUI_OnChangedAttackType(object sender, OnChangedAttackTypeEventArgs e)
    {
        switchingAttackTypeButton.interactable = e.switchingAttackType != switchingAttackType;
    }

    private void OnDestroy()
    {
        OnChangedAttackType -= PlayerChangeAttackTypeButtonSingleUI_OnChangedAttackType;
    }
}