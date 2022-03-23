using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameDataSelector : MonoBehaviour
{
    [SerializeField] private GameData currentGameData = null;
    [SerializeField] private GameLevelData levelData;

    private void Awake()
    {
        SelectSequentialBoardData();
    }

    private void SelectSequentialBoardData()
    {
        foreach (var record in levelData.data)
        {
            if (record.categoryName == currentGameData.selectedCategoryName)
            {
                var boardIndex = DataSaver.ReadCategoryCurrentIndexValues(currentGameData.selectedCategoryName);
                if (boardIndex < record.BoardDatas.Count)
                {
                    currentGameData.selectedBoardData = record.BoardDatas[boardIndex];
                } else
                {
                    var randomIndex = Random.Range(0, record.BoardDatas.Count);
                    currentGameData.selectedBoardData = record.BoardDatas[randomIndex];
                }
            }
        }
    }
}
