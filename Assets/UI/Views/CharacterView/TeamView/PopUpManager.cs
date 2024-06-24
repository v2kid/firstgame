using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Linq;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    public UIDocument uiDocument;
    private VisualElement charContent;
    private VisualElement currentPopup;
    private Label titleLabel;
    private Label detailLabel;
    private Button closeButton;
    private Button startButton; // Changed from "Start" to match the UXML
    private VisualElement randomList; // Reference to RandomList VisualElement
    private Coroutine highlightCoroutine; // Coroutine for automatic highlighting

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Optional: Keep the popup manager across scenes
    }

    private void OnEnable()
    {
        // Find CharContent within the UI Document
        var root = uiDocument.rootVisualElement;
        charContent = root.Q<VisualElement>("CharContent");

        if (charContent == null)
        {
            Debug.LogError("CharContent not found in UIDocument.");
            return;
        }
    }

    public void ShowPopup(string title, string details)
    {
        LoadAndShowPopup("SkillDetail", title, details);
    }

    public void ShowAbilityDetail(string title, string description)
    {
        LoadAndShowPopup("AbilityDetail", title, description);
    }

    public void ShowUpgradePopup(string title)
    {
        LoadAndShowPopup("UpgradePopup", title, "Upgrade details...");
    }

    private void LoadAndShowPopup(string popupResourceName, string title, string details)
    {
        // Load the visual tree asset for the popup
        var visualTree = Resources.Load<VisualTreeAsset>(popupResourceName);
        if (visualTree == null)
        {
            Debug.LogError($"VisualTreeAsset '{popupResourceName}' not found in Resources.");
            return;
        }

        // Clear any existing popup
        if (currentPopup != null)
        {
            charContent.Remove(currentPopup);
        }

        // Clone and add the new popup to the charContent
        currentPopup = visualTree.CloneTree();
        charContent.Add(currentPopup);

        // Query the elements within the popup
        titleLabel = currentPopup.Q<Label>("titleLabel");
        detailLabel = currentPopup.Q<Label>("detailLabel");
        closeButton = currentPopup.Q<Button>("closeButton");
        randomList = currentPopup.Q<VisualElement>("RandomList");
        startButton = currentPopup.Q<Button>("Start");

        // Set the content of the popup
        titleLabel.text = title;
        detailLabel.text = details;

        // Register the close button click event
        closeButton.clicked += HidePopup;

        // Register the start button click event to trigger automatic selection
        startButton.clicked += StartAutomaticSelection;

        // Display the popup
        currentPopup.style.display = DisplayStyle.Flex;
    }

    private void StartAutomaticSelection()
    {
        // Stop any ongoing coroutine before starting a new one
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
        }

        // Start the coroutine for automatic highlighting
        highlightCoroutine = StartCoroutine(HighlightItems());
    }
private IEnumerator HighlightItems()
{
    // Ensure randomList is valid
    if (randomList == null)
    {
        yield break;
    }

    int itemCount = randomList.childCount;
    int currentIndex = 0;
    float timer = 0f;
    float highlightDuration = 7f; // Highlight duration in seconds
    float highlightInterval = 0.1f; // Initial time interval between highlights
    float timeElapsedForIncrease = 0f; // Time tracker for increasing interval

    while (timer < highlightDuration)
    {
        // Highlight the current item
        HighlightItem(currentIndex);

        // Wait for the specified interval
        yield return new WaitForSecondsRealtime(highlightInterval);

        // Update timers
        timer += highlightInterval;
        timeElapsedForIncrease += highlightInterval;

        // Move to the next item
        currentIndex = (currentIndex + 1) % itemCount;

        // Increase the interval after every 1 second
        if (timeElapsedForIncrease >= 1f)
        {
            highlightInterval += 0.05f;
            timeElapsedForIncrease = 0f; // Reset the increase tracker
        }
    }

    // Highlight the selected item
    HighlightItem(currentIndex);

    // Optional: Perform any action after highlighting stops

    // Reset the coroutine reference
    highlightCoroutine = null;
}




    private void HighlightItem(int index)
    {
        // Reset all items' styles
        foreach (VisualElement child in randomList.Children())
        {
            child.style.borderBottomColor = new StyleColor(Color.clear);
            child.style.borderTopColor = new StyleColor(Color.clear);
            child.style.borderLeftColor = new StyleColor(Color.clear);
            child.style.borderRightColor = new StyleColor(Color.clear);
        }
        // Highlight the selected item
        VisualElement selectedItem = randomList.ElementAt(index);
        selectedItem.style.borderBottomColor = new StyleColor(Color.red);
        selectedItem.style.borderTopColor = new StyleColor(Color.red);
        selectedItem.style.borderLeftColor = new StyleColor(Color.red);
        selectedItem.style.borderRightColor = new StyleColor(Color.red);
    }

    public void HidePopup()
    {
        // Stop the highlight coroutine if running
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
            highlightCoroutine = null;
        }

        // Close the popup
        if (currentPopup != null)
        {
            currentPopup.style.display = DisplayStyle.None;
            charContent.Remove(currentPopup);
            currentPopup = null;
        }
    }
}
