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
            else
            {
                //Player not carrying anything
            }
        }
        else                        
        {   //There is a KitchenObject on top
            if (player.HasKitchenObject())
            {
                
                if(player.GetKitchenObjectOnTop().TryGetPlate(out PlateKitchenObject plate))
                {
                    // Player is holding a plate
                    if (plate.TryAddIngredientToPlate(GetKitchenObjectOnTop().GetKitchenObjectSO()))
                    {
                        // if adding is successfull
                        GetKitchenObjectOnTop().DestroySelf();
                    }
                    
                }
                else
                {
                    // PLayer is not carrying plate but something else
                    if(GetKitchenObjectOnTop().TryGetPlate(out plate))
                    {
                        // Counter has a plate
                        if (plate.TryAddIngredientToPlate(player.GetKitchenObjectOnTop().GetKitchenObjectSO()))
                        {
                            
                            // if adding is successfull
                            player.GetKitchenObjectOnTop().DestroySelf();
                        }
                    }
                }
                
            }
            else
            {
                // Player doesnt have a Object
                GetKitchenObjectOnTop().SetKitchenObjectParent(player);

                //face plate in players forward direction
                // Vector3 forward = player.transform.forward;
                // forward.y = 0f;
                // forward.Normalize();
                // Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
                // player.GetKitchenObjectOnTop().transform.rotation = rotation;
            }
        }
    }

}
