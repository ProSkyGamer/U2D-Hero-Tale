#region

using System;
using TMPro;
using UnityEngine;

#endregion

public class EntityHealthUI : MonoBehaviour
{
    [SerializeField] private EntityHealthController followingEntity;
    [SerializeField] private string entityHealthStringFormat = "{0} / {1}";
    private TextMeshProUGUI followingEntityHPText;

    private void Awake()
    {
        followingEntityHPText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        followingEntity.OnPlayerHpChanged += FollowingEntity_OnPlayerHpChanged;

        UpdateVisual();
    }

    private void FollowingEntity_OnPlayerHpChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        var entityHealthString = string.Format(entityHealthStringFormat, followingEntity.GetCurrentHP(), followingEntity.GetMaxHP());

        followingEntityHPText.text = entityHealthString;
    }
}