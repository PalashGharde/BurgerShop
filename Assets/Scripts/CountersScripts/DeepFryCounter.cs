using System;
using UnityEngine;
using UnityEngine.AI;

public class DeepFryCounter : BaseCounter, IHasProgress
{

    public enum DeepFryState
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
        public DeepFryState state;
    }


    [SerializeField] private FryingRecipeSO[] DeepFryRecipeSOs;
    [SerializeField] private KitchenObjectSO packedFries;

    private DeepFryState state;
    private float fryingTimer;

    private FryingRecipeSO fryingRecipeSO;

    private void Update()
    {
        switch (state)
        {
            case DeepFryState.Idle:
            break;

            case DeepFryState.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs
                    {
                        progressAmount = fryingTimer / fryingRecipeSO.maxFryingTime
                    });

                if(fryingTimer > fryingRecipeSO.maxFryingTime)
                {
                    GetKitchenObjectOnTop().DestroySelf();
                    KitchenObject.SpwanKitchenObject(fryingRecipeSO.output,this);

                    fryingRecipeSO = GetOutputForInput(GetKitchenObjectOnTop().GetKitchenObjectSO());

                    state = DeepFryState.Burning;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    
                    fryingTimer = 0f;
                }
            break;

            case DeepFryState.Burning:
                fryingTimer += Time.deltaTime;
                OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs
                    {
                        progressAmount = fryingTimer / fryingRecipeSO.maxFryingTime
                    });
                    
                if(fryingTimer > fryingRecipeSO.maxFryingTime)
                {
                    GetKitchenObjectOnTop().DestroySelf();
                    KitchenObject.SpwanKitchenObject(fryingRecipeSO.output,this);

                    state = DeepFryState.Burned;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                }
            break;

            case DeepFryState.Burned:
            break;

            

        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if(player.HasKitchenObject() && HasValidDeepFryRecipe(player.GetKitchenObjectOnTop().GetKitchenObjectSO()))
            {
                player.GetKitchenObjectOnTop().SetKitchenObjectParent(this);

                fryingTimer = 0f;

                state = DeepFryState.Frying;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                if(state == DeepFryState.Burning)
                {
                    GetKitchenObjectOnTop().DestroySelf();
                    KitchenObject.SpwanKitchenObject(packedFries,player);
                }
                else
                {
                    GetKitchenObjectOnTop().SetKitchenObjectParent(player);
                }
                OnProgressUpdate?.Invoke(this, new IHasProgress.OnProgressUpdateArgs
                {
                    progressAmount = 0f
                });
                
                state = DeepFryState.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
            }
            else
            {
                if(player.GetKitchenObjectOnTop().TryGetPlate(out PlateKitchenObject plate))
                {
                    if(state == DeepFryState.Burning)
                    {
                        GetKitchenObjectOnTop().DestroySelf();
                        KitchenObject.SpwanKitchenObject(packedFries,this);
                    }
                    
                    if (plate.TryAddIngredientToPlate(GetKitchenObjectOnTop().GetKitchenObjectSO()))
                    {
                        // if adding is successfull
                        GetKitchenObjectOnTop().DestroySelf();
                        state = DeepFryState.Idle;
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

    private bool HasValidDeepFryRecipe(KitchenObjectSO input)
    {
        fryingRecipeSO = GetOutputForInput(input);
        return fryingRecipeSO!=null;
    }

    private FryingRecipeSO GetOutputForInput(KitchenObjectSO input)
    {
        foreach(FryingRecipeSO fryingRecipeSO in DeepFryRecipeSOs)
        {
            if(fryingRecipeSO.input == input)
            {
                
                return fryingRecipeSO;
            }
        }

        return null;
    }


    public bool IsBurning()
    {
        return state == DeepFryState.Burning;
    }
}
