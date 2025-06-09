using System.Collections.Generic;
using UnityEngine;


public class TartManager : MonoBehaviour
{
    [Header("하위 매니저 참조 (각각 GameObject에는 panelObject가 포함되어 있어야 합니다)")]
    [SerializeField] private RecipeManager recipeManager;
    [SerializeField] private TartCrust crustManager;   // TartCrust MonoBehaviour
    [SerializeField] private TartOven ovenManager;     // TartOven MonoBehaviour
    [SerializeField] private TartTopping toppingManager;// TartTopping MonoBehaviour

    private TartRecipe currentRecipe;
    private bool crustSuccess;
    private bool ovenSuccess;
    private bool toppingSuccess;
    private bool productionDone = false;
    public System.Action<bool> OnTartProcessFinished;

    private void Start()
    {
        // 게임 시작 시에는 모든 단계 UI를 비활성화 상태로 세팅
        crustManager.gameObject.SetActive(false);
        ovenManager.gameObject.SetActive(false);
        toppingManager.gameObject.SetActive(false);
        productionDone = false;
    }

    /// 외부(예: NPCManager)에서 호출하여 타르트 제작을 시작합니다.
    public void StartTartMaking(int recipeID)
    {
        currentRecipe = recipeManager.GetRecipeByID(recipeID);
        if (currentRecipe == null)
        {
            Debug.LogError("TartManager: 레시피 ID " + recipeID + "가 존재하지 않습니다.");
            return;
        }

        if (currentRecipe.ingredients.Count == 0)
        {
            Debug.Log("TartManager: ID " + recipeID + "은 망한타르트. 즉시 실패 처리");
            OnToppingComplete(false);
            return;
        }

        // 1) 모든 단계 UI를 일단 비활성화
        crustManager.gameObject.SetActive(false);
        ovenManager.gameObject.SetActive(false);
        toppingManager.gameObject.SetActive(false);

        crustSuccess = false;
        ovenSuccess = false;
        toppingSuccess = false;

        // 2) 재료 단계 시작
        crustManager.gameObject.SetActive(true);
        crustManager.Init(currentRecipe.ingredients, this);

        productionDone = false;
        crustSuccess = ovenSuccess = toppingSuccess = false;
        crustManager.Init(currentRecipe.ingredients, this);
    }

    /// TartCrust 단계가 끝나면 호출됩니다.
    public void OnCrustComplete(bool success)
    {
        crustSuccess = success;
        Debug.Log("TartManager: 재료 단계 결과 = " + (success ? "성공" : "실패"));

        crustManager.gameObject.SetActive(false);

        ovenManager.gameObject.SetActive(true);
        ovenManager.Init(this);
    }

    /// TartOven 단계가 끝나면 호출됩니다.
    public void OnOvenComplete(bool success)
    {
        ovenSuccess = success;
        Debug.Log("TartManager: 오븐 단계 결과 = " + (success ? "성공" : "실패"));

        ovenManager.gameObject.SetActive(false);

        toppingManager.gameObject.SetActive(true);
        toppingManager.Init(currentRecipe.ingredients, this);
    }

    /// TartTopping 단계가 끝나면 호출됩니다.
    public void OnToppingComplete(bool success)
    {
        toppingSuccess = success;
        bool finalSuccess = crustSuccess && ovenSuccess && toppingSuccess;
        Debug.Log("TartManager: 토핑 단계 결과 = " + (success ? "성공" : "실패") + ", 최종 성공 = " + finalSuccess);

        toppingManager.gameObject.SetActive(false);
        productionDone = true;     // 여기서 제작 완료 플래그 올림

        OnTartProcessFinished?.Invoke(finalSuccess);
        
        if (finalSuccess == true)
        {
            GoodsManager.Instance.AddCheese(100);
            GoodsManager.Instance.AddStar(2);
        }
        else
        {
            GoodsManager.Instance.AddCheese(20);
        }
    }
    public GameObject crustPanel;
    public GameObject ovenPanel;
    public GameObject toppingPanel;

    public void HideAllPanels()
    {
        crustPanel?.SetActive(false);
        ovenPanel?.SetActive(false);
        toppingPanel?.SetActive(false);
    }

    public bool IsProductionDone => productionDone;

    /// <summary>
    /// 최종 성공 여부 (모든 단계 성공)
    /// </summary>
    public bool IsTartComplete => currentRecipe != null && crustSuccess && ovenSuccess && toppingSuccess;
    public bool CheckTartResult() => currentRecipe != null && crustSuccess && ovenSuccess && toppingSuccess;
}