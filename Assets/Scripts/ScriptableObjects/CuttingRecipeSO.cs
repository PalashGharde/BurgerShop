using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipeSO", menuName = "Scriptable Objects/CuttingRecipeSO")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO cuttingRecipeInput;
    public KitchenObjectSO cuttingRecipeOutput;

    public int maxCutCount;
}
