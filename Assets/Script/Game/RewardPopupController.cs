using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardPopupController : MonoBehaviour
{
    [Header("치즈 보상 그룹")]
    [SerializeField] private RectTransform cheeseGroup;
    [SerializeField] private Graphic cheeseGraphic;  // Image or RawImage
    [SerializeField] private TMP_Text cheeseText;

    [Header("별 보상 그룹")]
    [SerializeField] private RectTransform starGroup;
    [SerializeField] private Graphic starGraphic;    // Image or RawImage
    [SerializeField] private TMP_Text starText;

    private void Awake()
    {
        SetActiveGroup(cheeseGroup, false);
        SetActiveGroup(starGroup, false);
    }

    public void ShowReward(int cheese, int star)
    {
        if (cheese > 0)
            StartCoroutine(PlayPopup(cheeseGroup, cheeseGraphic, cheeseText, $"{cheese}"));

        if (star > 0)
            StartCoroutine(PlayPopup(starGroup, starGraphic, starText, $"{star}"));
    }


    private IEnumerator PlayPopup(RectTransform group, Graphic graphic, TMP_Text text, string message)
    {
        if (group == null || graphic == null || text == null) yield break;

        group.gameObject.SetActive(true);
        graphic.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        text.text = message;

        float duration = 1.6f;
        float elapsed = 0f;
        float fadeInTime = 0.3f;
        float fadeOutStart = 0.6f;

        Vector2 startPos = group.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0f, 50f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float alpha = 1f;
            if (t < fadeInTime)
                alpha = Mathf.Lerp(0f, 1f, t / fadeInTime);
            else if (t > fadeOutStart)
                alpha = Mathf.Lerp(1f, 0f, (t - fadeOutStart) / (duration - fadeOutStart));

            group.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            SetAlpha(graphic, alpha);
            SetAlpha(text, alpha);

            yield return null;
        }

        SetActiveGroup(group, false);
    }

    private void SetAlpha(Graphic g, float a)
    {
        if (g != null)
            g.color = new Color(g.color.r, g.color.g, g.color.b, a);
    }

    private void SetActiveGroup(RectTransform group, bool active)
    {
        if (group != null)
            group.gameObject.SetActive(active);
    }
}
