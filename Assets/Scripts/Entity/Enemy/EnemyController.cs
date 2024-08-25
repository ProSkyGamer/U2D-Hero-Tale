#region

using System;

#endregion

public class EnemyController : EntityController
{
    protected EnemyVisualController enemyVisualController;

    protected override void Awake()
    {
        base.Awake();

        enemyVisualController = GetComponent<EnemyVisualController>();
    }

    protected override void Start()
    {
        base.Start();

        entityAttackController.OnAttackTypeChanged += EntityAttackController_OnAttackTypeChanged;
    }

    private void EntityAttackController_OnAttackTypeChanged(object sender, EventArgs e)
    {
        entityAttackController.GetArmWeaponSprites(out var leftHandWeaponSprite, out var rightHandWeaponSprite);
        enemyVisualController.ChangeArmWeapons(leftHandWeaponSprite, rightHandWeaponSprite);
    }

    protected override void ResetAnimation()
    {
        enemyVisualController.ChangeAnimationState(EnemyVisualController.Animations.Idle);
    }

    protected override void AttackAnimation()
    {
        var currentWeaponType = entityAttackController.GetCurrentWeaponAttackType();

        enemyVisualController.ChangeAnimationState(EnemyVisualController.Animations.Attack);
        enemyVisualController.SetAttackTime(entityAttackController.GetCurrentAttackTypeTime(), (int)currentWeaponType);
    }

    protected override void AttackEnemy()
    {
        var attackingEnemyComponents = AllEntitiesController.Instance.GetPlayerEntityComponents();

        entityAttackController.Attack(attackingEnemyComponents);
    }
}