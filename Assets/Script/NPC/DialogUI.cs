using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ���(Dialog) UI�� �����ϴ� Ŭ����.
/// NPC �̹��� ��ȯ�� GetNPCImageObject() �޼��带 �߰��߽��ϴ�.
/// </summary>
/// <summary>
/// ���(Dialog) UI�� ����մϴ�.
/// NPC �̹��� ��ȯ�� GetNPCImageObject() �޼��带 �߰��߽��ϴ�.
/// </summary>
public class DialogUI : MonoBehaviour
{
    [Header("UI ���")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Button dialogNextButton;
    [SerializeField] private Button makeTartButton;
    [SerializeField] private GameObject tartTable;
    [SerializeField] private Image npcImage;

    /// <summary>
    /// NPC ID�� ������� Resources/Images/NPC �������� ��������Ʈ�� �ε��Ͽ� npcImage�� �Ҵ��ϰ� Ȱ��ȭ.
    /// npcImage.gameObject�� ��ȯ�մϴ�.
    /// </summary>
    public GameObject SetNPCImage(string npcID)
    {
        string path = $"Image/NPC/{npcID.Trim()}"; // Resources/Images/NPC/{npcID}.png
        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite != null)
        {
            npcImage.sprite = sprite;
            npcImage.gameObject.SetActive(true);
            return npcImage.gameObject;
        }
        else
        {
            Debug.LogWarning($"DialogUI: NPC �̹��� �ε� ����: {path}");
            npcImage.gameObject.SetActive(false);
            return null;
        }
    }

    /// <summary>
    /// npcImage.gameObject�� ��ȯ�մϴ�. (null ����)
    /// </summary>
    public GameObject GetNPCImageObject()
    {
        return npcImage != null ? npcImage.gameObject : null;
    }

    /// <summary>
    /// ��� �г��� �Ѱ� message�� �����ش�. �������� ��ư Ŭ������ ���.
    /// </summary>
    public IEnumerator Show(string message)
    {
        dialogPanel.SetActive(true);
        dialogText.text = message;

        bool clicked = false;
        dialogNextButton.gameObject.SetActive(true);

        dialogNextButton.onClick.RemoveAllListeners();
        dialogNextButton.onClick.AddListener(() =>
        {
            UISoundManager.Instance?.PlayClick();
            clicked = true;
        });

        yield return new WaitUntil(() => clicked);

        dialogNextButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// ��Ÿ��Ʈ ����⡱ ��ư�� Ȱ��ȭ�ϰ� Ŭ������ ���.
    /// Ŭ�� �� tartTable Ȱ��ȭ.
    /// </summary>
    public IEnumerator WaitForMakeTartClick()
    {
        makeTartButton.gameObject.SetActive(true);

        bool clicked = false;
        makeTartButton.onClick.RemoveAllListeners();
        makeTartButton.onClick.AddListener(() =>
        {
            UISoundManager.Instance?.PlayClick();
            clicked = true;
            makeTartButton.gameObject.SetActive(false);

            if (tartTable != null)
                tartTable.SetActive(true);
            else
                Debug.LogError("DialogUI: tartTable�� ������� �ʾҽ��ϴ�.");
        });

        yield return new WaitUntil(() => clicked);
    }
}