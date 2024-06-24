using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AbilityUiController : MonoBehaviour
{

    public List<AbilitySlot> AbilitiesCommonList = new List<AbilitySlot>();
    public List<AbilitySlot> AbilitiesRareList = new List<AbilitySlot>();
    private VisualElement m_Root;
    private Button m_UpgradeButton;
    private VisualElement m_CommonAbilities;
    private VisualElement m_RareAbilities;
    private void Awake()
    {
        // Store the root from the UI Document component
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_CommonAbilities = m_Root.Q<VisualElement>("CommonAbilities");
        m_RareAbilities = m_Root.Q<VisualElement>("RareAbilities");
        m_UpgradeButton = m_Root.Q<Button>("UpgradeButton");
        for (int i = 0; i < 8; i++)
        {
            AbilitySlot item = new AbilitySlot();
            AbilitiesCommonList.Add(item);
            m_CommonAbilities.Add(item);
        }
        for (int i = 0; i < 8; i++)
        {
            AbilitySlot item = new AbilitySlot();
            AbilitiesRareList.Add(item);
            m_RareAbilities.Add(item);
        }
        AbilityController.OnAbilitiesChanged += AbilityController_OnAbilitiesChanged;
        m_UpgradeButton.clicked += UpgradeButton_OnClick;
    }
    private void UpgradeButton_OnClick()
    {
         PopupManager.Instance.ShowUpgradePopup("Upgrade Abilities"); 
    }
   private void AbilityController_OnAbilitiesChanged(string[] itemGuids, AbilityChangeType change)
{
    foreach (string guid in itemGuids)
    {
        var item = AbilityController.GetItemByGuid(guid);
        if (item.SkillType == "Rare")
        {
            var emptySlot = AbilitiesRareList.FirstOrDefault(x => string.IsNullOrEmpty(x.ItemGuid));
            if (emptySlot != null)
            {
                emptySlot.HoldItem(item);
                m_RareAbilities.Add(emptySlot); // Assuming AbilitySlot can be directly added to VisualElement
            }
        }
        else // Assuming any non-rare item is common
        {
            var emptySlot = AbilitiesCommonList.FirstOrDefault(x => string.IsNullOrEmpty(x.ItemGuid));
            if (emptySlot != null)
            {
                emptySlot.HoldItem(item);
                m_CommonAbilities.Add(emptySlot); // Assuming AbilitySlot can be directly added to VisualElement
            }
        }
    }
}
}
