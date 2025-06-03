using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 대사(Dialog) UI를 관리하는 클래스.
/// NPC 이미지 반환용 GetNPCImageObject() 메서드를 추가했습니다.
/// </summary>
/// <summary>
/// 대사(Dialog) UI를 담당합니다.
/// NPC 이미지 반환용 GetNPCImageObject() 메서드를 추가했습니다.
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
    /// NPC ID를 기반으로 Resources/Images/NPC 폴더에서 스프라이트를 로드하여 npcImage에 할당하고 활성화.
    /// npcImage.gameObject를 반환합니다.
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
            Debug.LogWarning($"DialogUI: NPC 이미지 로드 실패: {path}");
            npcImage.gameObject.SetActive(false);
            return null;
        }
    }

    /// <summary>
    /// npcImage.gameObject를 반환합니다. (null 가능)
    /// </summary>
    public GameObject GetNPCImageObject()
    {
        return npcImage != null ? npcImage.gameObject : null;
    }

    /// <summary>
    /// 대사 패널을 켜고 message를 보여준다. “다음” 버튼 클릭까지 대기.
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
    /// “타르트 만들기” 버튼을 활성화하고 클릭까지 대기.
    /// 클릭 시 tartTable 활성화.
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
}