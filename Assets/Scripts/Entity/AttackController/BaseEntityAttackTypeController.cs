#region

using UnityEngine;
using Random = UnityEngine.Random;

#endregion

public class BaseEntityAttackTypeController : MonoBehaviour
{
    public enum WeaponType
    {
        Sword,
        Bow
    }

    [SerializeField] private WeaponType weaponType;
    [SerializeField] private float minCurrentAttackTypeMultiplier = .2f;
    [SerializeField] private float maxCurrentAttackTypeMultiplier = .5f;
    [SerializeField] private float currentAttackTypeTime = 1.0f;
    [SerializeField] private Sprite attackingTypeSprite;

    [SerializeField] private Sprite leftHandWeaponSprite;
    [SerializeField] private Sprite rightHandWeaponSprite;

    public void Attack(AllEntitiesController.EntityComponents opponentEntityComponents, float atkStat)
    {
        var currentAttackTypeMultiplier = Random.Range(minCurrentAttackTypeMultiplier, maxCurrentAttackTypeMultiplier);
        var damageToDeal = atkStat * currentAttackTypeMultiplier;
        damageToDeal = Mathf.Floor(damageToDeal);

        var opponentHealthController = opponentEntityComponents.entityHealthController;
        opponentHealthController.DealDamage(damageToDeal);
    }

    public float GetCurrentAttackTypeTime()
    {
        return currentAttackTypeTime;
    }

    public Sprite GetAttackingTypeSprite()
    {
        return attackingTypeSprite;
    }

    public WeaponType GetControllerWeaponType()
    {
        return weaponType;
    }

    public void GetArmWeaponSprites(out Sprite leftHandWeaponSprite, out Sprite rightHandWeaponSprite)
    {
        leftHandWeaponSprite = this.leftHandWeaponSprite;
        rightHandWeaponSprite = this.rightHandWeaponSprite;
    }
}