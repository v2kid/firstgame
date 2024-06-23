using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

    public class InventoryUIController : MonoBehaviour
    {
        public List<InventorySlot> InventoryItems = new List<InventorySlot>();

        private VisualElement m_Root;
        private VisualElement m_SlotContainer;
        private static VisualElement m_GhostIcon;
        
        private static bool m_IsDragging;
        private static InventorySlot m_OriginalSlot;

        private void Awake()
        {
            // Store the root from the UI Document component
            m_Root = GetComponent<UIDocument>().rootVisualElement;
            m_GhostIcon = m_Root.Query<VisualElement>("GhostIcon");
            
            // Search the root for the SlotContainer Visual Element
            m_SlotContainer = m_Root.Q<VisualElement>("SlotContainer");

            // Create 12 InventorySlots and add them as children to the SlotContainer
            for (int i = 0; i < 10; i++)
            {
                InventorySlot item = new InventorySlot();
                InventoryItems.Add(item);
                m_SlotContainer.Add(item);
            }

            // Register event listeners
            GameController.OnInventoryChanged += GameController_OnInventoryChanged;
            m_GhostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            m_GhostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        public static void StartDrag(Vector2 position, InventorySlot originalSlot)
        {
            // Set tracking variables
            m_IsDragging = true;
            m_OriginalSlot = originalSlot;

            // Set the new position
            m_GhostIcon.style.top = position.y - m_GhostIcon.layout.height / 2;
            m_GhostIcon.style.left = position.x - m_GhostIcon.layout.width / 2;

            // Set the image
            m_GhostIcon.style.backgroundImage = GameController.GetItemByGuid(originalSlot.ItemGuid).Icon.texture;

            // Flip the visibility on
            m_GhostIcon.style.visibility = Visibility.Visible;
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!m_IsDragging)
            {
                return;
            }

            // Set the new position
            m_GhostIcon.style.top = evt.position.y - m_GhostIcon.layout.height / 2;
            m_GhostIcon.style.left = evt.position.x - m_GhostIcon.layout.width / 2;
        }

     private void OnPointerUp(PointerUpEvent evt)
{
    if (!m_IsDragging)
    {
        return;
    }

    // Check to see if they are dropping the ghost icon over any inventory slots.
    IEnumerable<InventorySlot> slots = InventoryItems.Where(x => x.worldBound.Overlaps(m_GhostIcon.worldBound));

    // Found at least one
    if (slots.Count() != 0)
    {
        InventorySlot closestSlot = slots.OrderBy(x => Vector2.Distance(x.worldBound.position, m_GhostIcon.worldBound.position)).First();

        // Check if the closest slot is the original slot
        if (closestSlot == m_OriginalSlot)
        {
            // Dropped back to the original slot, do nothing
            ResetDrag();
            return;
        }

        // Check if the closest slot already has an item
        if (!string.IsNullOrEmpty(closestSlot.ItemGuid))
        {
            // Handle item merging logic here if needed
            // For now, we'll simply swap the items
            var originalItem = GameController.GetItemByGuid(m_OriginalSlot.ItemGuid);
            var targetItem = GameController.GetItemByGuid(closestSlot.ItemGuid);

            m_OriginalSlot.HoldItem(targetItem);
            closestSlot.HoldItem(originalItem);
        }
        else
        {
            // Set the new inventory slot with the data
            closestSlot.HoldItem(GameController.GetItemByGuid(m_OriginalSlot.ItemGuid));

            // Clear the original slot
            m_OriginalSlot.DropItem();
        }
    }
    else
    {
        // Dropped outside any slot, reset to original slot
        m_OriginalSlot.Icon.image = GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).Icon.texture;
    }

    // Clear dragging related visuals and data
    ResetDrag();
}

        private void ResetDrag()
        {
            m_IsDragging = false;
            m_OriginalSlot = null;
            m_GhostIcon.style.visibility = Visibility.Hidden;
        }

        private void GameController_OnInventoryChanged(string[] itemGuid, InventoryChangeType change)
        {
            foreach (string item in itemGuid)
            {
                if (change == InventoryChangeType.Pickup)
                {
                    var emptySlot = InventoryItems.FirstOrDefault(x => string.IsNullOrEmpty(x.ItemGuid));
                    if (emptySlot != null)
                    {
                        emptySlot.HoldItem(GameController.GetItemByGuid(item));
                    }
                }
            }
        }
    }
