#region

using System;
using UnityEngine;

#endregion

public class EntityHealthController : MonoBehaviour
{
    public event EventHandler OnPlayerHpChanged;
    public event EventHandler OnPlayerDefChanged;

    public event EventHandler OnEntityHPEnded;

    private float baseHP;
    private float baseDef;
    private float maxDefenceAbsorption;
    private float defenceForMaxAbsorption;

    private int additionalDefenceNumberFormula;

    private float currentHPStat;
    private float defStat;
    private float maxHPStat;
    private float additionalHPPercentage;
    private float additionalHPFlat;
    private float additionalDefPercentage;
    private float additionalDefFlat;

    public void InitializeHealthController(EntitySO entitySO)
    {
        baseHP = entitySO.entityBaseHP;
        maxHPStat = baseHP;
        currentHPStat = maxHPStat;

        baseDef = entitySO.entityBaseDef;
        defStat = baseDef;
        maxDefenceAbsorption = entitySO.maxDefenceAbsorption;
        defenceForMaxAbsorption = entitySO.defenceForMaxAbsorption;

        additionalDefenceNumberFormula =
            (int)(defenceForMaxAbsorption * (1 - maxDefenceAbsorption) / maxDefenceAbsorption);
    }

    public void DealDamage(float originalDamageToDeal)
    {
        var damageToDeal = Mathf.Clamp((int)(originalDamageToDeal * (1 - defStat /
            (additionalDefenceNumberFormula + defStat))), 0f, originalDamageToDeal);

        var newCurrentHP = currentHPStat - damageToDeal;
        newCurrentHP = newCurrentHP > maxHPStat ? maxHPStat :
            newCurrentHP < 0 ? 0 : newCurrentHP;
        currentHPStat = newCurrentHP;

        OnPlayerHpChanged?.Invoke(this, EventArgs.Empty);

        if (currentHPStat <= 0)
            OnEntityHPEnded?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(float healingAmount)
    {
        var newCurrentHP = currentHPStat + healingAmount;
        newCurrentHP = newCurrentHP > maxHPStat ? maxHPStat :
            newCurrentHP < 0 ? 0 : newCurrentHP;
        currentHPStat = newCurrentHP;

        Debug.Log($"Healed for {healingAmount}! HP Left: {currentHPStat}");

        OnPlayerHpChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseMaxHP(int toIncreaseFlat)
    {
        additionalHPFlat += toIncreaseFlat;

        var newMaxHP = baseHP * (1f + additionalHPPercentage) + additionalHPFlat;
        var currentHPPercentage = currentHPStat / maxHPStat;

        currentHPStat = currentHPPercentage * newMaxHP;
        maxHPStat = newMaxHP;

        OnPlayerHpChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseMaxHP(float toIncreasePercentage)
    {
        additionalHPPercentage += toIncreasePercentage / 100;

        var newMaxHP = baseHP * (1f + additionalHPPercentage) + additionalHPFlat;
        var currentHPPercentage = currentHPStat / maxHPStat;

        currentHPStat = currentHPPercentage * newMaxHP;
        maxHPStat = newMaxHP;

        OnPlayerHpChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseDef(int toIncreaseFlat)
    {
        additionalDefFlat += toIncreaseFlat;

        var newDef = baseDef * (1f + additionalDefPercentage) + additionalDefFlat;
        defStat = newDef;

        OnPlayerDefChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseDef(float toIncreasePercentage)
    {
        additionalDefPercentage += toIncreasePercentage / 100;

        var newDef = baseDef * (1f + additionalDefPercentage) + additionalDefFlat;
        defStat = newDef;

        OnPlayerDefChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetBaseHP()
    {
        return baseHP;
    }

    public float GetCurrentHP()
    {
        return currentHPStat;
    }

    public float GetMaxHP()
    {
        return maxHPStat;
    }

    public float GetBaseDef()
    {
        return baseDef;
    }

    public float GetCurrentDef()
    {
        return defStat;
    }
}