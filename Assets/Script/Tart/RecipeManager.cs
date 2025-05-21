using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TartRecipe
{
    public int tartCode;
    public List<string> crustOrder;
    public string ovenSetting;
    public List<string> toppings;
}

[CreateAssetMenu(fileName = "RecipeManager", menuName = "Tart/RecipeManager")]
public class RecipeManager : MonoBehaviour
{
    public List<TartRecipe> recipes;

    public TartRecipe GetRecipeByCode(int code)
    {
        return recipes.Find(r => r.tartCode == code);
    }

    public int GetTartCodeByName(string name)
    {
        switch (name)
        {
            case "����": return 1;
            case "����": return 2;
            default: return 0;
        }
    }
}

