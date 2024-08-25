#region

using System;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class InventorySlotInteractButtons : MonoBehaviour
{
    public static event EventHandler<OnStartDraggingEventArgs> OnStartDragging;

    public class OnStartDraggingEventArgs : EventArgs
    {
        public InventoryObject draggingObject;
    }

    #region Variables & References

    [SerializeField] private Button unEquipItem;
    [SerializeField] private Button equipItem;
    [SerializeField] private Button changeItem;
    [SerializeField] private Button deleteItem;

    #endregion

    #region Slot Buttons Methods

    public void SetSlotInfo(InventoryObject inventoryObject,
        CharacterInventoryUI.InventoryType storedInventoryType, Action onClickAction)
    {
        deleteItem.gameObject.SetActive(true);
        equipItem.gameObject.SetActive(true);
        unEquipItem.gameObject.SetActive(true);
        changeItem.gameObject.SetActive(true);

        deleteItem.onClick.RemoveAllListeners();
        equipItem.onClick.RemoveAllListeners();
        unEquipItem.onClick.RemoveAllListeners();
        changeItem.onClick.RemoveAllListeners();

        deleteItem.onClick.AddListener(() =>
        {
            inventoryObject.RemoveInventoryParent();
            onClickAction();
        });

        var inventoryReference = inventoryObject.GetInventoryObjectParent();
        if (storedInventoryType == CharacterInventoryUI.InventoryType.PlayerInventory)
        {
            unEquipItem.gameObject.SetActive(false);

            if (!inventoryObject.TryGetEquipmentSO(out var equipmentSO))
            {
                equipItem.gameObject.SetActive(false);
                changeItem.gameObject.SetActive(false);
            }
            else
            {
                IInventoryParent equipmentInventory;
                switch (equipmentSO.equipmentType)
                {
                    default:
                    case EquipmentSO.EquipmentType.Head:
                        equipmentInventory = PlayerController.Instance.GetPlayerEquipmentHeadInventory();
                        break;
                    case EquipmentSO.EquipmentType.Body:
                        equipmentInventory = PlayerController.Instance.GetPlayerEquipmentBodyInventory();
                        break;
                    case EquipmentSO.EquipmentType.Pants:
                        equipmentInventory = PlayerController.Instance.GetPlayerEquipmentPantsInventory();
                        break;
                    case EquipmentSO.EquipmentType.Boots:
                        equipmentInventory = PlayerController.Instance.GetPlayerEquipmentBootsInventory();
                        break;
                }

                if (equipmentInventory.IsHasAnyAvailableSlot())
                {
                    changeItem.gameObject.SetActive(false);
                    equipItem.onClick.AddListener(() =>
                    {
                        inventoryObject.SetInventoryParent(equipmentInventory, CharacterInventoryUI.InventoryType.PlayerInventory);
                        onClickAction();
                    });
                }
                else
                {
                    equipItem.gameObject.SetActive(false);
                    changeItem.onClick.AddListener(() =>
                    {
                        var changingInventoryObject = equipmentInventory.GetCurrentInventoryObjectsCount() > 0
                            ? equipmentInventory.GetInventoryObjectBySlot(0)
                            : null;

                        if (changingInventoryObject == null)
                        {
                            Debug.LogError("NO SPACE IN INVENTORY");
                            return;
                        }

                        var currentInventorySlot = inventoryReference.GetSlotNumberByInventoryObject(inventoryObject);

                        var currentItemInventory = inventoryObject.GetInventoryObjectParent();

                        inventoryObject.RemoveInventoryParent();

                        changingInventoryObject.SetInventoryParentBySlot(currentItemInventory, 0, currentInventorySlot);
                        inventoryObject.SetInventoryParentBySlot(equipmentInventory, 0, 0);
                        onClickAction();
                    });
                }
            }
        }
        else
        {
            equipItem.gameObject.SetActive(false);
            changeItem.gameObject.SetActive(false);

            inventoryReference = PlayerController.Instance.GetPlayerInventory();
            if (inventoryReference.IsHasAnyAvailableSlot())
                unEquipItem.onClick.AddListener(() =>
                {
                    inventoryObject.SetInventoryParent(inventoryReference, 0);
                    onClickAction();
                });
        }
    }

    #endregion
}