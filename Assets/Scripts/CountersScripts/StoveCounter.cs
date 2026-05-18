using System;
using UnityEngine;
using UnityEngine.AI;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum FryingState
    {
        Idle,
        Frying,
        Burning,
        Burned,
    }
    
    public event EventHandler<IHasProgress.OnProgressUpdateArgs> OnProgressUpdate;  // For Progress Bar

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;              // For State change
    public class OnStateChangedEventArgs: EventArgs
    {
        public FryingState state;
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    private FryingState state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;

    private void Start()
    {
        state = FryingState.Idle;
        fryingTimer = 0f;
    }

    private void Update()
    {
        if (HasKitchenObject())    
        {
            switch (state)
            {
                case FryingState.Idle:
                    break;

                case FryingState.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs
                    {
                        progressAmount = fryingTimer / fryingRecipeSO.maxFryingTime
                    });
                    
                    if (fryingTimer >= fryingRecipeSO.maxFryingTime)
                    {
                        GetKitchenObjectOnTop().DestroySelf();
                        KitchenObject.SpwanKitchenObject(fryingRecipeSO.output,this);

                        fryingTimer = 0f;
                        fryingRecipeSO = GetFryingRecipeSOFromInput(GetKitchenObjectOnTop().GetKitchenObjectSO());
                        state = FryingState.Burning;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;

                case FryingState.Burning:
                    fryingTimer += Time.deltaTime;
                    OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs
                    {
                        progressAmount = fryingTimer / fryingRecipeSO.maxFryingTime
                    });
                    if (fryingTimer >= fryingRecipeSO.maxFryingTime)
                    {
                        GetKitchenObjectOnTop().DestroySelf();

                        KitchenObject.SpwanKitchenObject(fryingRecipeSO.output,this);

                        state = FryingState.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;

                case FryingState.Burned:
                    OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs
                    {
                        progressAmount = 0f
                    });
                    break;
            }
            
            
            
        }
        
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())    
        {   //Counter is Empty
            if (player.HasKitchenObject())      
            {   // Player has a Object
                if (HasValidFryingRecipe(player.GetKitchenObjectOnTop().GetKitchenObjectSO()))
                {
                    player.GetKitchenObjectOnTop().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOFromInput(GetKitchenObjectOnTop().GetKitchenObjectSO());

                    fryingTimer = 0f;
                    state = fryingRecipeSO.startingState;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    
                }
                
            }
        }
        else                        
        {   //There is a KitchenObject on top
            if (!player.HasKitchenObject())      
            {   // Player doesnt have a Object
                GetKitchenObjectOnTop().SetKitchenObjectParent(player);
                state = FryingState.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs
                {
                    progressAmount = 0f
                });
            }
            else
            {
                if(player.GetKitchenObjectOnTop().TryGetPlate(out PlateKitchenObject plate))
                {
                    // Player is holding a plate
                    if (plate.TryAddIngredientToPlate(GetKitchenObjectOnTop().GetKitchenObjectSO()))
                    {
                        // if adding is successfull
                        GetKitchenObjectOnTop().DestroySelf();
                        state = FryingState.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs
                        {
                            progressAmount = 0f
                        });
                    }
                    
                }
            }
        }
    }

    private bool HasValidFryingRecipe(KitchenObjectSO kitchenObjectSOInput)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOFromInput(kitchenObjectSOInput);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputRecipeForInputAfterFrying(KitchenObjectSO kitchenObjectSOInput)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOFromInput(kitchenObjectSOInput);
        if(fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
        
    }

    private FryingRecipeSO GetFryingRecipeSOFromInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach(FryingRecipeSO fryingRecipe in fryingRecipeSOArray)
        {
            if(fryingRecipe.input == kitchenObjectSO)
            {
                return fryingRecipe;
            }
        }
        return null;
    }

}
