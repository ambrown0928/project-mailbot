using System;
using System.Collections;
using System.Collections.Generic;
using Loot;
using UnityEngine;

namespace Inventory.Items
{
    [System.Serializable] [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Package ", order = 1)]
    public class Package : Item
    {
        internal LootObserver lootObserver;
        public LootWindowController lootWindowController;
        public ItemSaveData intendedItem; // items package is meant to have
        public ItemSaveData currentItem;
        
        void Awake() 
        {
            lootObserver = new LootObserver();
        }
        public override void Use()
        {
            lootObserver.Subscribe(lootWindowController.OpenLootWindow(currentItem, name));
        }
        public void RemoveItem()
        {
            currentItem = null;
        }
        public void AddItem(ItemSaveData item)
        {
            if(currentItem.name != "" && currentItem != null && currentItem.Quantity != 0) throw new PackageIsFullException();

            currentItem = item;
        }
    }
}
