#region

using TMPro;
using UnityEngine;

#endregion

public class InventoryItemDescription : MonoBehaviour
{
    #region Variables & References

    [SerializeField] private TextMeshProUGUI inventoryObjectName;
    [SerializeField] private TextMeshProUGUI inventoryObjectDescription;

    [SerializeField] private Transform inventoryObjectEquipmentTransform;
    [SerializeField] private TextMeshProUGUI inventoryObjectWeaponAdditionalStatTypeText;
    [SerializeField] private string inventoryObjectWeaponAdditionalStatTypeTextTranslationSo;
    [SerializeField] private TextMeshProUGUI inventoryObjectWeaponAdditionalStat;

    [SerializeField] private Transform brokenStateTransform;
    [SerializeField] private string brokenTextTranslationsSo;
    [SerializeField] private TextMeshProUGUI brokenText;

    #endregion

    #region Inventory Object Methods

    public void SetInventoryObject(InventoryObject inventoryObject)
    {
        inventoryObjectName.text = inventoryObject.GetInventoryObjectName();
        var inventoryObjectDescriptionText = inventoryObject.GetInventoryObjectDescriptionText();
        inventoryObjectDescriptionText = inventoryObjectDescriptionText == "" ? "..." : inventoryObjectDescriptionText;
        inventoryObjectDescription.text = inventoryObjectDescriptionText;

        if (!inventoryObject.IsBroken())
            brokenStateTransform.gameObject.SetActive(false);
        else
            brokenText.text = brokenTextTranslationsSo;

        if (inventoryObject.TryGetEquipmentSO(out var equipmentSO))
        {
            var additionalStatValueString = equipmentSO.isStatValueFlat
                ? $"{equipmentSO.equipmentAdditionalStatValue}"
                : $"{equipmentSO.equipmentAdditionalStatValue} %";
            inventoryObjectWeaponAdditionalStat.text = $"+ {additionalStatValueString} {equipmentSO.equipmentAdditionalStatType}";
        }
        else
        {
            inventoryObjectEquipmentTransform.gameObject.SetActive(false);
        }
    }

    #endregion
}