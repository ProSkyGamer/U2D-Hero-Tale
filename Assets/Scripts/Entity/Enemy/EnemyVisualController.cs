#region

using UnityEngine;

#endregion

public class EnemyVisualController : MonoBehaviour
{
    public enum Animations
    {
        Idle,
        Attack,
        Win,
        Lose
    }

    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private SpriteRenderer leftArmWeaponSpriteRenderer;
    [SerializeField] private SpriteRenderer rightArmWeaponSpriteRenderer;

    private readonly float swordAttackAnimationTime = 38f / 60;
    private readonly float bowAttackAnimationTime = 55f / 60;

    private const string ANIMATOR_ANIMATION_STATE_ACCESS_KEY = "State";

    public void ChangeAnimationState(Animations newAnimationState)
    {
        if (newAnimationState != Animations.Attack)
            SetAttackTime(1f, -1);

        enemyAnimator.SetInteger(ANIMATOR_ANIMATION_STATE_ACCESS_KEY, (int)newAnimationState);
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

        enemyAnimator.speed = attackAnimationSpeed;
    }
}