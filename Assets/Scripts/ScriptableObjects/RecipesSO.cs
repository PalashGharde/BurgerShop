using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipesSO", menuName = "Scriptable Objects/RecipesSO")]
public class RecipesSO : ScriptableObject
{
    public List<RecipeSO> listOfAllRecipes;
}
