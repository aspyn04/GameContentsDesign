using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 대사(Dialog) UI를 관리합니다.
/// NPC 이미지를 세팅하고, 대사/타르트 제작 버튼 흐름을 제공합니다.
/// </summary>
public class DialogUI : MonoBehaviour
{
    [Header("UI 요소")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Button dialogNextButton;
    [SerializeField] private Button makeTartButton;
    [SerializeField] private GameObject tartTable;
    [SerializeField] private Image npcImage;

    /// <summary>
    /// NPC ID에 대응하는 스프라이트를 로드해서 보여주고, 이미지 오브젝트를 반환합니다.
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
            Debug.LogWarning($"DialogUI: NPC 이미지 로드 실패: {path}");
            npcImage.gameObject.SetActive(false);
            return null;
        }
    }

    /// <summary>
    /// NPC 이미지 오브젝트를 반환합니다(없으면 null).
    /// </summary>
    public GameObject GetNPCImageObject()
    {
        return npcImage != null ? npcImage.gameObject : null;
    }

    /// <summary>
    /// 대사 패널을 켜고 메시지를 띄운 뒤, “다음” 버튼 클릭을 대기합니다.
    /// 클릭하면 버튼만 숨기고 반환합니다.
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
    /// “타르트 만들기” 버튼을 켜고 클릭을 대기합니다.
    /// 클릭하면 버튼은 숨기고, tartTable을 보여줍니다.
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
                Debug.LogError("DialogUI: tartTable이 연결되지 않았습니다.");
        });

        yield return new WaitUntil(() => clicked);
    }

    /// <summary>
    /// NPC 이미지를 숨깁니다.
    /// </summary>
    public void HideNPCImage()
    {
        if (npcImage != null)
            npcImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 대사 패널을 숨깁니다.
    /// </summary>
    public void HideDialogPanel()
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }

    /// <summary>
    /// 토스트 제작 테이블 UI를 숨깁니다.
    /// </summary>
    public void HideTartTable()
    {
        if (tartTable != null)
            tartTable.SetActive(false);
    }
}