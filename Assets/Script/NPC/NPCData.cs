using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC 정보를 담는 데이터 클래스.
/// </summary>
[System.Serializable]
public class NPCData
{
    public string npcID;
    public string npcName;
    public string type;
    public string species;
    public string greetingDialogue;
    public string orderDialogue;
    public int recipeID;               // 레시피 ID (예: 4011001)
    public string satisfiedDialogue;
    public string unsatisfiedDialogue;
}