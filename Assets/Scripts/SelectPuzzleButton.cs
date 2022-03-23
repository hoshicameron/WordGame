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

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        gameData.selectedCategoryName = gameObject.name;
        SceneManager.LoadScene(gameSceneName);
    }
}
