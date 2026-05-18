using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private List<RecipeSO> recipeSOList;

    private void Awake()
    {
        recipeSOList = new List<RecipeSO>();
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderAdded += DeliveryManager_OnOrderAdded;
        DeliveryManager.Instance.OnOrderRemoved += DeliveryManager_OnOrderRemoved;
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderRemoved(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderAdded(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach(Transform child in container)
        {
            if(child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetAllOrdersList())
        {
            Transform recipeUITransform = Instantiate(recipeTemplate, container);
            recipeUITransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeUI(recipeSO);
            recipeUITransform.gameObject.SetActive(true);
        }
    }


}
