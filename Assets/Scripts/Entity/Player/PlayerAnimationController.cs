#region

using UnityEngine;

#endregion

public class PlayerAnimationController : MonoBehaviour
{
    public enum Animations
    {
        Idle,
        Attack_Sword,
        Attack_Bow,
        Win,
        Lose
    }

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer leftArmWeaponSpriteRenderer;
    [SerializeField] private SpriteRenderer rightArmWeaponSpriteRenderer;

    private readonly float swordAttackAnimationTime = 38f / 60;
    private readonly float bowAttackAnimationTime = 55f / 60;

    private const string ANIMATOR_ANIMATION_STATE_ACCESS_KEY = "State";

    public void ChangeAnimationState(Animations newAnimationState)
    {
        if (newAnimationState != Animations.Attack_Bow && newAnimationState != Animations.Attack_Sword)
            SetAttackTime(1f, -1);

        playerAnimator.SetInteger(ANIMATOR_ANIMATION_STATE_ACCESS_KEY, (int)newAnimationState);
    }

    public void ChangeArmWeapons(Sprite leftArmWeaponSprite, Sprite rightArmWeaponSprite)
    {
        leftArmWeaponSpriteRenderer.sprite = leftArmWeaponSprite;
        rightArmWeaponSpriteRenderer.sprite = rightArmWeaponSprite;
    }

    public void SetAttackTime(float attackStateTime, int weaponTypeInt)
    {
        var attackAnimationSpeed = weaponTypeInt == -1
            ? 1f
            : (BaseEntityAttackTypeController.WeaponType)weaponTypeInt == BaseEntityAttackTypeController.WeaponType.Sword
                ? swordAttackAnimationTime / attackStateTime
                : (BaseEntityAttackTypeController.WeaponType)weaponTypeInt == BaseEntityAttackTypeController.WeaponType.Bow
                    ? bowAttackAnimationTime / attackStateTime
                    : 1f;

        playerAnimator.speed = attackAnimationSpeed;
    }
}