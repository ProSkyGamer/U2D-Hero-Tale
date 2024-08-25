#region

using UnityEngine;

#endregion

[CreateAssetMenu]
public class EntitySO : ScriptableObject
{
    public float entityBaseHP = 10f;
    public float entityBaseAtk = 10f;
    public float entityBaseDef = 10f;
    [Range(0f, 1f)] public float maxDefenceAbsorption = .5f;
    public float defenceForMaxAbsorption = 500f;

    public float entityBaseAttackPreparationTime = 1.5f;
}