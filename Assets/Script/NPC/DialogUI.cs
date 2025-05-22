using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Button dialogNextButton;
    [SerializeField] private Button makeTartButton;
    [SerializeField] private GameObject tartTable;
    [SerializeField] private Image npcImage;


    public void SetNPCImage(string npcID)
    {
        string path = $"Image/NPC/{npcID.Trim()}";
        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite != null)
        {
            npcImage.sprite = sprite;
            npcImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("NPC 이미지 로드 실패: " + path);
            npcImage.gameObject.SetActive(false);
        }
    }

    public IEnumerator Show(string message)
    {
        dialogPanel.SetActive(true);
        dialogText.text = message;

        bool clicked = false;

        dialogNextButton.gameObject.SetActive(true);

        dialogNextButton.onClick.RemoveAllListeners();
        dialogNextButton.onClick.AddListener(() =>
        {
            Debug.Log(" 다음 버튼 클릭됨!");
            clicked = true;
        });

        yield return new WaitUntil(() => clicked);

        dialogNextButton.gameObject.SetActive(false);
    }

    public IEnumerator WaitForMakeTartClick()
    {
        makeTartButton.gameObject.SetActive(true);

        bool clicked = false;

        makeTartButton.onClick.RemoveAllListeners();
        makeTartButton.onClick.AddListener(() =>
        {
            clicked = true;
            makeTartButton.gameObject.SetActive(false);

            if (tartTable != null)
            {
                tartTable.SetActive(true);
                Debug.Log("타르트 제작대 활성화됨");
            }
            else
            {
                Debug.LogError("tartTable이 연결되지 않았습니다.");
            }
        });

        yield return new WaitUntil(() => clicked);
    }
}
