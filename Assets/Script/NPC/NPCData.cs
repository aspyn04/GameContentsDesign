using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC ������ ��� ������ Ŭ����.
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
    public int recipeID;               // ������ ID (��: 4011001)
    public string satisfiedDialogue;
    public string unsatisfiedDialogue;
}