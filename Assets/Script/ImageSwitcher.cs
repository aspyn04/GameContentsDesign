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
    public Button nextButton;           // ���� ��ư (ó���� ������ ���� �̹����� �ѱ�� ��ư)
    public Button sceneChangeButton;   // �������� ������ ��ư (�� ��ȯ��)
    public string nextSceneName;       // ��ȯ�� �� �̸�

    private int globalMaxIndex = 0;

    void Start()
    {
        // �ִ� ���
        foreach (var slot in slots)
        {
            if (slot.Count - 1 > globalMaxIndex)
                globalMaxIndex = slot.Count - 1;
        }

        UpdateAllImages();
        sceneChangeButton.gameObject.SetActive(false); // ��ȯ ��ư ��Ȱ��ȭ
    }

    public void OnNextButton()
    {
        // ���� �̹�����
        foreach (var slot in slots)
        {
            slot.MoveNext(globalMaxIndex);
        }

        UpdateAllImages();

        // ������ �ε����� �����ߴ��� Ȯ��
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
            nextButton.gameObject.SetActive(false);           // ���� ��ư ����
            sceneChangeButton.gameObject.SetActive(true);     // ���ο� ��ư ǥ��
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
            Debug.LogWarning("���� �� �̸��� ����ֽ��ϴ�.");
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
