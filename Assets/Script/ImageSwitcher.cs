using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Image imageSlot;
        public List<Sprite> sprites;
        [HideInInspector] public int currentIndex = 0;

        public int Count => sprites.Count;
        public Sprite CurrentSprite => Count > 0 ? sprites[Mathf.Min(currentIndex, Count - 1)] : null;

        public void MoveNext(int globalMaxIndex)
        {
            if (Count == 0) return;

            if (currentIndex < globalMaxIndex)
            {
                currentIndex++;
            }
            else if (Count < globalMaxIndex + 1)
            {
                currentIndex = 0; // loop for shorter slots
            }
        }
    }

    public List<Slot> slots;
    public Button nextButton;           // 메인 버튼 (처음에 눌러서 다음 이미지로 넘기는 버튼)
    public Button sceneChangeButton;   // 마지막에 등장할 버튼 (씬 전환용)
    public string nextSceneName;       // 전환할 씬 이름

    private int globalMaxIndex = 0;

    void Start()
    {
        // 최댓값 계산
        foreach (var slot in slots)
        {
            if (slot.Count - 1 > globalMaxIndex)
                globalMaxIndex = slot.Count - 1;
        }

        UpdateAllImages();
        sceneChangeButton.gameObject.SetActive(false); // 전환 버튼 비활성화
    }

    public void OnNextButton()
    {
        // 다음 이미지로
        foreach (var slot in slots)
        {
            slot.MoveNext(globalMaxIndex);
        }

        UpdateAllImages();

        // 마지막 인덱스에 도달했는지 확인
        bool allReachedEnd = true;
        foreach (var slot in slots)
        {
            if (slot.currentIndex < globalMaxIndex)
            {
                allReachedEnd = false;
                break;
            }
        }

        if (allReachedEnd)
        {
            nextButton.gameObject.SetActive(false);           // 기존 버튼 숨김
            sceneChangeButton.gameObject.SetActive(true);     // 새로운 버튼 표시
        }
    }

    public void OnChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("다음 씬 이름이 비어있습니다.");
        }
    }

    void UpdateAllImages()
    {
        foreach (var slot in slots)
        {
            if (slot.Count > 0)
                slot.imageSlot.sprite = slot.CurrentSprite;
        }
    }
}
