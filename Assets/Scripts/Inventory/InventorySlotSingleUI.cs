#region

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#endregion

public class InventorySlotSingleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    #region Events & Event Args

    public static event EventHandler<OnStartItemDraggingEventArgs> OnStartItemDragging;

    public class OnStartItemDraggingEventArgs : EventArgs
    {
        public InventoryObject draggingInventoryObject;
        public CharacterInventoryUI previousInventoryParent;
        public int previousInventorySlotNumber;
    }

    public static event EventHandler OnCurrentSlotSelected;
    public static event EventHandler<OnDisplaySlotDescriptionEventArgs> OnDisplaySlotDescription;

    public class OnDisplaySlotDescriptionEventArgs : EventArgs
    {
        public InventoryObject inventoryObject;
        public CharacterInventoryUI displayedInventory;
    }

    //public static event EventHandler OnStopDisplaySlotDescription;

    //public static event EventHandler<OnDisplaySlotInteractButtonsEventArgs> OnDisplaySlotInteractButtons;

    public class OnDisplaySlotInteractButtonsEventArgs : EventArgs
    {
        public int slotNumber;
        public InventoryObject inventoryObject;
        public CharacterInventoryUI displayedInventory;
    }

    #endregion

    #region Variables & References

    private CharacterInventoryUI.InventoryType inventoryType;

    [SerializeField] private Image inventorySlotImage;
    [SerializeField] private Image inventorySlotTypeImage;
    [SerializeField] private TextMeshProUGUI inventorySlotText;
    private TextMeshProUGUI inventorySlotTextTranslationSingleUI;
    [SerializeField] private Transform lockedInventorySlotTransform;
    [SerializeField] private Transform selectedInventorySlotTransform;

    private Button slotButton;

    private int slotNumber = -1;
    private InventoryObject storedItem;

    private bool isCurrentlyDragging;
    private bool isCurrentSlotSelected;
    private bool isInteractable;
    private bool isShowingName;

    private bool isSubscribedToShowingButton;
    //private bool isShowingButtons;

    private CharacterInventoryUI inventoryParent;

    #endregion

    #region Initialization

    public void SetStarterData(int newSlotNumber, CharacterInventoryUI.InventoryType newInventoryType,
        bool isSlotInteractable, bool isShowingObjectName, CharacterInventoryUI inventoryUI)
    {
        if (slotNumber != -1) return;

        slotNumber = newSlotNumber;
        inventoryType = newInventoryType;
        isInteractable = isSlotInteractable;
        isShowingName = isShowingObjectName;
        inventoryParent = inventoryUI;

        inventorySlotTypeImage.sprite = GetAdditionalUIPrefabs.Instance.GetInventoryTypeIcon(newInventoryType);

        UpdateVisual();
    }

    private void Awake()
    {
        inventorySlotTextTranslationSingleUI = inventorySlotText.GetComponent<TextMeshProUGUI>();
        slotButton = GetComponent<Button>();

        slotButton.onClick.AddListener(() =>
        {
            if (storedItem == null) return;

            OnDisplaySlotDescription?.Invoke(this, new OnDisplaySlotDescriptionEventArgs
            {
                displayedInventory = inventoryParent,
                inventoryObject = storedItem
            });
        });

        OnStartItemDragging += InventorySlotSingleUI_OnStartItemDragging;
        //OnDisplaySlotDescription += InventorySingleUI_OnDisplaySlotDescription;
    }

    #endregion

    #region Item Dragging

    private void Start()
    {
        InventorySlotInteractButtons.OnStartDragging += InventorySlotInteractButtons_OnStartDragging;
    }

    private void InventorySlotInteractButtons_OnStartDragging(object sender, InventorySlotInteractButtons.OnStartDraggingEventArgs e)
    {
        if (e.draggingObject != storedItem) return;

        StartDraggingItem();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isInteractable) return;
        if (storedItem == null) return;
        /*if (isShowingButtons) return;*/

        /*StartDraggingItem();*/
    }

    private void StartDraggingItem()
    {
        if (!isInteractable) return;

        OnStartItemDragging?.Invoke(this, new OnStartItemDraggingEventArgs
        {
            draggingInventoryObject = storedItem,
            previousInventoryParent = inventoryParent,
            previousInventorySlotNumber = slotNumber
        });

        RemoveItem();
    }

    private void InventorySlotSingleUI_OnStartItemDragging(object sender, OnStartItemDraggingEventArgs e)
    {
        if (!isInteractable) return;

        isCurrentlyDragging = true;
        OnCurrentSlotSelected += InventorySlotSingleUI_OnCurrentSlotSelected;
        CharacterInventoryUI.OnStopItemDragging += OnStopItemDragging;

        var slotSingleUI = sender as InventorySlotSingleUI;

        if (!IsCurrentDraggingObjectMatchesCurrentInventoryType())
            lockedInventorySlotTransform.gameObject.SetActive(true);

        if (slotSingleUI != this) return;

        isCurrentSlotSelected = true;
        selectedInventorySlotTransform.gameObject.SetActive(true);
    }

    private void InventorySlotSingleUI_OnCurrentSlotSelected(object sender, EventArgs e)
    {
        if (!isInteractable) return;

        var slotSingleUI = sender as InventorySlotSingleUI;

        if (slotSingleUI == this) return;

        isCurrentSlotSelected = false;
        selectedInventorySlotTransform.gameObject.SetActive(false);
    }

    private void OnStopItemDragging(object sender, EventArgs e)
    {
        if (!isInteractable) return;

        isCurrentSlotSelected = false;
        isCurrentlyDragging = false;
        lockedInventorySlotTransform.gameObject.SetActive(false);
        selectedInventorySlotTransform.gameObject.SetActive(false);

        CharacterInventoryUI.OnStopItemDragging -= OnStopItemDragging;
    }

    #endregion

    #region Item Description

    private void InventorySingleUI_OnDisplaySlotDescription(object sender, OnDisplaySlotDescriptionEventArgs e)
    {
        /*isShowingButtons = false;*/
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInteractable) return;

        /*if (storedItem != null)
        {
            OnDisplaySlotDescription?.Invoke(this, new OnDisplaySlotDescriptionEventArgs
            {
                inventoryObject = storedItem, displayedInventory = inventoryParent
            });
            GameInput.Instance.OnInventorySlotInteractAction += GameInput_OnInventorySlotInteractAction;
            isSubscribedToShowingButton = true;
        }*/

        if (!isCurrentlyDragging) return;
        if (!IsCurrentDraggingObjectMatchesCurrentInventoryType()) return;

        selectedInventorySlotTransform.gameObject.SetActive(true);
        isCurrentSlotSelected = true;
        OnCurrentSlotSelected?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Item Interact Buttons

    private void GameInput_OnInventorySlotInteractAction(object sender, EventArgs e)
    {
        /*isShowingButtons = true;
        OnDisplaySlotInteractButtons?.Invoke(this, new OnDisplaySlotInteractButtonsEventArgs
        {
            inventoryObject = storedItem, displayedInventory = inventoryParent, slotNumber = slotNumber
        });*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*OnStopDisplaySlotDescription?.Invoke(this, EventArgs.Empty);

        if (isSubscribedToShowingButton)
        {
            GameInput.Instance.OnInventorySlotInteractAction -= GameInput_OnInventorySlotInteractAction;
            isSubscribedToShowingButton = false;
        }*/
    }

    #endregion

    #region Inventory Slot Methods

    public void StoreItem(InventoryObject inventoryObject)
    {
        storedItem = inventoryObject;

        UpdateVisual();
    }

    public void RemoveItem()
    {
        storedItem = null;

        UpdateVisual();
    }

    #endregion

    #region Inventory Slot Visual

    private void UpdateVisual()
    {
        if (storedItem != null)
        {
            inventorySlotImage.gameObject.SetActive(true);
            inventorySlotImage.sprite = storedItem.GetInventoryObjectSprite();

            if (inventorySlotTextTranslationSingleUI == null)
                inventorySlotTextTranslationSingleUI = inventorySlotText.GetComponent<TextMeshProUGUI>();

            if (isShowingName)
            {
                inventorySlotText.gameObject.SetActive(true);
                inventorySlotTextTranslationSingleUI.text = storedItem.GetInventoryObjectName();
            }
        }
        else
        {
            inventorySlotImage.gameObject.SetActive(false);
            inventorySlotText.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Get Inventory Slot Data

    private bool IsCurrentDraggingObjectMatchesCurrentInventoryType()
    {
        switch (inventoryType)
        {
            default:
                return false;
            case CharacterInventoryUI.InventoryType.PlayerInventory:
                return true;
            case CharacterInventoryUI.InventoryType.PlayerEquipment_Helmet:
                CharacterInventoryUI.GetCurrentDraggingObject().TryGetEquipmentSO(out var equipmentSO);
                return equipmentSO.equipmentType == EquipmentSO.EquipmentType.Head;
            case CharacterInventoryUI.InventoryType.PlayerEquipment_Body:
                CharacterInventoryUI.GetCurrentDraggingObject().TryGetEquipmentSO(out equipmentSO);
                return equipmentSO.equipmentType == EquipmentSO.EquipmentType.Body;
            case CharacterInventoryUI.InventoryType.PlayerEquipment_Pants:
                CharacterInventoryUI.GetCurrentDraggingObject().TryGetEquipmentSO(out equipmentSO);
                return equipmentSO.equipmentType == EquipmentSO.EquipmentType.Pants;
            case CharacterInventoryUI.InventoryType.PlayerEquipment_Boots:
                CharacterInventoryUI.GetCurrentDraggingObject().TryGetEquipmentSO(out equipmentSO);
                return equipmentSO.equipmentType == EquipmentSO.EquipmentType.Boots;
        }
    }

    public bool IsHasStoredItem()
    {
        return storedItem != null;
    }

    public int GetSlotNumber()
    {
        return slotNumber;
    }

    public InventoryObject GetStoredItem()
    {
        return storedItem;
    }

    public bool IsCurrentSlotSelected()
    {
        return isCurrentSlotSelected;
    }

    #endregion

    public static void ResetStaticData()
    {
        OnStartItemDragging = null;
        OnCurrentSlotSelected = null;
        /*OnDisplaySlotDescription = null;
        OnStopDisplaySlotDescription = null;
        OnDisplaySlotInteractButtons = null;*/
    }
}