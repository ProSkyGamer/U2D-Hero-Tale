#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class BaseEntityAttackController : MonoBehaviour
{
    public event EventHandler OnAtkStatChanged;
    public event EventHandler OnAtkPreparationTimeChanged;
    public event EventHandler OnAttackTypeChanged;

    [SerializeField] protected BaseEntityAttackTypeController firstSelectedEntityAttackController;
    protected float baseAttackPreparationTime = 1.5f;
    private float additionalAttackPreparationTimeDecreasePercentage;
    private float additionalAttackPreparationTimerDecreaseFlat;
    protected readonly List<BaseEntityAttackTypeController> allPlayerAttackControllers = new List<BaseEntityAttackTypeController>();
    protected BaseEntityAttackTypeController currentEntityAttackController;

    protected float baseAtk = 10f;
    protected float atkStat;
    private float additionalAtkPercentage;
    private float additionalAtkFlat;

    protected bool isFirstUpdate = true;

    protected virtual void Awake()
    {
        var playerAttackControllers = GetComponents<BaseEntityAttackTypeController>();
        allPlayerAttackControllers.AddRange(playerAttackControllers);
    }

    public void InitializeAttackController(EntitySO entitySO)
    {
        baseAtk = entitySO.entityBaseAtk;
        atkStat = baseAtk;

        baseAttackPreparationTime = entitySO.entityBaseAttackPreparationTime;
    }

    protected virtual void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            ChangeAttackTypeTo(
                firstSelectedEntityAttackController == null || !allPlayerAttackControllers.Contains(firstSelectedEntityAttackController)
                    ? allPlayerAttackControllers[0]
                    : firstSelectedEntityAttackController);
        }
    }

    public void IncreaseAtk(int toIncreaseFlat)
    {
        additionalAtkFlat += toIncreaseFlat;

        var newAtk = baseAtk * (1f + additionalAtkPercentage) + additionalAtkFlat;
        atkStat = newAtk;

        OnAtkStatChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseAtk(float toIncreasePercentage)
    {
        additionalAtkPercentage += toIncreasePercentage / 100;

        var newAtk = baseAtk * (1f + additionalAtkPercentage) + additionalAtkFlat;
        atkStat = newAtk;

        OnAtkStatChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DecreaseAttackPreparationTimer(int toDecreaseFlat)
    {
        additionalAttackPreparationTimerDecreaseFlat += toDecreaseFlat / 100f;

        OnAtkPreparationTimeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DecreaseAttackPreparationTimer(float toDecreasePercentage)
    {
        additionalAttackPreparationTimeDecreasePercentage += toDecreasePercentage / 100;

        OnAtkPreparationTimeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Attack(AllEntitiesController.EntityComponents opponentEntityComponents)
    {
        currentEntityAttackController.Attack(opponentEntityComponents, atkStat);
    }

    public void ChangeAttackTypeTo(BaseEntityAttackTypeController newAttackTypeController)
    {
        if (!IsCanChangeAttackTypeTo(newAttackTypeController)) return;

        currentEntityAttackController = newAttackTypeController;

        OnAttackTypeChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsCanChangeAttackTypeTo(BaseEntityAttackTypeController newAttackTypeController)
    {
        return allPlayerAttackControllers.Contains(newAttackTypeController);
    }

    public float GetBaseAtk()
    {
        return baseAtk;
    }

    public float GetCurrentAtk()
    {
        return atkStat;
    }

    public float GetBaseAttackPreparationTime()
    {
        return baseAttackPreparationTime;
    }

    public float GetAttackPreparationTime()
    {
        return baseAttackPreparationTime -
               baseAttackPreparationTime * additionalAttackPreparationTimeDecreasePercentage
               - additionalAttackPreparationTimerDecreaseFlat;
    }

    public float GetCurrentAttackTypeTime()
    {
        var currentAttackTypeTime = currentEntityAttackController.GetCurrentAttackTypeTime();
        return currentAttackTypeTime;
    }

    public BaseEntityAttackTypeController.WeaponType GetCurrentWeaponAttackType()
    {
        return currentEntityAttackController.GetControllerWeaponType();
    }

    public void GetArmWeaponSprites(out Sprite leftHandWeaponSprite, out Sprite rightHandWeaponSprite)
    {
        currentEntityAttackController.GetArmWeaponSprites(out leftHandWeaponSprite, out rightHandWeaponSprite);
    }
}