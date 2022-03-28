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

    /// <summary>
    /// Select board Data to play
    /// </summary>
    private void SelectSequentialBoardData()
    {
        foreach (var record in levelData.data)
        {
            if (record.categoryName == currentGameData.selectedCategoryName)
            {
                var boardIndex = DataSaver.ReadCategoryCurrentIndexValues(currentGameData.selectedCategoryName);
                // if there is a level that player not complete it then ...
                if (boardIndex < record.BoardDatas.Count)
                {
                    currentGameData.selectedBoardData = record.BoardDatas[boardIndex];
                }
                // load one of the board datas in current category
                else
                {
                    var randomIndex = Random.Range(0, record.BoardDatas.Count);
                    currentGameData.selectedBoardData = record.BoardDatas[randomIndex];
                }
            }
        }
    }
}
