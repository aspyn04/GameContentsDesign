using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// <summary>
/// Resources/CSV/TartRecipes.csv �� �о� �鿩,
/// ������ ID �� TartRecipe ������ Dictionary�� �����մϴ�.
/// </summary>
public class RecipeManager : MonoBehaviour
{
    [Header("Resources/CSV ������ TartRecipes.csv ������ �μ���.")]
    [SerializeField] private TextAsset recipeCsv;

    private Dictionary<int, TartRecipe> recipeDict = new Dictionary<int, TartRecipe>();

    private void Awake()
    {
        if (recipeCsv == null)
        {
            Debug.LogError("RecipeManager: recipeCsv�� �Ҵ���� ����");
            return;
        }
        LoadRecipesFromCSV();
    }

    private void LoadRecipesFromCSV()
    {
        string[] lines = recipeCsv.text.Split('\n');
        if (lines.Length <= 1) return;

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            // ������ ��ǥ�� �и� (��� �� ������ �÷� ���ٰ� ����)
            string[] cols = line.Split(',');
            if (cols.Length < 5) continue;

            if (!int.TryParse(cols[0].Trim(), out int id))
                continue;

            TartRecipe recipe = new TartRecipe
            {
                id = id,
                name = cols[1].Trim(),
                ingredients = new List<string>()
            };

            // 3~4��° �÷��� ��� ID�� ��������� �߰�
            for (int j = 2; j < 5 && j < cols.Length; j++)
            {
                string ing = cols[j].Trim();
                if (!string.IsNullOrEmpty(ing))
                    recipe.ingredients.Add(ing);
            }

            recipeDict[id] = recipe;
        }

        Debug.Log($"RecipeManager: {recipeDict.Count}���� ������ �ε� �Ϸ�");
    }

    /// <summary>
    /// �־��� ID�� �ش��ϴ� �����ǰ� ������ ��ȯ, ������ null
    /// </summary>
    public TartRecipe GetRecipeByID(int id)
    {
        recipeDict.TryGetValue(id, out TartRecipe recipe);
        return recipe;
    }
}

/// <summary>
/// CSV �� �� ������ ��� Ŭ����
/// </summary>
