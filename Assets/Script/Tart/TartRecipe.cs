using System.Collections.Generic;

/// <summary>
/// 하나의 타르트 레시피 정보를 담는 데이터 클래스.
/// </summary>
[System.Serializable]
public class TartRecipe
{
    public int id;                        // 예: 4011001
    public string name;                   // 예: "달빛 펄 타르트"
    public List<string> ingredients;      // 예: {"3001001", "3002001", "3002010"}
}
