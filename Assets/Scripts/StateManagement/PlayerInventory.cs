using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace StateManagement
{
    public static class PlayerInventory
    {
        private static readonly Dictionary<string, int> Items = new Dictionary<string, int>();

        public static bool HasItem(string itemName)
        {
            if (!Items.ContainsKey(itemName))
            {
                Items.Add(itemName, 0);
            }
            return Items[itemName] > 0;
        }
        
        public static int ItemCount(string itemName)
        {
            if (!Items.ContainsKey(itemName))
            {
                Items.Add(itemName, 0);
            }
            return Items[itemName];
        }
        
        public static void AddItem(string itemName, int count)
        {
            if (!Items.ContainsKey(itemName))
            {
                Items.Add(itemName, 0);
            }
            Items[itemName] += count;
        }
        
        public static void RemoveItem(string itemName, int count)
        {
            if (!Items.ContainsKey(itemName) || Items[itemName] <= 0)
            {
                return;
            }
            Items[itemName] -= count;
            if (Items[itemName] < 0)
            {
                Items[itemName] = 0;
            }
        }
    }
}