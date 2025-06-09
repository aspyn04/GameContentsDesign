using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClearSceneController : MonoBehaviour
{
    [Header("결과 표시 UI")]
    public Image tartImage;
    public TextMeshProUGUI tartNameText;

    public Image ingredientImage1;
    public TextMeshProUGUI ingredientText1;
    public Image ingredientImage2;
    public TextMeshProUGUI ingredientText2;
    public Image ingredientImage3;
    public TextMeshProUGUI ingredientText3;

    [Header("타르트 스프라이트")]
    public Sprite tart01, tart02, tart03, tart04, tart05, tart06, tart07, tart08;

    [Header("재료 스프라이트")]
    public Sprite blueMoonJam;
    public Sprite hoyoJelly;
    public Sprite galaxyPearl;
    public Sprite frozenWaveShard;
    public Sprite brokenShell;
    public Sprite citrineCream;
    public Sprite ruby;
    public Sprite chocoChip;
    public Sprite amethystShard;
    public Sprite purpleHoney;
    public Sprite glowMushroom;
    public Sprite marimo;
    public Sprite lavaJam;
    public Sprite cookieNcreamMushroom;
    public Sprite cloudMarshmallow;
    public Sprite moonFried;
    public Sprite roastedLeaf;
    public Sprite nutStack;

    void Start()
    {
        var mgr = RecipeMinigameManager.Instance;
        if (mgr == null)
        {
            Debug.LogError("RecipeMinigameManager 인스턴스를 찾을 수 없습니다.");
            HideAll();
            return;
        }

        // PanelEntries 프로퍼티 사용
        var entry = mgr.panelEntries
                       .FirstOrDefault(e => e.panelObject != null && e.panelObject.activeSelf);

        if (entry == null)
        {
            Debug.LogWarning("활성화된 미니게임 패널이 없습니다.");
            HideAll();
            return;
        }

        switch (entry.day)
        {
            case 1:
                tartImage.sprite = tart01;
                tartNameText.text = "호요 반짝 타르트";
                SetIngredients(
                    blueMoonJam, "블루문 잼",
                    hoyoJelly, "호요 젤리",
                    galaxyPearl, "은하 펄");
                break;
            case 2:
                tartImage.sprite = tart02;
                tartNameText.text = "겨울의 파도 타르트";
                SetIngredients(
                    blueMoonJam, "블루문 잼",
                    frozenWaveShard, "얼린 파도 조각",
                    brokenShell, "부서진 조개");
                break;
            case 3:
                tartImage.sprite = tart03;
                tartNameText.text = "레빗 타르트";
                SetIngredients(
                    citrineCream, "시트린 크림",
                    ruby, "루비",
                    chocoChip, "초코칩");
                break;
            case 4:
                tartImage.sprite = tart04;
                tartNameText.text = "아메디스트 타르트";
                SetIngredients(
                    citrineCream, "시트린 크림",
                    amethystShard, "자수정 조각",
                    purpleHoney, "보라색 꿀");
                break;
            case 5:
                tartImage.sprite = tart05;
                tartNameText.text = "버섯 마리모 타르트";
                SetIngredients(
                    citrineCream, "시트린 크림",
                    glowMushroom, "야광 버섯",
                    marimo, "마리모");
                break;
            case 6:
                tartImage.sprite = tart06;
                tartNameText.text = "데빌 타르트";
                SetIngredients(
                    lavaJam, "용암잼",
                    cookieNcreamMushroom, "쿠앤크 머쉬룸",
                    chocoChip, "초코칩");
                break;
            case 7:
                tartImage.sprite = tart07;
                tartNameText.text = "뭉게뭉게 타르트";
                SetIngredients(
                    lavaJam, "용암잼",
                    cloudMarshmallow, "구름 마시멜로",
                    moonFried, "달 튀김");
                break;
            case 8:
                tartImage.sprite = tart08;
                tartNameText.text = "불타는 넛츠 타르트";
                SetIngredients(
                    lavaJam, "용암잼",
                    roastedLeaf, "구운 잎",
                    nutStack, "견과류 더미");
                break;
            default:
                HideAll();
                break;
        }
    }

    private void SetIngredients(
        Sprite img1, string txt1,
        Sprite img2, string txt2,
        Sprite img3, string txt3)
    {
        ingredientImage1.sprite = img1;
        ingredientText1.text = txt1;
        ingredientImage1.enabled = img1 != null;
        ingredientText1.enabled = !string.IsNullOrEmpty(txt1);

        ingredientImage2.sprite = img2;
        ingredientText2.text = txt2;
        ingredientImage2.enabled = img2 != null;
        ingredientText2.enabled = !string.IsNullOrEmpty(txt2);

        ingredientImage3.sprite = img3;
        ingredientText3.text = txt3;
        ingredientImage3.enabled = img3 != null;
        ingredientText3.enabled = !string.IsNullOrEmpty(txt3);
    }

    private void HideAll()
    {
        tartImage.enabled = false;
        tartNameText.enabled = false;
        ingredientImage1.enabled = false;
        ingredientText1.enabled = false;
        ingredientImage2.enabled = false;
        ingredientText2.enabled = false;
        ingredientImage3.enabled = false;
        ingredientText3.enabled = false;
    }
}