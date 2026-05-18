using UnityEngine;

public class DeliveryCounter : BaseCounter
{
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
