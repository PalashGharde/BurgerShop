using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validkitchenObjectSOList;

    
    private List<KitchenObjectSO> kitchenObjectSOList;
    private const string DRINK_CUP = "DrinkCup";
    private bool hasDrink= false;
    
    


    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredientToPlate(KitchenObjectSO kitchenObjectSO)
    {
        if (!validkitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }

        if(kitchenObjectSO.objectName == DRINK_CUP)
        {
            if (hasDrink)
            {
                return false;
            }

            hasDrink = true;
        }

        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
        
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }


}
