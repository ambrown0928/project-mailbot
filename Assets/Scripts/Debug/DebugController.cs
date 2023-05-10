using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using UnityEngine;

namespace Debugging
{
    public class DebugController : MonoBehaviour
    {
        [SerializeField] private InventoryController inventoryController;

        bool showDebug = false;
        string input;

        public static DebugCommand<string, int> ADD_ITEM;
        
        public List<object> commandList;
        
        void OnToggleDebug()
        {
            showDebug = !showDebug;
        }
        void OnReturn()
        {
            if(showDebug)
            {
                HandleInput();
                input = "";
            }
        }
        private void Awake() 
        {
            ADD_ITEM = new DebugCommand<string, int>("add_item", "Add an item of given quantity to inventory", "add_item", (name, quantity) => 
            {
                name = name.Replace("/", " ");
                ItemSaveData item = new ItemSaveData(name, quantity);
                inventoryController.AddItem(item);
            });    
            commandList = new List<object>
            {
                ADD_ITEM,

            };
        }

        private void OnGUI() 
        {
            if(!showDebug) return;

            float y = 0f;

            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0,0,0,0);
            input = GUI.TextField(new Rect(10f,y + 5f, Screen.width - 20f, 20f ), input);
        }

        private void HandleInput()
        {
            string[] properties = input.Split(' ');
            foreach(object o in commandList)
            {
                DebugCommandBase commandBase = o as DebugCommandBase;
                if(!input.Contains(commandBase.commandId)) continue;
                if(o as DebugCommand != null) 
                    (o as DebugCommand).Invoke();
                else if (o as DebugCommand<string, int> != null)
                {
                    (o as DebugCommand<string, int>).Invoke(properties[1], int.Parse(properties[2]));
                }
                    
            }
        }
    }
}
