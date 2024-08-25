#region

using System;
using System.Collections.Generic;

#endregion

public class BasePlayerAttackController : BaseEntityAttackController
{
    public static event EventHandler<OnPlayerAttackControllersChangedEventArgs> OnPlayerAttackControllersChanged;

    public class OnPlayerAttackControllersChangedEventArgs : EventArgs
    {
        public List<BaseEntityAttackTypeController> baseEntityAttackTypeControllers;
        public BaseEntityAttackTypeController startingEntityAttackControllerType;
    }

    protected override void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            ChangeAttackTypeTo(
                firstSelectedEntityAttackController == null || !allPlayerAttackControllers.Contains(firstSelectedEntityAttackController)
                    ? allPlayerAttackControllers[0]
                    : firstSelectedEntityAttackController);

            OnPlayerAttackControllersChanged?.Invoke(this, new OnPlayerAttackControllersChangedEventArgs
            {
                baseEntityAttackTypeControllers = allPlayerAttackControllers,
                startingEntityAttackControllerType = currentEntityAttackController
            });
        }
    }
}