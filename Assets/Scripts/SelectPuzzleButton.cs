using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPuzzleButton : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private GameLevelData levelData;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private Image progressBarFilling;


    private string gameSceneName="Scene_Game";
    private Button button;
    private bool levelLocked = false;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        UpdateButtonInformation();
        button.interactable = !levelLocked;
    }

    private void UpdateButtonInformation()
    {
        int currentIndex = -1;
        int totalBoards = 0;

        foreach (var record in levelData.data)
        {
            if (record.categoryName == gameObject.name)
            {
                currentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                totalBoards = record.BoardDatas.Count;

                if (levelData.data[0].categoryName == gameObject.name && currentIndex < 0)
                {
                    DataSaver.SaveCategoryData(levelData.data[0].categoryName,0);
                    currentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                }
            }
        }

        if (currentIndex == -1) levelLocked = true;

        categoryText.SetText(levelLocked ? string.Empty : $"{currentIndex}/{totalBoards}");
        progressBarFilling.fillAmount =
            (currentIndex > 0 && totalBoards > 0) ? ((float) currentIndex / (float) totalBoards) : 0f;
    }

    private void OnButtonClick()
    {
        gameData.selectedCategoryName = gameObject.name;
        SceneManager.LoadScene(gameSceneName);
    }
}