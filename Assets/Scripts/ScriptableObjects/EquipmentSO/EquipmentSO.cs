#region

using UnityEngine;

#endregion

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
    public enum AdditionalStat
    {
        Atk,
        Def,
        HP,
        AtkPreparationTime
    }

    public enum EquipmentType
    {
        Head,
        Body,
        Pants,
        Boots
    }

    public EquipmentType equipmentType;
    public AdditionalStat equipmentAdditionalStatType;
    public bool isStatValueFlat;
    public float equipmentAdditionalStatValue;
}