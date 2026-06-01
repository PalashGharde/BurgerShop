using System;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArr;
    private int cuttingProgress;

    public event EventHandler<IHasProgress.OnProgressUpdateArgs> OnProgressUpdate;
    public static event EventHandler OnCutAction;

    private bool cuttingStarted;

    new public static void ResetStaticData()
    {
        OnCutAction = null;
    } 


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())    
        {   //Counter is Empty
            if (player.HasKitchenObject())      
            {   // Player has a Object
                if (HasValidCuttingRecipe(player.GetKitchenObjectOnTop().GetKitchenObjectSO()))
                {
                    player.GetKitchenObjectOnTop().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    cuttingStarted = false;
                }
                
            }
        }
        else                        
        {   if (!cuttingStarted)
            {
                //There is a KitchenObject on top
                if (!player.HasKitchenObject())      
                {
                    // Player doesnt have a Object
                
                    GetKitchenObjectOnTop().SetKitchenObjectParent(player);

                    // resets Progress bar
                    OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs {
                        progressAmount = 0f, playAnimation=false
                    });
                    }    
                
                else    // player has a object. it can be a plate
                {
                    if(player.GetKitchenObjectOnTop().TryGetPlate(out PlateKitchenObject plate))
                    {
                        // Player is holding a plate
                        if (plate.TryAddIngredientToPlate(GetKitchenObjectOnTop().GetKitchenObjectSO()))
                        {
                            // if adding is successfull
                            GetKitchenObjectOnTop().DestroySelf();

                            // resets Progress bar
                            OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs {
                                progressAmount = 0f, playAnimation=false
                            });
                        }
                        
                    }
                }
            }
        }
    }

    public override void AltInteract()
    {
        if (HasKitchenObject() && HasValidCuttingRecipe(GetKitchenObjectOnTop().GetKitchenObjectSO()))
        {
            
            //There is a Kitchen Object so we need to cut it
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOFromInput(GetKitchenObjectOnTop().GetKitchenObjectSO());
            int maxCutCount = cuttingRecipeSO.maxCutCount;

            cuttingProgress++;
            OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs {
                progressAmount = (float) cuttingProgress/maxCutCount, playAnimation=true
            });
            OnCutAction?.Invoke(this, EventArgs.Empty);

            cuttingStarted = true;  // to prevent picking up object after cutting starts

            if(cuttingProgress >= maxCutCount)
            {
                KitchenObjectSO OutputKitchenObjectSO = GetOutputRecipeForInputAfterCutting(GetKitchenObjectOnTop().GetKitchenObjectSO());
                GetKitchenObjectOnTop().DestroySelf();

                KitchenObject.SpwanKitchenObject(OutputKitchenObjectSO, this);

                cuttingStarted = false;  
            }

        }
    }

    private bool HasValidCuttingRecipe(KitchenObjectSO kitchenObjectSOInput)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOFromInput(kitchenObjectSOInput);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputRecipeForInputAfterCutting(KitchenObjectSO kitchenObjectSOInput)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOFromInput(kitchenObjectSOInput);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.cuttingRecipeOutput;
        }
        else
        {
            return null;
        }
        
    }

    private CuttingRecipeSO GetCuttingRecipeSOFromInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipe in cuttingRecipeSOArr)
        {
            if(cuttingRecipe.cuttingRecipeInput == kitchenObjectSO)
            {
                return cuttingRecipe;
            }
        }
        return null;
    }

}

