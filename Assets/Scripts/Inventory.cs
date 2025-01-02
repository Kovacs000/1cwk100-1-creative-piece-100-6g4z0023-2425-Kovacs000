using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Store items and their counts
    private Dictionary<string, int> items = new Dictionary<string, int>();

    // Get the count of a specific item
    public int GetCount(string item)
    {
        if (items.ContainsKey(item))
        {
            return items[item]; // Return the count if the item exists
        }
        return 0; // Return 0 if the item doesn't exist
    }

    // Add an item to the inventory
    public void Add(string item, int count)
    {
        if (items.ContainsKey(item))
        {
            items[item] += count; // Add to the existing count
        }
        else
        {
            items[item] = count; // Add the new item with its count
        }
    }

    // Remove an item from the inventory
    public void Remove(string item, int count = -1)
    {
        if (items.ContainsKey(item))
        {
            if (count == -1) // Remove the item completely
            {
                items.Remove(item);
            }
            else
            {
                int newCount = items[item] - count;
                if (newCount < 1)
                {
                    items.Remove(item); // Remove the item if the count drops below 1
                }
                else
                {
                    items[item] = newCount; // Update the item's count
                }
            }
        }
    }

    // Get a string representation of the inventory
    public string GetInventoryString()
    {
        string result = "Inventory:\n";
        foreach (var item in items)
        {
            result += $"{item.Value}x {item.Key}\n"; // Example: "2x Sword"
        }
        return result;
    }
}
