#region

using UnityEngine;
using UnityEngine.UI;

#endregion

public class InterfaceInputButton : MonoBehaviour
{
    #region Variables & References

    [SerializeField] private GameInput.Binding inputBinding = GameInput.Binding.Attack;

    private Button interfaceBindingButton;

    #endregion

    #region Initialization

    private void Awake()
    {
        interfaceBindingButton = GetComponent<Button>();
    }

    private void Start()
    {
        interfaceBindingButton.onClick.AddListener(() => { GameInput.Instance.TriggerBindingButton(inputBinding); });
    }

    #endregion
}