using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeSO", menuName = "Scriptable Objects/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<KitchenObjectSO> ingridientKitchenSOList;
}
