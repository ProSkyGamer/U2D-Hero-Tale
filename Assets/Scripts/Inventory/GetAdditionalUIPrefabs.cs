#region

using UnityEngine;

#endregion

public class GetAdditionalUIPrefabs : MonoBehaviour
{
    public static GetAdditionalUIPrefabs Instance { get; private set; }

    #region Variables & References

    [SerializeField] private Transform smallInventoryItemDescriptionPrefab;
    [SerializeField] private Transform mediumInventoryItemDescriptionPrefab;
    [SerializeField] private Transform largeInventoryItemDescriptionPrefab;

    [SerializeField] private Transform smallInventoryItemInteractButtonsPrefab;
    [SerializeField] private Transform mediumInventoryItemInteractButtonsPrefab;
    [SerializeField] private Transform largeInventoryItemInteractButtonsPrefab;

    [SerializeField] private Transform upgradesDescriptionPrefab;

    [SerializeField] private Sprite playerInventoryIconSprite;
    [SerializeField] private Sprite headEquipmentInventoryIconSprite;
    [SerializeField] private Sprite bodyEquipmentInventoryIconSprite;
    [SerializeField] private Sprite pantsEquipmentInventoryIconSprite;
    [SerializeField] private Sprite bootsEquipmentInventoryIconSprite;

    #endregion

    #region Initialization

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #endregion

    #region Get Additional UI Prefabs

    public Transform GetInventoryItemDescriptionPrefabBySize(
        CharacterInventoryUI.InventoryItemDescriptionSize inventoryItemDescriptionSize)
    {
        switch (inventoryItemDescriptionSize)
        {
            default:
                return smallInventoryItemDescriptionPrefab;
            case CharacterInventoryUI.InventoryItemDescriptionSize.Small:
                return smallInventoryItemDescriptionPrefab;
            case CharacterInventoryUI.InventoryItemDescriptionSize.Medium:
                return mediumInventoryItemDescriptionPrefab;
            case CharacterInventoryUI.InventoryItemDescriptionSize.Large:
                return largeInventoryItemDescriptionPrefab;
        }
    }

    public Transform GetInventoryItemInteractButtonPrefabBySize(
        CharacterInventoryUI.InventoryItemInteractButtonsSize inventoryItemInteractButtonsSize)
    {
        switch (inventoryItemInteractButtonsSize)
        {
            default:
                return smallInventoryItemInteractButtonsPrefab;
            case CharacterInventoryUI.InventoryItemInteractButtonsSize.Small:
                return smallInventoryItemInteractButtonsPrefab;
            case CharacterInventoryUI.InventoryItemInteractButtonsSize.Medium:
                return mediumInventoryItemInteractButtonsPrefab;
            case CharacterInventoryUI.InventoryItemInteractButtonsSize.Large:
                return largeInventoryItemInteractButtonsPrefab;
        }
    }

    public Sprite GetInventoryTypeIcon(CharacterInventoryUI.InventoryType inventoryType)
    {
        switch (inventoryType)
        {
            default:
            case CharacterInventoryUI.InventoryType.PlayerInventory:
                return playerInventoryIconSprite;
            case CharacterInventoryUI.InventoryType.PlayerEquipment_Helmet:
                return headEquipmentInventoryIconSprite;
            case CharacterInventoryUI.InventoryType.PlayerEquipment_Body:
                return bodyEquipmentInventoryIconSprite;
            case CharacterInventoryUI.InventoryType.PlayerEquipment_Pants:
                return pantsEquipmentInventoryIconSprite;
            case CharacterInventoryUI.InventoryType.PlayerEquipment_Boots:
                return bootsEquipmentInventoryIconSprite;
        }
    }

    #endregion
}