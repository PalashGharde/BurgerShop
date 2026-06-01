using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaced;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public static void ResetStaticData()
    {
        OnAnyObjectPlaced = null;
    } 

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter Interact");
    }

    public virtual void AltInteract()
    {
        //Debug.Log("BaseCounter AltInteract");
    }



    
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public KitchenObject GetKitchenObjectOnTop()
    {
        return kitchenObject;
    }

    public void SetKitchenObjectOnTop(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
    }

    public Transform GetKitchenObjectParentTop()
    {
        return counterTopPoint;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }
}
