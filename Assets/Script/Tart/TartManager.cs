using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타르트 제작 전체 흐름을 관리합니다.
/// RecipeManager → TartCrustManager → TartTopping 순서대로 호출하고,
/// 최종 성공 여부를 외부에서 확인할 수 있도록 IsTartComplete, CheckTartResult 메서드를 제공합니다.
/// </summary>
/// <summary>
/// 타르트 제작 전체 흐름을 관리합니다.
/// RecipeManager → TartCrustManager → TartTopping 순서대로 호출하고,
/// 최종 성공 여부를 외부에서 확인할 수 있도록 IsTartComplete, CheckTartResult 메서드를 제공합니다.
/// </summary>
public class TartManager : MonoBehaviour
{
    [Header("하위 매니저 참조")]
    [SerializeField] private RecipeManager recipeManager;
    [SerializeField] private TartCrust crustManager;
    [SerializeField] private TartTopping toppingManager;

    // 현재 진행 중인 레시피 정보 (RecipeManager.TartRecipe)
    private TartRecipe currentRecipe;

    // 단계별 성공/실패 플래그
    private bool crustSuccess;
    private bool toppingSuccess;

    /// <summary>
    /// 외부에서 호출하여 타르트 제작 과정을 시작.
    /// recipeID: RecipeManager에 등록된 ID (예: 4011001) 또는 실패용 ID(4001010).
    /// </summary>
    public void StartTartMaking(int recipeID)
    {
        currentRecipe = recipeManager.GetRecipeByID(recipeID);
        if (currentRecipe == null)
        {
            Debug.LogError($"TartManager: 레시피 ID {recipeID}가 존재하지 않습니다.");
            return;
        }

        // 만약 ingredients가 비어있으면(망한타르트) 바로 실패 처리
        if (currentRecipe.ingredients.Count == 0)
        {
            Debug.Log($"TartManager: ID {recipeID}은 망한타르트. 바로 실패 처리");
            OnToppingComplete(false);
            return;
        }

        // 1단계: 재료 선택 단계로 진입
        crustSuccess = false;
        toppingSuccess = false;
        crustManager.Init(currentRecipe.ingredients, this);
    }

    /// <summary>
    /// TartCrustManager가 재료 단계 완료 후 호출.
    /// </summary>
    public void OnCrustComplete(bool success)
    {
        crustSuccess = success;
        Debug.Log($"TartManager: 재료 단계 결과 = {(success ? "성공" : "실패")}");

        // 2단계: 토핑 확인 단계로 진입
        toppingManager.Init(currentRecipe.ingredients, crustSuccess, this);
    }

    /// <summary>
    /// TartTopping이 토핑 완료 후 호출.
    /// </summary>
    public void OnToppingComplete(bool success)
    {
        toppingSuccess = success;
        bool finalSuccess = (crustSuccess && toppingSuccess);
        Debug.Log($"TartManager: 토핑 단계 결과 = {(success ? "성공" : "실패")}, 최종 성공 = {finalSuccess}");
    }

    /// <summary>
    /// 재료+토핑 단계를 모두 성공했는지 여부.
    /// </summary>
    public bool IsTartComplete => currentRecipe != null && crustSuccess && toppingSuccess;

    /// <summary>
    /// 최종 성공 여부 반환.
    /// </summary>
    public bool CheckTartResult() => currentRecipe != null && crustSuccess && toppingSuccess;
}