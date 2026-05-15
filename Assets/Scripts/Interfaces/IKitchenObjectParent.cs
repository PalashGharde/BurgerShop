using UnityEngine;

public interface IKitchenObjectParent
{
    public bool HasKitchenObject();

    public KitchenObject GetKitchenObjectOnTop();

    public void SetKitchenObjectOnTop(KitchenObject kitchenObject);

    public Transform GetKitchenObjectParentTop();

    public void ClearKitchenObject();
}
