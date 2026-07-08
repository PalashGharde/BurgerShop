using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateSO;

    private float plateSpawnedTimerMax = 4f;
    private float plateSpawnedTimer = 0f;

    private int plateSpawnedAmount = 0;
    private int plateSpawnedAmountMax = 4;

    public event EventHandler OnPlateAdded;
    public event EventHandler OnPlateRemoved;


    private void Update()
    {
        if(GameManager.Instance.IsGamePlaying() || GameManager.Instance.IsGameCountDown())
        {
            plateSpawnedTimer += Time.deltaTime;

            if(plateSpawnedTimer > plateSpawnedTimerMax)
            {
                if(plateSpawnedAmount < plateSpawnedAmountMax)
                {
                    plateSpawnedTimer = 0f;
                    plateSpawnedAmount++;
                    OnPlateAdded?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject() && plateSpawnedAmount>0)
        {
            KitchenObject.SpwanKitchenObject(plateSO, player);
            plateSpawnedTimer = 0f;
            plateSpawnedAmount--;
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);

            //face plate in players forward direction
            // Vector3 forward = player.transform.forward;
            // forward.y = 0f;
            // forward.Normalize();
            // Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
            // player.GetKitchenObjectOnTop().transform.rotation = rotation;
        }
    }
}
