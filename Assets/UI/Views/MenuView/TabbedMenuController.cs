using System.Collections.Generic;
using UnityEngine.UIElements;

public class TabbedMenuController
{
    /* Define member variables */
    private const string tabClassName = "tab";
    private const string currentlySelectedTabClassName = "currentlySelectedTab";
    private const string unselectedContentClassName = "unselectedContent";
    private const string tabNameSuffix = "Tab";
    private const string contentNameSuffix = "Content";

    private readonly VisualElement root = default;
    private readonly HashSet<Button> initializedTabs = new();

    public TabbedMenuController(in VisualElement root)
    {
        this.root = root;
    }

    public void RegisterTabCallbacks()
    {
        UQueryBuilder<Button> tabs = GetAllTabs();
        tabs.ForEach(RegisterTabCallbacks);
    }

    private void RegisterTabCallbacks(Button tab)
    {
        tab.RegisterCallback<ClickEvent>(TabOnClick);
        InitializeNestedTabs(tab);
    }

    private void InitializeNestedTabs(Button tab)
    {
        var nestedTabs = GetAllNestedTabs(tab);
        nestedTabs.ForEach(nestedTab =>
        {
            if (!initializedTabs.Contains(nestedTab))
            {
                RegisterTabCallbacks(nestedTab);
                initializedTabs.Add(nestedTab);
            }
        });
    }

    private void TabOnClick(ClickEvent evt)
    {
        Button clickedTab = evt.currentTarget as Button;
        if (!TabIsCurrentlySelected(clickedTab))
        {
            UQueryBuilder<Button> tabs = GetAllTabs(clickedTab.hierarchy.parent);
            UQueryBuilder<Button> otherSelectedTabs =
                tabs.Where((Button tab) => tab != clickedTab && TabIsCurrentlySelected(tab));
            otherSelectedTabs.ForEach(UnselectTab);
            SelectTab(clickedTab);
        }
    }

    private static bool TabIsCurrentlySelected(in Button tab)
    {
        return tab.ClassListContains(currentlySelectedTabClassName);
    }

    private UQueryBuilder<Button> GetAllTabs(VisualElement context = null)
    {
        context = context ?? root;
        return context.Query<Button>(className: tabClassName);
    }

    private UQueryBuilder<Button> GetAllNestedTabs(Button tab)
    {
        VisualElement content = FindContent(tab);
        return content.Query<Button>(className: tabClassName);
    }

    private void SelectTab(in Button tab)
    {
        tab.AddToClassList(currentlySelectedTabClassName);
        VisualElement content = FindContent(tab);
        content.RemoveFromClassList(unselectedContentClassName);

        InitializeNestedTabs(tab);
    }

    private void UnselectTab(Button tab)
    {
        tab.RemoveFromClassList(currentlySelectedTabClassName);
        VisualElement content = FindContent(tab);
        content.AddToClassList(unselectedContentClassName);
    }

    private static string GenerateContentName(in Button tab)
    {
        int prefixLength = tab.name.Length - tabNameSuffix.Length;
        string prefix = tab.name.Substring(0, prefixLength);
        return prefix + contentNameSuffix;
    }

    private VisualElement FindContent(in Button tab)
    {
        return root.Q(GenerateContentName(tab));
    }
}
