#region

using System;
using UnityEngine;

#endregion

public class PlayerInventory : MonoBehaviour, IInventoryParent
{
    #region Events

    public event EventHandler OnInventorySizeChanged;

    #endregion

    #region Variables & References

    [SerializeField] private int playerMaxSlots = 10;
    private InventoryObject[] storedInventoryObjects;
    [SerializeField] private InventoryObject[] addStoredInventoryObjects;
    private bool isFirstUpdate = true;

    #endregion

    #region Inititalization

    private void Awake()
    {
        storedInventoryObjects = new InventoryObject[playerMaxSlots];
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            foreach (var inventoryObject in addStoredInventoryObjects)
            {
                inventoryObject.SetInventoryParent(this, CharacterInventoryUI.InventoryType.PlayerInventory);
            }
        }
    }

    #endregion

    #region Add & Remove Inventory Objects

    public void AddInventoryObject(InventoryObject inventoryObject, bool isNeedToSendNotification = false)
    {
        var storedSlot = GetFirstAvailableSlot();
        if (storedSlot == -1) return;

        storedInventoryObjects[storedSlot] = inventoryObject;
    }

    public void AddInventoryObjectToSlot(InventoryObject inventoryObject, int slotNumber, bool isNeedToSendNotification)
    {
        if (!IsSlotNumberAvailable(slotNumber)) return;

        storedInventoryObjects[slotNumber] = inventoryObject;
    }

    public void RemoveInventoryObjectBySlot(int slotNumber)
    {
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
                    storedInventoryObjects[i].RemoveInventoryParent();

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