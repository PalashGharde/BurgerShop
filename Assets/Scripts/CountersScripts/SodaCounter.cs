using System;


public class SodaCounter : BaseCounter
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public event EventHandler OnSodaInteract;

    public static event EventHandler OnMiniGameStarted;

    

    private void Start()
    {
        SodaMachineUI.OnFinalSodaMade +=  SodaMachineUI_OnFinalSodaMade;
    }

    private void SodaMachineUI_OnFinalSodaMade(object sender, SodaMachineUI.OnMiniGameArgs e)
    {
        KitchenObject.SpwanKitchenObject(e.drink, Player.Instance);
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            OnSodaInteract?.Invoke(this, EventArgs.Empty);
            OnMiniGameStarted?.Invoke(this,EventArgs.Empty);
            
        }
    }
    // public void GiveFinalDrinkToPlayer(KitchenObjectSO drink)
    // {
    //     KitchenObject.SpwanKitchenObject(drink, Player.Instance);
        
    // }


}
