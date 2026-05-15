using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;


    public virtual void Interact(Player player)
    {
        Debug.Log("BaseCounter Interact");
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
    }

    public Transform GetKitchenObjectParentTop()
    {
        return counterTopPoint;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
}
