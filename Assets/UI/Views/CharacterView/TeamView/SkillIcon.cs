using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting;

public class SkillIcon : Button
{
    public Image Icon;
    public string ItemGuid = "";
    private string _Title;
    public string Title
    {
        get { return _Title; }
        set { _Title = value; }
    }
    public SkillIcon(string title)
    {
        _Title = title;
        // Create a new Image element and add it to the root
        Icon = new Image();
        Add(Icon);

        // Add USS style properties to the elements
        Icon.AddToClassList("image");
        AddToClassList("button");

        // Register event listeners
        RegisterCallback<ClickEvent>(OnPointerDown);
    }

    private void OnPointerDown(ClickEvent evt)
    {
        if (PopupManager.Instance == null)
        {
            Debug.LogError("PopupManager.Instance is null. Ensure PopupManager is initialized before using SkillIcon.");
            return;
        }

        // Not the left mouse button or this is an empty slot
        PopupManager.Instance.ShowPopup(_Title, "Item details go here.");
    }

    public void HoldItem(ItemDetails item)
    {
        Icon.image = item.Icon.texture;
        ItemGuid = item.GUID;
    }


    #region UXML
    [Preserve]
    // public new class UxmlFactory : UxmlFactory<SkillIcon, UxmlTraits> { }
    // [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
