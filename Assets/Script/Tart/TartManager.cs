using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartManager : MonoBehaviour
{
    public RecipeManager recipeManager;
    private TartRecipe currentRecipe;

    private bool crustSuccess, ovenSuccess, toppingSuccess;
    public bool IsTartComplete => crustSuccess && ovenSuccess && toppingSuccess;

    public void StartTartMaking(NPCData guest)
    {
        int code = recipeManager.GetTartCodeByName(guest.orderedTart);
        currentRecipe = recipeManager.GetRecipeByCode(code);

        crustSuccess = ovenSuccess = toppingSuccess = false;

        FindObjectOfType<TartCrust>().StartCrust(currentRecipe.crustOrder, this);
    }

    public void SetCrustResult(bool success)
    {
        crustSuccess = success;
        FindObjectOfType<TartOven>().StartOven(currentRecipe.ovenSetting, this);
    }

    public void SetOvenResult(bool success)
    {
        ovenSuccess = success;
        FindObjectOfType<TartTopping>().StartTopping(currentRecipe.toppings, this);
    }

    public void SetToppingResult(bool success)
    {
        toppingSuccess = success;
    }

    public bool CheckTartResult()
    {
        return crustSuccess && ovenSuccess && toppingSuccess;
    }
}