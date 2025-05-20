using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public NPCDataManager npcDataManager;
    public GameObject tartCrust; // Ÿ��Ʈ ���� ���� ������Ʈ
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Button nextButton;

    private NPCData currentGuest;
    private int dialogueStep = 0;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        tartCrust.SetActive(false);
    }

    public void StartGuestSequence()
    {
        // 1. ���� �մ� ����
        if (npcDataManager.NPCDataList.Count == 0) return;
        currentGuest = npcDataManager.NPCDataList[Random.Range(0, npcDataManager.NPCDataList.Count)];

        dialogueStep = 0;
        ShowDialogue();
    }

    private void ShowDialogue()
    {
        dialoguePanel.SetActive(true);

        switch (dialogueStep)
        {
            case 0:
                dialogueText.text = currentGuest.greetingDialogue;
                break;
            case 1:
                dialogueText.text = currentGuest.orderDialogue;
                break;
            case 2:
                dialoguePanel.SetActive(false);
                tartCrust.SetActive(true); // Ÿ��Ʈ ���� ����
                break;
        }
    }

    public void OnNextButtonPressed()
    {
        dialogueStep++;
        ShowDialogue();
    }
}

