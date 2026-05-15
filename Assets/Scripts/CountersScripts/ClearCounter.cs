using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter
{
  
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())    
        {   //Counter is Empty
            if (player.HasKitchenObject())      
            {   // Player has a Object
                player.GetKitchenObjectOnTop().SetKitchenObjectParent(this);
            }
        }
        else                        
        {   //There is a KitchenObject on top
            if (!player.HasKitchenObject())      
            {   // Player doesnt have a Object
                GetKitchenObjectOnTop().SetKitchenObjectParent(player);
            }
        }
    }

}
