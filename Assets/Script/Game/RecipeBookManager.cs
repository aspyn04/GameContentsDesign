using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookManager : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private GameObject recipePanel;
    [SerializeField] private RawImage displayRawImage;        // 보여줄 RawImage
    [SerializeField] private Button nextImageButton;          // 순환 버튼
    [SerializeField] private List<Texture> textureList;       // 에셋에 있는 이미지들 (Texture2D)

    private bool isSettingOpen = false;
    private int currentImageIndex = 0;

    void Start()
    {
        if (recipePanel != null)
            recipePanel.SetActive(false);

        if (nextImageButton != null)
            nextImageButton.onClick.AddListener(OnClickNextImage);

        if (displayRawImage != null && textureList.Count > 0)
            displayRawImage.texture = textureList[0];
    }

    void Update()
    {
        if (!isSettingOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            CloseSetting();
        }
    }

    public void OnClickSetting()
    {
        if (recipePanel != null)
        {
            recipePanel.SetActive(true);
            isSettingOpen = true;
            Time.timeScale = 0f;
            BGMManager.Instance?.PauseMusic();

            currentImageIndex = 0;
            if (displayRawImage != null && textureList.Count > 0)
                displayRawImage.texture = textureList[0];
        }
    }

    private void CloseSetting()
    {
        if (recipePanel != null)
        {
            recipePanel.SetActive(false);
            isSettingOpen = false;
            Time.timeScale = 1f;
            BGMManager.Instance?.ResumeMusic();
        }
    }

    private void OnClickNextImage()
    {
        if (textureList == null || textureList.Count == 0 || displayRawImage == null) return;

        currentImageIndex = (currentImageIndex + 1) % textureList.Count;
        displayRawImage.texture = textureList[currentImageIndex];
    }
}
