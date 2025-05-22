using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TartManager : MonoBehaviour
{
    // public RecipeManager recipeManager; // ������ ��Ȱ��ȭ
    // private TartRecipe currentRecipe;

    private bool crustSuccess, ovenSuccess, toppingSuccess;
    public bool IsTartComplete => crustSuccess && ovenSuccess && toppingSuccess;

    private void Start()
    {
        // �ӽ÷� �ٷ� ����
        Debug.Log("TartManager �׽�Ʈ ����");

        List<string> testCrustOrder = new List<string> { "a", "b", "c", "d", "e", "f" };

        // TartCrustManager ����
        var crustManager = FindObjectOfType<TartCrustManager>();
        if (crustManager != null)
        {
            crustManager.Init(testCrustOrder);
        }
        else
        {
            Debug.LogError("TartCrustManager not found in scene");
        }
    }

    // ���� NPCData ��� ��� ��Ȱ��ȭ
    /*
    public void StartTartMaking(NPCData guest)
    {
        int code = recipeManager.GetTartCodeByName(guest.orderedTart);
        currentRecipe = recipeManager.GetRecipeByCode(code);

        crustSuccess = ovenSuccess = toppingSuccess = false;

        FindObjectOfType<TartCrustManager>().Init(currentRecipe.crustOrder, this);
    }

    */
    // �׽�Ʈ�� ���� �ּ� ó��������, ���� ���� �����Ϸ��� �޼���� ���ܾ� �մϴ�.
    public void StartTartMaking(NPCData npc)
    {
        Debug.LogWarning("StartTartMaking()�� ���� �׽�Ʈ������ ��Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
    }


    public void SetCrustResult(bool success)
    {
        crustSuccess = success;
        Debug.Log("ũ����Ʈ ���� ����: " + success);

        // ���� �ܰ� ���� ȣ�� (�ӽ� �ּ� ó��)
        // FindObjectOfType<TartOven>().StartOven(currentRecipe.ovenSetting, this);
    }

    public void SetOvenResult(bool success)
    {
        ovenSuccess = success;
        Debug.Log("���� ���� ����: " + success);

        // ���� �ܰ� ���� ȣ�� (�ӽ� �ּ� ó��)
        // FindObjectOfType<TartTopping>().StartTopping(currentRecipe.toppings, this);
    }

    public void SetToppingResult(bool success)
    {
        toppingSuccess = success;
        Debug.Log("���� ���� ����: " + success);
    }

    public bool CheckTartResult()
    {
        return crustSuccess && ovenSuccess && toppingSuccess;
    }
}
