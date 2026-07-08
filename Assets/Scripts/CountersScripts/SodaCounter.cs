using System;
using UnityEngine;


public class SodaCounter : BaseCounter
{

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
            
            SodaMachineUI.Instance.SodaCounter_OnSodaInteract();
        }
    }


    private void OnDestroy()
    {
        SodaMachineUI.OnFinalSodaMade -= SodaMachineUI_OnFinalSodaMade;
    }



}
