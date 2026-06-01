using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance {get; private set;}

    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(Player player)
    {
        if(player.HasKitchenObject() && player.GetKitchenObjectOnTop().TryGetPlate(out PlateKitchenObject plate))
        {
            // player has a plate in their hand
            DeliveryManager.Instance.DeliverRecipeOnPlate(plate);
            player.GetKitchenObjectOnTop().DestroySelf();

        }
    }
}
