#region

using UnityEngine;
using UnityEngine.UI;

#endregion

public class SlotDescriptionUI : MonoBehaviour
{
    [SerializeField] private Button closeDescriptionButton;
    [SerializeField] private InventoryItemDescription inventoryItemDescription;
    [SerializeField] private InventorySlotInteractButtons inventorySlotInteractButtons;

    private void Awake()
    {
        closeDescriptionButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        InventorySlotSingleUI.OnDisplaySlotDescription += InventorySlotSingleUI_OnDisplaySlotDescription;

        Hide();
    }

    private void InventorySlotSingleUI_OnDisplaySlotDescription(object sender, InventorySlotSingleUI.OnDisplaySlotDescriptionEventArgs e)
    {
        Show();
        inventoryItemDescription.SetInventoryObject(e.inventoryObject);
        inventorySlotInteractButtons.SetSlotInfo(e.inventoryObject, e.displayedInventory.GetInventoryType(), Hide);
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
        InventorySlotSingleUI.OnDisplaySlotDescription -= InventorySlotSingleUI_OnDisplaySlotDescription;
    }
}