#region

using System;
using UnityEngine;

#endregion

public class PlayerController : EntityController
{
    public event EventHandler OnAnyStatChanged;

    public static PlayerController Instance { get; private set; }

    protected PlayerInventory playerInventory;
    protected PlayerEquipmentHead playerEquipmentHeadInventory;
    protected PlayerEqupmentBody playerEquipmentBodyInventory;
    protected PlayerEquipmentPants playerEquipmentPantsInventory;
    protected PlayerEquipmentBoots playerEquipmentBootsInventory;
    protected PlayerAnimationController playerAnimationController;

    protected override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        base.Awake();

        playerInventory = GetComponent<PlayerInventory>();
        playerEquipmentHeadInventory = GetComponent<PlayerEquipmentHead>();
        playerEquipmentBodyInventory = GetComponent<PlayerEqupmentBody>();
        playerEquipmentPantsInventory = GetComponent<PlayerEquipmentPants>();
        playerEquipmentBootsInventory = GetComponent<PlayerEquipmentBoots>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    protected override void Start()
    {
        base.Start();

        PlayerChangeAttackTypeButtonSingleUI.OnChangedAttackType += PlayerChangeAttackTypeButtonSingleUI_OnChangedAttackType;

        playerEquipmentHeadInventory.OnPlayerEquipmentChanged += PlayerEquipmentHeadInventory_OnPlayerEquipmentChanged;
        playerEquipmentBodyInventory.OnPlayerEquipmentChanged += PlayerEquipmentBodyInventory_OnPlayerEquipmentChanged;
        playerEquipmentPantsInventory.OnPlayerEquipmentChanged += PlayerEquipmentPantsInventory_OnPlayerEquipmentChanged;
        playerEquipmentBootsInventory.OnPlayerEquipmentChanged += PlayerEquipmentBootsInventory_OnPlayerEquipmentChanged;

        entityHealthController.OnPlayerHpChanged += EntityHealthController_OnPlayerHpChanged;
        entityHealthController.OnPlayerDefChanged += EntityHealthController_OnPlayerDefChanged;

        entityAttackController.OnAtkStatChanged += EntityAttackController_OnAtkStatChanged;
        entityAttackController.OnAtkPreparationTimeChanged += EntityAttackController_OnAtkPreparationTimeChanged;

        AllEnemiesController.Instance.OnNewLootAppeared += AllEnemiesController_OnNewLootAppeared;

        entityAttackController.OnAttackTypeChanged += EntityAttackController_OnAttackTypeChanged;
    }

    private void EntityAttackController_OnAttackTypeChanged(object sender, EventArgs e)
    {
        entityAttackController.GetArmWeaponSprites(out var leftHandWeaponSprite, out var rightHandWeaponSprite);
        playerAnimationController.ChangeArmWeapons(leftHandWeaponSprite, rightHandWeaponSprite);
    }

    private void AllEnemiesController_OnNewLootAppeared(object sender, AllEnemiesController.OnNewLootAppearedEventArgs e)
    {
        foreach (var enemyLoot in e.enemyLoot)
        {
            if (!playerInventory.IsHasAnyAvailableSlot()) return;

            Debug.Log($"Adding {enemyLoot.name} {playerInventory.name}");

            var lootNewInventoryObject = Instantiate(enemyLoot);

            lootNewInventoryObject.SetInventoryParent(playerInventory, CharacterInventoryUI.InventoryType.PlayerInventory, true);
        }
    }

    private void EntityHealthController_OnPlayerHpChanged(object sender, EventArgs e)
    {
        OnAnyStatChanged?.Invoke(this, EventArgs.Empty);
    }

    private void EntityHealthController_OnPlayerDefChanged(object sender, EventArgs e)
    {
        OnAnyStatChanged?.Invoke(this, EventArgs.Empty);
    }

    private void EntityAttackController_OnAtkStatChanged(object sender, EventArgs e)
    {
        OnAnyStatChanged?.Invoke(this, EventArgs.Empty);
    }

