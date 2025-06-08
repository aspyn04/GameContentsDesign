using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ���(Dialog) UI�� �����մϴ�.
/// NPC �̹����� �����ϰ�, ���/Ÿ��Ʈ ���� ��ư �帧�� �����մϴ�.
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
    /// NPC ID�� �����ϴ� ��������Ʈ�� �ε��ؼ� �����ְ�, �̹��� ������Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    public GameObject SetNPCImage(string npcID)
    {
        string path = $"Image/NPC/{npcID.Trim()}";
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
    /// NPC �̹��� ������Ʈ�� ��ȯ�մϴ�(������ null).
    /// </summary>
    public GameObject GetNPCImageObject()
    {
        return npcImage != null ? npcImage.gameObject : null;
    }

    /// <summary>
    /// ��� �г��� �Ѱ� �޽����� ��� ��, �������� ��ư Ŭ���� ����մϴ�.
    /// Ŭ���ϸ� ��ư�� ����� ��ȯ�մϴ�.
    /// </summary>
    public IEnumerator Show(string message)
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(true);

        dialogText.text = message;
        dialogNextButton.gameObject.SetActive(true);

        bool clicked = false;
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
    /// ��Ÿ��Ʈ ����⡱ ��ư�� �Ѱ� Ŭ���� ����մϴ�.
    /// Ŭ���ϸ� ��ư�� �����, tartTable�� �����ݴϴ�.
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

    /// <summary>
    /// NPC �̹����� ����ϴ�.
    /// </summary>
    public void HideNPCImage()
    {
        if (npcImage != null)
            npcImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// ��� �г��� ����ϴ�.
    /// </summary>
    public void HideDialogPanel()
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }

    /// <summary>
    /// �佺Ʈ ���� ���̺� UI�� ����ϴ�.
    /// </summary>
    public void HideTartTable()
    {
        if (tartTable != null)
            tartTable.SetActive(false);
    }
}