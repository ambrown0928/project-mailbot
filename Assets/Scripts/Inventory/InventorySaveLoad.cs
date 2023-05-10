using System.Collections;
using System.Collections.Generic;
using System.Storage;
using Inventory.Items;
using Palmmedia.ReportGenerator.Core.Common;
using Saving;
using UnityEngine;

namespace Inventory
{
    public class InventorySaveLoad : SaveLoad<Storage<ItemSaveData>>
    {
        public override void SaveToJson(Storage<ItemSaveData> item, string path)
        {
           base.SaveToJson(item, path);
        }
    }
    
}