using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// <summary>
/// Resources/CSV/TartRecipes.csv 를 읽어 들여,
/// 레시피 ID → TartRecipe 정보를 Dictionary에 저장합니다.
/// </summary>
public class RecipeManager : MonoBehaviour
{
    [Header("Resources/CSV 폴더에 TartRecipes.csv 파일을 두세요.")]
    [SerializeField] private TextAsset recipeCsv;

    private Dictionary<int, TartRecipe> recipeDict = new Dictionary<int, TartRecipe>();

    private void Awake()
    {
        if (recipeCsv == null)
        {
            Debug.LogError("RecipeManager: recipeCsv가 할당되지 않음");
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

            // 간단히 쉼표로 분리 (대사 등 복잡한 컬럼 없다고 가정)
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

            // 3~4번째 컬럼에 재료 ID가 들어있으면 추가
            for (int j = 2; j < 5 && j < cols.Length; j++)
            {
                string ing = cols[j].Trim();
                if (!string.IsNullOrEmpty(ing))
                    recipe.ingredients.Add(ing);
            }

            recipeDict[id] = recipe;
        }

        Debug.Log($"RecipeManager: {recipeDict.Count}개의 레시피 로드 완료");
    }

    /// <summary>
    /// 주어진 ID에 해당하는 레시피가 있으면 반환, 없으면 null
    /// </summary>
    public TartRecipe GetRecipeByID(int id)
    {
        recipeDict.TryGetValue(id, out TartRecipe recipe);
        return recipe;
    }
}

/// <summary>
/// CSV 한 줄 정보를 담는 클래스
/// </summary>
