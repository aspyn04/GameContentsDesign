using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static StoryDialogueManager;

[System.Serializable]
public class DayImageGroup
{
    public int day;
    public List<Sprite> sprites;
}

public class StoryPanelUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelObject;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public Button nextButton;
    public Image mainImage;

    [Header("������ �̹��� ����")]
    public List<DayImageGroup> dayImageGroups;

    private bool waitingForClick = false;
    private Dictionary<int, List<Sprite>> imageDict = new();

    void Awake()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(() => waitingForClick = false);

        if (panelObject != null)
            panelObject.SetActive(false);

        foreach (var group in dayImageGroups)
        {
            imageDict[group.day] = group.sprites;
        }
    }

    public IEnumerator PlayStoryWithLastSfx(List<StoryLine> lines, int day, AudioSource sfxSource, AudioClip sfxClip)
    {
        if (panelObject != null)
            panelObject.SetActive(true);

        List<Sprite> images = imageDict.ContainsKey(day) ? imageDict[day] : null;

        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            speakerText.text = line.speaker;
            dialogueText.text = line.dialogue;

            if (mainImage != null)
            {
                if (images != null && line.imageIndex > 0 && line.imageIndex <= images.Count)
                {
                    mainImage.sprite = images[line.imageIndex - 1];
                    mainImage.gameObject.SetActive(true);
                }
                else
                {
                    mainImage.gameObject.SetActive(false);
                }
            }

            // ������ ��翡 ȿ���� ���
            if (i == lines.Count - 1 && sfxSource != null && sfxClip != null)
            {
                sfxSource.PlayOneShot(sfxClip);
            }

            waitingForClick = true;
            yield return WaitUntilUnscaled(() => !waitingForClick); //  ����� �κ�
        }

        if (panelObject != null)
            panelObject.SetActive(false);
    }

    //  �߰��� �޼���: �ð� ���� ���¿����� �۵��ϴ� WaitUntil
    private IEnumerator WaitUntilUnscaled(System.Func<bool> condition)
    {
        while (!condition())
            yield return new WaitForSecondsRealtime(0.01f); // Time.timeScale�� �����ϰ� ���
    }
}
