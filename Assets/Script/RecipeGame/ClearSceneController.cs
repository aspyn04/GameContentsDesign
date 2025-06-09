using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClearSceneController : MonoBehaviour
{
    [Header("��� ǥ�� UI")]
    public Image tartImage;
    public TextMeshProUGUI tartNameText;

    public Image ingredientImage1;
    public TextMeshProUGUI ingredientText1;
    public Image ingredientImage2;
    public TextMeshProUGUI ingredientText2;
    public Image ingredientImage3;
    public TextMeshProUGUI ingredientText3;

    [Header("Ÿ��Ʈ ��������Ʈ")]
    public Sprite tart01, tart02, tart03, tart04, tart05, tart06, tart07, tart08;

    [Header("��� ��������Ʈ")]
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
            Debug.LogError("RecipeMinigameManager �ν��Ͻ��� ã�� �� �����ϴ�.");
            HideAll();
            return;
        }

        // PanelEntries ������Ƽ ���
        var entry = mgr.panelEntries
                       .FirstOrDefault(e => e.panelObject != null && e.panelObject.activeSelf);

        if (entry == null)
        {
            Debug.LogWarning("Ȱ��ȭ�� �̴ϰ��� �г��� �����ϴ�.");
            HideAll();
            return;
        }

        switch (entry.day)
        {
            case 1:
                tartImage.sprite = tart01;
                tartNameText.text = "ȣ�� ��¦ Ÿ��Ʈ";
                SetIngredients(
                    blueMoonJam, "��繮 ��",
                    hoyoJelly, "ȣ�� ����",
                    galaxyPearl, "���� ��");
                break;
            case 2:
                tartImage.sprite = tart02;
                tartNameText.text = "�ܿ��� �ĵ� Ÿ��Ʈ";
                SetIngredients(
                    blueMoonJam, "��繮 ��",
                    frozenWaveShard, "�� �ĵ� ����",
                    brokenShell, "�μ��� ����");
                break;
            case 3:
                tartImage.sprite = tart03;
                tartNameText.text = "���� Ÿ��Ʈ";
                SetIngredients(
                    citrineCream, "��Ʈ�� ũ��",
                    ruby, "���",
                    chocoChip, "����Ĩ");
                break;
            case 4:
                tartImage.sprite = tart04;
                tartNameText.text = "�Ƹ޵�Ʈ Ÿ��Ʈ";
                SetIngredients(
                    citrineCream, "��Ʈ�� ũ��",
                    amethystShard, "�ڼ��� ����",
                    purpleHoney, "����� ��");
                break;
            case 5:
                tartImage.sprite = tart05;
                tartNameText.text = "���� ������ Ÿ��Ʈ";
                SetIngredients(
                    citrineCream, "��Ʈ�� ũ��",
                    glowMushroom, "�߱� ����",
                    marimo, "������");
                break;
            case 6:
                tartImage.sprite = tart06;
                tartNameText.text = "���� Ÿ��Ʈ";
                SetIngredients(
                    lavaJam, "�����",
                    cookieNcreamMushroom, "���ũ �ӽ���",
                    chocoChip, "����Ĩ");
                break;
            case 7:
                tartImage.sprite = tart07;
                tartNameText.text = "���Թ��� Ÿ��Ʈ";
                SetIngredients(
                    lavaJam, "�����",
                    cloudMarshmallow, "���� ���ø��",
                    moonFried, "�� Ƣ��");
                break;
            case 8:
                tartImage.sprite = tart08;
                tartNameText.text = "��Ÿ�� ���� Ÿ��Ʈ";
                SetIngredients(
                    lavaJam, "�����",
                    roastedLeaf, "���� ��",
                    nutStack, "�߰��� ����");
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