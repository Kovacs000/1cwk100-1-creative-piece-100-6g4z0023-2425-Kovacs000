using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    // Check if an item exists in the inventory
    public bool HasItem(string itemName)
    {
        return items.ContainsKey(itemName) && items[itemName] > 0;
    }

    // Add an item to the inventory
    public void Add(string item, int count)
    {
        if (items.ContainsKey(item))
        {
            items[item] += count;
        }
        else
        {
            items[item] = count;
        }
    }

    // Remove an item from the inventory
    public void Remove(string item, int count = -1)
    {
        if (items.ContainsKey(item))
        {
            if (count == -1)
            {
                items.Remove(item);
            }
            else
            {
                int newCount = items[item] - count;
                if (newCount <= 0)
                {
                    items.Remove(item);
                }
                else
                {
                    items[item] = newCount;
                }
            }
        }
    }

    // Get count of a specific item
    public int GetCount(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            return items[itemName];
        }
        return 0;
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
