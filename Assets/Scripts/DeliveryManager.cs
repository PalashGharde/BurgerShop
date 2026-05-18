using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance {get; private set;}
    [SerializeField] private RecipesSO allRecipesList;
    private List<RecipeSO> allOrdersList;

    public event EventHandler OnOrderAdded;
    public event EventHandler OnOrderRemoved;

    private float waitTimer = 0f;
    private float waitTimerMax = 4f;

    private int ordersInWaiting = 0;
    private int ordersInWaitingMax = 4;

    private void Awake()
    {
        Instance = this;
        allOrdersList = new List<RecipeSO>();
    }

    private void Update()
    {
        waitTimer += Time.deltaTime;
        if(waitTimer >= waitTimerMax)
        {
            waitTimer = 0f;
            if(ordersInWaiting < ordersInWaitingMax)
            {
                ordersInWaiting++;
                RecipeSO newOrder = allRecipesList.listOfAllRecipes[UnityEngine.Random.Range(0, allRecipesList.listOfAllRecipes.Count)];
                allOrdersList.Add(newOrder);
                OnOrderAdded?.Invoke(this, EventArgs.Empty);
                
            }
        }
    }

    public void DeliverRecipeOnPlate(PlateKitchenObject plateKitchenObject)
    {
        for(int i=0; i<allOrdersList.Count; i++)
        {
            RecipeSO recipeSO = allOrdersList[i];   // check each recipe
            
            if(recipeSO.ingridientKitchenSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // count of recipe's ingredient and plate's ingredient are same
                bool recipeMatched = true;
                // now check if plate matches each ingridients of recipe
                foreach(KitchenObjectSO recipekitchenObjectSO in recipeSO.ingridientKitchenSOList)
                {
                    bool ingridientFound = false;
                    foreach(KitchenObjectSO platekitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if(recipekitchenObjectSO == platekitchenObjectSO)
                        {
                            // ingridient found on plate
                            ingridientFound = true;
                            break;                      
                            // check next ingridient
                        }
                    }

                    if (!ingridientFound)   // ingridient not fouind on plate, maybe try next recipe
                    {
                        recipeMatched = false;
                    }
                }

                if (recipeMatched)
                { 
                    // Recipe matched
                    allOrdersList.Remove(recipeSO);
                    ordersInWaiting--;
                    OnOrderRemoved?.Invoke(this, EventArgs.Empty);
                    waitTimer = 0f;
                    return;
                }
                
            }
        }

    }

    public List<RecipeSO> GetAllOrdersList()
    {
        return allOrdersList;
    }
}
