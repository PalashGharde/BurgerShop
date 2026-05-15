using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // Clearing the previous Counter
        if(this.kitchenObjectParent != null){
            this.kitchenObjectParent.ClearKitchenObject();
        }

        // Setting new Counter and placing the object on top of counter
        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("KitchenObjectParent already has an item");
        }

        kitchenObjectParent.SetKitchenObjectOnTop(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectParentTop();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
}
