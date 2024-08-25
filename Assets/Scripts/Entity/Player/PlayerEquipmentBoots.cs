#region

using System;
using UnityEngine;

#endregion

public class PlayerEquipmentBoots : MonoBehaviour, IInventoryParent
{
    #region Events

    public event EventHandler OnInventorySizeChanged;
    public event EventHandler<OnPlayerEquipmentChangedEventArgs> OnPlayerEquipmentChanged;

    public class OnPlayerEquipmentChangedEventArgs : EventArgs
    {
        public InventoryObject equipeedInventoryObject;
        public InventoryObject removedInventoryObject;
    }

    #endregion

    #region Variables & References

    [SerializeField] private int playerMaxSlots = 10;
    private InventoryObject[] storedInventoryObjects;

    #endregion

    #region Inititalization

    private void Awake()
    {
        storedInventoryObjects = new InventoryObject[playerMaxSlots];
    }

    #endregion

    #region Add & Remove Inventory Objects

    public void AddInventoryObject(InventoryObject inventoryObject, bool isNeedToSendNotification)
    {
        var storedSlot = GetFirstAvailableSlot();
        if (storedSlot == -1) return;

        storedInventoryObjects[storedSlot] = inventoryObject;

        OnPlayerEquipmentChanged?.Invoke(this, new OnPlayerEquipmentChangedEventArgs
        {
            equipeedInventoryObject = inventoryObject
        });
    }

    public void AddInventoryObjectToSlot(InventoryObject inventoryObject, int slotNumber, bool isNeedToSendNotification)
    {
        if (!IsSlotNumberAvailable(slotNumber)) return;

        storedInventoryObjects[slotNumber] = inventoryObject;

        OnPlayerEquipmentChanged?.Invoke(this, new OnPlayerEquipmentChangedEventArgs
        {
            equipeedInventoryObject = inventoryObject
        });
    }

    public void RemoveInventoryObjectBySlot(int slotNumber)
    {
        OnPlayerEquipmentChanged?.Invoke(this, new OnPlayerEquipmentChangedEventArgs
        {
            removedInventoryObject = storedInventoryObjects[slotNumber]
        });

        storedInventoryObjects[slotNumber] = null;
    }

    #endregion

    #region Inventory Size

    public void ChangeInventorySize(int newSize)
    {
        if (playerMaxSlots == newSize) return;

        var newPlayerMaxSlots = newSize;

        if (storedInventoryObjects.Length > newSize)
            for (var i = newPlayerMaxSlots; i < storedInventoryObjects.Length; i++)
                if (storedInventoryObjects[i] != null)
                {
                    OnPlayerEquipmentChanged?.Invoke(this, new OnPlayerEquipmentChangedEventArgs
                    {
                        removedInventoryObject = storedInventoryObjects[i]
                    });
                    storedInventoryObjects[i].RemoveInventoryParent();
                }

        var newStoredRelicsInventory = new InventoryObject[newPlayerMaxSlots];

        for (var i = 0; i < storedInventoryObjects.Length; i++)
        {
            var storedRelic = storedInventoryObjects[i];
            newStoredRelicsInventory[i] = storedRelic;
        }

        playerMaxSlots = newPlayerMaxSlots;

        storedInventoryObjects = newStoredRelicsInventory;

        OnInventorySizeChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Get Inventory Data

    public InventoryObject GetInventoryObjectBySlot(int slotNumber)
    {
        return storedInventoryObjects[slotNumber];
    }

    public bool IsHasAnyInventoryObject()
    {
        foreach (var inventoryObject in storedInventoryObjects)
        {
            if (inventoryObject != null)
                return true;
        }

        return false;
    }

    public bool IsSlotNumberAvailable(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= storedInventoryObjects.Length) return false;

        return storedInventoryObjects[slotNumber] == null;
    }

    public bool IsHasAnyAvailableSlot()
    {
        foreach (var inventoryObject in storedInventoryObjects)
        {
            if (inventoryObject == null)
                return true;
        }

        return false;
    }

    public int GetFirstAvailableSlot()
    {
        for (var i = 0; i < storedInventoryObjects.Length; i++)
            if (storedInventoryObjects[i] == null)
                return i;

        return -1;
    }

    public int GetSlotNumberByInventoryObject(InventoryObject inventoryObject)
    {
        for (var i = 0; i < storedInventoryObjects.Length; i++)
            if (storedInventoryObjects[i] == inventoryObject)
                return i;

        return -1;
    }

    public int GetMaxSlotsCount()
    {
        return playerMaxSlots;
    }

    public int GetCurrentInventoryObjectsCount()
    {
        var storedInventoryObjectsCount = 0;
        foreach (var inventoryObject in storedInventoryObjects)
        {
            if (inventoryObject != null)
                storedInventoryObjectsCount++;
        }

        return storedInventoryObjectsCount;
    }

    #endregion
}