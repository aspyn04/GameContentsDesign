using System.Collections.Generic;

/// <summary>
/// �ϳ��� Ÿ��Ʈ ������ ������ ��� ������ Ŭ����.
/// </summary>
[System.Serializable]
public class TartRecipe
{
    public int id;                        // ��: 4011001
    public string name;                   // ��: "�޺� �� Ÿ��Ʈ"
    public List<string> ingredients;      // ��: {"3001001", "3002001", "3002010"}
}
