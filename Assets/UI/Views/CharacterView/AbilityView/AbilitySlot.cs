using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting;

public class AbilitySlot : Button
{
    public Image Icon;
    public Label Name;
    public string ItemGuid = "";
    public string SkillType = "";
    public Label Level;
    public Label Description;
    public Label Effect;
    public AbilitySlot()
    {
        // Create a new Image element and add it to the root
        Level = new Label();
        Add(Level);
        Icon = new Image();
        Add(Icon);
        Name = new Label(); // Initialize the Label
        Add(Name); // Add the Label to the AbilitySlot
        Effect = new Label();
        Add(Effect);
        Description = new Label();
        Add(Description);
        AddToClassList("ability");
        Icon.AddToClassList("abilityIcon");
        Name.AddToClassList("abilityLabel");
        Description.AddToClassList("abilityDescription");
        Effect.AddToClassList("abilityEffect");
        // Register event listeners
        RegisterCallback<ClickEvent>(OnPointerDown);
    }

   

    /// <summary>
    /// Sets the Icon and GUID properties
    /// </summary>
    /// <param name="item"></param>
    public void HoldItem(AbilityDetails item)
    {

        if (item == null)
        {
            Debug.LogError("Attempted to hold a null item.");
            return;
        }

        if (item.Icon == null || item.Icon.texture == null)
        {
            Debug.LogError($"Item {item.Name} does not have a valid icon or texture.");
            // Optionally, set a default icon here if you have one
            // Icon.image = defaultIconTexture;
        }
        else
        {
            Icon.image = item.Icon.texture;
        }
        Level.text = item.Level;
        ItemGuid = item.GUID;
        Name.text = item.Name;
        SkillType = item.SkillType;
        Description.text = item.Description;
        Effect.text = item.Effect.ToString();
        
    }

    /// <summary>
    /// Clears the Icon and GUID properties
    /// </summary>
    public void DropItem()
    {
        ItemGuid = "";
        Icon.image = null;
    }
    private void OnPointerDown(ClickEvent evt)
    {
        if (PopupManager.Instance == null)
        {
            Debug.LogError("PopupManager.Instance is null. Ensure PopupManager is initialized before using SkillIcon.");
            return;
        }

        // Not the left mouse button or this is an empty slot
        PopupManager.Instance.ShowAbilityDetail(Name.text, Description.text);
    }
    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<AbilitySlot, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
