using System.Collections.Generic;
using UnityEngine;


public class TartManager : MonoBehaviour
{
    [Header("���� �Ŵ��� ���� (���� GameObject���� panelObject�� ���ԵǾ� �־�� �մϴ�)")]
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
        // ���� ���� �ÿ��� ��� �ܰ� UI�� ��Ȱ��ȭ ���·� ����
        crustManager.gameObject.SetActive(false);
        ovenManager.gameObject.SetActive(false);
        toppingManager.gameObject.SetActive(false);
        productionDone = false;
    }

    /// �ܺ�(��: NPCManager)���� ȣ���Ͽ� Ÿ��Ʈ ������ �����մϴ�.
    public void StartTartMaking(int recipeID)
    {
        currentRecipe = recipeManager.GetRecipeByID(recipeID);
        if (currentRecipe == null)
        {
            Debug.LogError("TartManager: ������ ID " + recipeID + "�� �������� �ʽ��ϴ�.");
            return;
        }

        if (currentRecipe.ingredients.Count == 0)
        {
            Debug.Log("TartManager: ID " + recipeID + "�� ����Ÿ��Ʈ. ��� ���� ó��");
            OnToppingComplete(false);
            return;
        }

        // 1) ��� �ܰ� UI�� �ϴ� ��Ȱ��ȭ
        crustManager.gameObject.SetActive(false);
        ovenManager.gameObject.SetActive(false);
        toppingManager.gameObject.SetActive(false);

        crustSuccess = false;
        ovenSuccess = false;
        toppingSuccess = false;

        // 2) ��� �ܰ� ����
        crustManager.gameObject.SetActive(true);
        crustManager.Init(currentRecipe.ingredients, this);

        productionDone = false;
        crustSuccess = ovenSuccess = toppingSuccess = false;
        crustManager.Init(currentRecipe.ingredients, this);
    }

    /// TartCrust �ܰ谡 ������ ȣ��˴ϴ�.
    public void OnCrustComplete(bool success)
    {
        crustSuccess = success;
        Debug.Log("TartManager: ��� �ܰ� ��� = " + (success ? "����" : "����"));

        crustManager.gameObject.SetActive(false);

        ovenManager.gameObject.SetActive(true);
        ovenManager.Init(this);
    }

    /// TartOven �ܰ谡 ������ ȣ��˴ϴ�.
    public void OnOvenComplete(bool success)
    {
        ovenSuccess = success;
        Debug.Log("TartManager: ���� �ܰ� ��� = " + (success ? "����" : "����"));

        ovenManager.gameObject.SetActive(false);

        toppingManager.gameObject.SetActive(true);
        toppingManager.Init(currentRecipe.ingredients, this);
    }

    /// TartTopping �ܰ谡 ������ ȣ��˴ϴ�.
    public void OnToppingComplete(bool success)
    {
        toppingSuccess = success;
        bool finalSuccess = crustSuccess && ovenSuccess && toppingSuccess;
        Debug.Log("TartManager: ���� �ܰ� ��� = " + (success ? "����" : "����") + ", ���� ���� = " + finalSuccess);

        toppingManager.gameObject.SetActive(false);
        productionDone = true;     // ���⼭ ���� �Ϸ� �÷��� �ø�

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
    /// ���� ���� ���� (��� �ܰ� ����)
    /// </summary>
    public bool IsTartComplete => currentRecipe != null && crustSuccess && ovenSuccess && toppingSuccess;
    public bool CheckTartResult() => currentRecipe != null && crustSuccess && ovenSuccess && toppingSuccess;
}