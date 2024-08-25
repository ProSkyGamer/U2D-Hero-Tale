#region

using System;
using UnityEngine;

#endregion

public class InventoryObject : MonoBehaviour
{
    #region Events

    public static event EventHandler OnInventoryParentChanged;

    public event EventHandler OnObjectRepaired;

    #endregion

    #region Variables & References

    [SerializeField] private Sprite inventoryObjectSprite;
    [SerializeField] private Sprite brokenInventoryObjectSprite;
    [SerializeField] private string inventoryObjectNameText;
    [SerializeField] private string inventoryObjectDescriptionText;
    [SerializeField] private EquipmentSO equipmentSO;

    [SerializeField] private bool isBroken;

    private IInventoryParent inventoryObjectParent;

    #endregion

    #region Inventory Parent

    public void SetInventoryParent(IInventoryParent storedInventory,
        CharacterInventoryUI.InventoryType inventoryType, bool isNeedToSendNotification = false)
    {
        inventoryObjectParent?.RemoveInventoryObjectBySlot(inventoryObjectParent.GetSlotNumberByInventoryObject(this));
        storedInventory.AddInventoryObject(this, isNeedToSendNotification);
        inventoryObjectParent = storedInventory;

        OnInventoryParentChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetInventoryParentBySlot(IInventoryParent storedInventory,
        int intInventoryType, int slotNumber, bool isNeedToSendNotification = false)
    {
        if (!storedInventory.IsSlotNumberAvailable(slotNumber)) return;

        inventoryObjectParent?.RemoveInventoryObjectBySlot(
            inventoryObjectParent.GetSlotNumberByInventoryObject(this));

        storedInventory.AddInventoryObjectToSlot(this, slotNumber, isNeedToSendNotification);
        inventoryObjectParent = storedInventory;

        OnInventoryParentChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveInventoryParent()
    {
        inventoryObjectParent?.RemoveInventoryObjectBySlot(inventoryObjectParent.GetSlotNumberByInventoryObject(this));
        inventoryObjectParent = null;

        OnInventoryParentChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Get Inventory Object Data

    public IInventoryParent GetInventoryObjectParent()
    {
        return inventoryObjectParent;
    }

    public string GetInventoryObjectDescriptionText()
    {
        return inventoryObjectDescriptionText;
    }

    public Sprite GetInventoryObjectSprite()
    {
        return isBroken ? brokenInventoryObjectSprite : inventoryObjectSprite;
    }

    public string GetInventoryObjectName()
    {
        return inventoryObjectNameText;
    }

    public bool IsBroken()
    {
        return isBroken;
    }

    public bool TryGetEquipmentSO(out EquipmentSO equipmentSO)
    {
        equipmentSO = this.equipmentSO;
        return this.equipmentSO != null;
    }

    #endregion

    #region Inventory Object Break States

    public void BreakObject()
    {
        ChangeBrokenObjectState(true);
    }

    public void RepairObject()
    {
        ChangeBrokenObjectState(false);
    }

    private void ChangeBrokenObjectState(bool newState)
    {
        isBroken = newState;

        if (!isBroken)
            OnObjectRepaired?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}