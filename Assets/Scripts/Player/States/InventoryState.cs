using System;
using System.Collections;
using System.Collections.Generic;
using float_oat.Desktop90;
using UnityEngine;

public class InventoryState : ExtraState
{
    private WindowController inventoryController;

    public override void Enter () { }
    public override void Exit () { }
    
    public override void Action (Rigidbody body, GameObject inventoryWindow)
    {
        if(PlayerHasNotOpenedInventory())
        {
            inventoryController = inventoryWindow.GetComponent<WindowController>();
            inventoryController.Open();
        }
        
    }

    private bool PlayerHasNotOpenedInventory()
    {
        return inventoryController == null;
    }
}