    private void EntityAttackController_OnAtkPreparationTimeChanged(object sender, EventArgs e)
    {
        OnAnyStatChanged?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerEquipmentHeadInventory_OnPlayerEquipmentChanged(object sender, PlayerEquipmentHead.OnPlayerEquipmentChangedEventArgs e)
    {
        if (e.equipeedInventoryObject != null)
        {
            e.equipeedInventoryObject.TryGetEquipmentSO(out var addedEquipmentSO);
            TryAddEquipmentBuffs(addedEquipmentSO);
        }

        if (e.removedInventoryObject != null)
        {
            e.removedInventoryObject.TryGetEquipmentSO(out var removedEquipmentSO);
            TryRemoveEquipmentBuffs(removedEquipmentSO);
        }
    }

    private void PlayerEquipmentBodyInventory_OnPlayerEquipmentChanged(object sender, PlayerEqupmentBody.OnPlayerEquipmentChangedEventArgs e)
    {
        if (e.equipeedInventoryObject != null)
        {
            e.equipeedInventoryObject.TryGetEquipmentSO(out var addedEquipmentSO);
            TryAddEquipmentBuffs(addedEquipmentSO);
        }

        if (e.removedInventoryObject != null)
        {
            e.removedInventoryObject.TryGetEquipmentSO(out var removedEquipmentSO);
            TryRemoveEquipmentBuffs(removedEquipmentSO);
        }
    }

    private void PlayerEquipmentPantsInventory_OnPlayerEquipmentChanged(object sender, PlayerEquipmentPants.OnPlayerEquipmentChangedEventArgs e)
    {
        if (e.equipeedInventoryObject != null)
        {
            e.equipeedInventoryObject.TryGetEquipmentSO(out var addedEquipmentSO);
            TryAddEquipmentBuffs(addedEquipmentSO);
        }

        if (e.removedInventoryObject != null)
        {
            e.removedInventoryObject.TryGetEquipmentSO(out var removedEquipmentSO);
            TryRemoveEquipmentBuffs(removedEquipmentSO);
        }
    }

    private void PlayerEquipmentBootsInventory_OnPlayerEquipmentChanged(object sender, PlayerEquipmentBoots.OnPlayerEquipmentChangedEventArgs e)
    {
        if (e.equipeedInventoryObject != null)
        {
            e.equipeedInventoryObject.TryGetEquipmentSO(out var addedEquipmentSO);
            TryAddEquipmentBuffs(addedEquipmentSO);
        }

        if (e.removedInventoryObject != null)
        {
            e.removedInventoryObject.TryGetEquipmentSO(out var removedEquipmentSO);
            TryRemoveEquipmentBuffs(removedEquipmentSO);
        }
    }

    private void TryAddEquipmentBuffs(EquipmentSO equipmentSO)
    {
        if (equipmentSO == null) return;

        switch (equipmentSO.equipmentAdditionalStatType)
        {
            case EquipmentSO.AdditionalStat.Atk:
                if (equipmentSO.isStatValueFlat)
                    entityAttackController.IncreaseAtk((int)equipmentSO.equipmentAdditionalStatValue);
                else
                    entityAttackController.IncreaseAtk(equipmentSO.equipmentAdditionalStatValue);
                break;
            case EquipmentSO.AdditionalStat.Def:
                if (equipmentSO.isStatValueFlat)
                    entityHealthController.IncreaseDef((int)equipmentSO.equipmentAdditionalStatValue);
                else
                    entityHealthController.IncreaseDef(equipmentSO.equipmentAdditionalStatValue);
                break;
            case EquipmentSO.AdditionalStat.HP:
                if (equipmentSO.isStatValueFlat)
                    entityHealthController.IncreaseMaxHP((int)equipmentSO.equipmentAdditionalStatValue);
                else
                    entityHealthController.IncreaseMaxHP(equipmentSO.equipmentAdditionalStatValue);
                break;
            case EquipmentSO.AdditionalStat.AtkPreparationTime:
                if (equipmentSO.isStatValueFlat)
                    entityAttackController.DecreaseAttackPreparationTimer((int)equipmentSO.equipmentAdditionalStatValue);
                else
                    entityAttackController.DecreaseAttackPreparationTimer(equipmentSO.equipmentAdditionalStatValue);
                break;
        }
    }

    private void TryRemoveEquipmentBuffs(EquipmentSO equipmentSO)
    {
        if (equipmentSO == null) return;

        switch (equipmentSO.equipmentAdditionalStatType)
        {
            case EquipmentSO.AdditionalStat.Atk:
                if (equipmentSO.isStatValueFlat)
                    entityAttackController.IncreaseAtk((int)-equipmentSO.equipmentAdditionalStatValue);
                else
                    entityAttackController.IncreaseAtk(-equipmentSO.equipmentAdditionalStatValue);
                break;
            case EquipmentSO.AdditionalStat.Def:
                if (equipmentSO.isStatValueFlat)
                    entityHealthController.IncreaseDef((int)-equipmentSO.equipmentAdditionalStatValue);
                else
                    entityHealthController.IncreaseDef(-equipmentSO.equipmentAdditionalStatValue);
                break;
            case EquipmentSO.AdditionalStat.HP:
                if (equipmentSO.isStatValueFlat)
                    entityHealthController.IncreaseMaxHP((int)-equipmentSO.equipmentAdditionalStatValue);
                else
                    entityHealthController.IncreaseMaxHP(-equipmentSO.equipmentAdditionalStatValue);
                break;
            case EquipmentSO.AdditionalStat.AtkPreparationTime:
                if (equipmentSO.isStatValueFlat)
                    entityAttackController.DecreaseAttackPreparationTimer((int)-equipmentSO.equipmentAdditionalStatValue);
                else
                    entityAttackController.DecreaseAttackPreparationTimer(-equipmentSO.equipmentAdditionalStatValue);
                break;
        }
    }

    protected override void ResetAnimation()
    {
        playerAnimationController.ChangeAnimationState(PlayerAnimationController.Animations.Idle);
    }

    private void PlayerChangeAttackTypeButtonSingleUI_OnChangedAttackType(object sender,
        PlayerChangeAttackTypeButtonSingleUI.OnChangedAttackTypeEventArgs e)
    {
        isChangeWeaponRequested = true;
        changingWeaponAttackType = e.switchingAttackType;
    }

    protected override void AttackAnimation()
    {
        var currentWeaponType = entityAttackController.GetCurrentWeaponAttackType();

        playerAnimationController.ChangeAnimationState(
            currentWeaponType == BaseEntityAttackTypeController.WeaponType.Sword
                ? PlayerAnimationController.Animations.Attack_Sword
                : PlayerAnimationController.Animations.Attack_Bow);
        playerAnimationController.SetAttackTime(entityAttackController.GetCurrentAttackTypeTime(), (int)currentWeaponType);
    }

    protected override void AttackEnemy()
    {
        var attackingEnemyComponents = AllEntitiesController.Instance.GetEnemyEntityComponents();

        entityAttackController.Attack(attackingEnemyComponents);
    }

    public void TryFullyHeal()
    {
        if (isCurrentlyFighting) return;

        entityHealthController.Heal(entityHealthController.GetMaxHP());
    }

    public IInventoryParent GetPlayerInventory()
    {
        return playerInventory;
    }

    public IInventoryParent GetPlayerEquipmentHeadInventory()
    {
        return playerEquipmentHeadInventory;
    }

    public IInventoryParent GetPlayerEquipmentBodyInventory()
    {
        return playerEquipmentBodyInventory;
    }

    public IInventoryParent GetPlayerEquipmentPantsInventory()
    {
        return playerEquipmentPantsInventory;
    }

    public IInventoryParent GetPlayerEquipmentBootsInventory()
    {
        return playerEquipmentBootsInventory;
    }

    public float GetBaseAtk()
    {
        return entityAttackController.GetBaseAtk();
    }

    public float GetCurrentAtk()
    {
        return entityAttackController.GetCurrentAtk();
    }

    public float GetBaseAttackPreparationTime()
    {
        return entityAttackController.GetBaseAttackPreparationTime();
    }

    public float GetAttackPreparationTime()
    {
        return entityAttackController.GetAttackPreparationTime();
    }

    public float GetBaseHP()
    {
        return entityHealthController.GetBaseHP();
    }

    public float GetMaxHP()
    {
        return entityHealthController.GetMaxHP();
    }

    public float GetBaseDef()
    {
        return entityHealthController.GetBaseDef();
    }

    public float GetCurrentDef()
    {
        return entityHealthController.GetCurrentDef();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        PlayerChangeAttackTypeButtonSingleUI.OnChangedAttackType -= PlayerChangeAttackTypeButtonSingleUI_OnChangedAttackType;

        playerEquipmentHeadInventory.OnPlayerEquipmentChanged -= PlayerEquipmentHeadInventory_OnPlayerEquipmentChanged;
        playerEquipmentBodyInventory.OnPlayerEquipmentChanged -= PlayerEquipmentBodyInventory_OnPlayerEquipmentChanged;
        playerEquipmentPantsInventory.OnPlayerEquipmentChanged -= PlayerEquipmentPantsInventory_OnPlayerEquipmentChanged;
        playerEquipmentBootsInventory.OnPlayerEquipmentChanged -= PlayerEquipmentBootsInventory_OnPlayerEquipmentChanged;

        entityHealthController.OnPlayerHpChanged -= EntityHealthController_OnPlayerHpChanged;
        entityHealthController.OnPlayerDefChanged -= EntityHealthController_OnPlayerDefChanged;

        entityAttackController.OnAtkStatChanged -= EntityAttackController_OnAtkStatChanged;
        entityAttackController.OnAtkPreparationTimeChanged -= EntityAttackController_OnAtkPreparationTimeChanged;

        AllEnemiesController.Instance.OnNewLootAppeared -= AllEnemiesController_OnNewLootAppeared;

        entityAttackController.OnAttackTypeChanged -= EntityAttackController_OnAttackTypeChanged;
    }
}