using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ÿ��Ʈ ���� ��ü �帧�� �����մϴ�.
/// RecipeManager �� TartCrustManager �� TartTopping ������� ȣ���ϰ�,
/// ���� ���� ���θ� �ܺο��� Ȯ���� �� �ֵ��� IsTartComplete, CheckTartResult �޼��带 �����մϴ�.
/// </summary>
/// <summary>
/// Ÿ��Ʈ ���� ��ü �帧�� �����մϴ�.
/// RecipeManager �� TartCrustManager �� TartTopping ������� ȣ���ϰ�,
/// ���� ���� ���θ� �ܺο��� Ȯ���� �� �ֵ��� IsTartComplete, CheckTartResult �޼��带 �����մϴ�.
/// </summary>
public class TartManager : MonoBehaviour
{
    [Header("���� �Ŵ��� ����")]
    [SerializeField] private RecipeManager recipeManager;
    [SerializeField] private TartCrust crustManager;
    [SerializeField] private TartTopping toppingManager;

    // ���� ���� ���� ������ ���� (RecipeManager.TartRecipe)
    private TartRecipe currentRecipe;

    // �ܰ躰 ����/���� �÷���
    private bool crustSuccess;
    private bool toppingSuccess;

    /// <summary>
    /// �ܺο��� ȣ���Ͽ� Ÿ��Ʈ ���� ������ ����.
    /// recipeID: RecipeManager�� ��ϵ� ID (��: 4011001) �Ǵ� ���п� ID(4001010).
    /// </summary>
    public void StartTartMaking(int recipeID)
    {
        currentRecipe = recipeManager.GetRecipeByID(recipeID);
        if (currentRecipe == null)
        {
            Debug.LogError($"TartManager: ������ ID {recipeID}�� �������� �ʽ��ϴ�.");
            return;
        }

        // ���� ingredients�� ���������(����Ÿ��Ʈ) �ٷ� ���� ó��
        if (currentRecipe.ingredients.Count == 0)
        {
            Debug.Log($"TartManager: ID {recipeID}�� ����Ÿ��Ʈ. �ٷ� ���� ó��");
            OnToppingComplete(false);
            return;
        }

        // 1�ܰ�: ��� ���� �ܰ�� ����
        crustSuccess = false;
        toppingSuccess = false;
        crustManager.Init(currentRecipe.ingredients, this);
    }

    /// <summary>
    /// TartCrustManager�� ��� �ܰ� �Ϸ� �� ȣ��.
    /// </summary>
    public void OnCrustComplete(bool success)
    {
        crustSuccess = success;
        Debug.Log($"TartManager: ��� �ܰ� ��� = {(success ? "����" : "����")}");

        // 2�ܰ�: ���� Ȯ�� �ܰ�� ����
        toppingManager.Init(currentRecipe.ingredients, crustSuccess, this);
    }

    /// <summary>
    /// TartTopping�� ���� �Ϸ� �� ȣ��.
    /// </summary>
    public void OnToppingComplete(bool success)
    {
        toppingSuccess = success;
        bool finalSuccess = (crustSuccess && toppingSuccess);
        Debug.Log($"TartManager: ���� �ܰ� ��� = {(success ? "����" : "����")}, ���� ���� = {finalSuccess}");
    }

    /// <summary>
    /// ���+���� �ܰ踦 ��� �����ߴ��� ����.
    /// </summary>
    public bool IsTartComplete => currentRecipe != null && crustSuccess && toppingSuccess;

    /// <summary>
    /// ���� ���� ���� ��ȯ.
    /// </summary>
    public bool CheckTartResult() => currentRecipe != null && crustSuccess && toppingSuccess;
}