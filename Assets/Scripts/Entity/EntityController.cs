#region

using System;
using UnityEngine;

#endregion

public class EntityController : MonoBehaviour
{
    public event EventHandler<OnEntityAttackingTimerChangedEventArgs> OnEntityAttackingTimerChanged;

    public class OnEntityAttackingTimerChangedEventArgs : EventArgs
    {
        public bool isAttackingPhase;
        public bool isChangingWeapon;
        public float currentTimerLeft;
        public float currentTimerFull;
    }

    public event EventHandler OnEntityDead;

    [SerializeField] protected EntitySO entitySO;
    [SerializeField] protected float weaponChangeTime;
    protected bool isChangeWeaponRequested;
    protected BaseEntityAttackTypeController changingWeaponAttackType;
    protected float weaponChangeTimer;
    protected bool isChangingWeapon;

    protected float currentAttackPhaseTime;
    protected float currentAttackPhaseTimer;
    protected bool isCurrentlyAttackingPhase;

    protected EntityHealthController entityHealthController;
    protected BaseEntityAttackController entityAttackController;

    private bool isInitialized;
    protected bool isCurrentlyFighting;
    protected bool isPaused;

    protected virtual void Awake()
    {
        entityHealthController = GetComponent<EntityHealthController>();
        entityAttackController = GetComponent<BaseEntityAttackController>();

        InitializeEntity();

        ChangeAttackPhaseTime(entityAttackController.GetAttackPreparationTime());
    }

    protected virtual void InitializeEntity()
    {
        if (entitySO == null || isInitialized) return;

        isInitialized = true;

        entityHealthController.InitializeHealthController(entitySO);
        entityAttackController.InitializeAttackController(entitySO);
    }

    public void InitializeEntitySO(EntitySO newEntitySO)
    {
        if (isInitialized) return;

        entitySO = newEntitySO;
        InitializeEntity();
    }

    protected virtual void Start()
    {
        GameStageManager.Instance.OnGameStageChanged += GameStageManager_OnGameStageChanged;
        entityHealthController.OnEntityHPEnded += EntityHealthController_OnEntityHPEnded;

        isCurrentlyFighting = GameStageManager.Instance.IsFighting();
    }

    private void EntityHealthController_OnEntityHPEnded(object sender, EventArgs e)
    {
        OnEntityDead?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    private void GameStageManager_OnGameStageChanged(object sender, EventArgs e)
    {
        isCurrentlyFighting = GameStageManager.Instance.IsFighting();

        if (!GameStageManager.Instance.IsPaused() && !GameStageManager.Instance.IsFighting())
            ResetAnimation();

        if (GameStageManager.Instance.IsPaused())
        {
            isPaused = true;
            return;
        }

        if (isPaused)
        {
            isPaused = false;
            return;
        }

        isCurrentlyAttackingPhase = false;
        ChangeAttackPhaseTime(entityAttackController.GetAttackPreparationTime());
    }

    protected virtual void Update()
    {
        if (!isCurrentlyFighting) return;

        if (isChangeWeaponRequested && !isCurrentlyAttackingPhase)
        {
            isChangeWeaponRequested = false;
            weaponChangeTimer = weaponChangeTime;
            if (entityAttackController.IsCanChangeAttackTypeTo(changingWeaponAttackType))
                isChangingWeapon = true;
        }

        if (isChangingWeapon)
        {
            weaponChangeTimer -= Time.deltaTime;

            OnEntityAttackingTimerChanged?.Invoke(this, new OnEntityAttackingTimerChangedEventArgs
            {
                isChangingWeapon = true,
                currentTimerFull = weaponChangeTime,
                currentTimerLeft = weaponChangeTimer
            });

            if (weaponChangeTimer <= 0f)
            {
                isChangingWeapon = false;
                entityAttackController.ChangeAttackTypeTo(changingWeaponAttackType);
            }

            return;
        }

        currentAttackPhaseTimer -= Time.deltaTime;

        OnEntityAttackingTimerChanged?.Invoke(this, new OnEntityAttackingTimerChangedEventArgs
        {
            isAttackingPhase = isCurrentlyAttackingPhase,
            currentTimerFull = currentAttackPhaseTime,
            currentTimerLeft = currentAttackPhaseTimer
        });

        if (currentAttackPhaseTimer <= 0f)
        {
            if (isCurrentlyAttackingPhase)
            {
                isCurrentlyAttackingPhase = false;
                AttackEnemy();
                ChangeAttackPhaseTime(entityAttackController.GetAttackPreparationTime());
                ResetAnimation();
            }
            else
            {
                isCurrentlyAttackingPhase = true;
                AttackAnimation();
                ChangeAttackPhaseTime(entityAttackController.GetCurrentAttackTypeTime());
            }
        }
    }

    protected virtual void AttackAnimation()
    {
    }

    protected virtual void ResetAnimation()
    {
    }

    protected virtual void AttackEnemy()
    {
        Debug.LogError("NOT IMPLEMENTED ATTACK CONTROLLER");
    }

    private void ChangeAttackPhaseTime(float newAttackPhaseTime)
    {
        currentAttackPhaseTime = newAttackPhaseTime;
        currentAttackPhaseTimer = currentAttackPhaseTime;
    }

    protected virtual void OnDestroy()
    {
        GameStageManager.Instance.OnGameStageChanged -= GameStageManager_OnGameStageChanged;
        entityHealthController.OnEntityHPEnded -= EntityHealthController_OnEntityHPEnded;
    }
}