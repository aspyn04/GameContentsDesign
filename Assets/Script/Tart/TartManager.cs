using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TartManager : MonoBehaviour
{
    // public RecipeManager recipeManager; // 레시피 비활성화
    // private TartRecipe currentRecipe;

    private bool crustSuccess, ovenSuccess, toppingSuccess;
    public bool IsTartComplete => crustSuccess && ovenSuccess && toppingSuccess;

    private void Start()
    {
        // 임시로 바로 시작
        Debug.Log("TartManager 테스트 시작");

        List<string> testCrustOrder = new List<string> { "a", "b", "c", "d", "e", "f" };

        // TartCrustManager 실행
        var crustManager = FindObjectOfType<TartCrustManager>();
        if (crustManager != null)
        {
            crustManager.Init(testCrustOrder);
        }
        else
        {
            Debug.LogError("TartCrustManager not found in scene");
        }
    }

    // 기존 NPCData 기반 방식 비활성화
    /*
    public void StartTartMaking(NPCData guest)
    {
        int code = recipeManager.GetTartCodeByName(guest.orderedTart);
        currentRecipe = recipeManager.GetRecipeByCode(code);

        crustSuccess = ovenSuccess = toppingSuccess = false;

        FindObjectOfType<TartCrustManager>().Init(currentRecipe.crustOrder, this);
    }

    */
    // 테스트할 때는 주석 처리했지만, 에러 없이 빌드하려면 메서드는 남겨야 합니다.
    public void StartTartMaking(NPCData npc)
    {
        Debug.LogWarning("StartTartMaking()은 현재 테스트용으로 비활성화되어 있습니다.");
    }


    public void SetCrustResult(bool success)
    {
        crustSuccess = success;
        Debug.Log("크러스트 성공 여부: " + success);

        // 다음 단계 오븐 호출 (임시 주석 처리)
        // FindObjectOfType<TartOven>().StartOven(currentRecipe.ovenSetting, this);
    }

    public void SetOvenResult(bool success)
    {
        ovenSuccess = success;
        Debug.Log("오븐 성공 여부: " + success);

        // 다음 단계 토핑 호출 (임시 주석 처리)
        // FindObjectOfType<TartTopping>().StartTopping(currentRecipe.toppings, this);
    }

    public void SetToppingResult(bool success)
    {
        toppingSuccess = success;
        Debug.Log("토핑 성공 여부: " + success);
    }

    public bool CheckTartResult()
    {
        return crustSuccess && ovenSuccess && toppingSuccess;
    }
}
