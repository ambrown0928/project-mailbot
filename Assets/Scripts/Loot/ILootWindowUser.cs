using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;

namespace Loot
{
    public interface ILootWindowUser
    {
        void AddLoot(ItemSaveData loot);
        ItemSaveData RemoveLoot();
    }
}
