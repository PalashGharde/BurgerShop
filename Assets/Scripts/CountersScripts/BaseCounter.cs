using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;


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
