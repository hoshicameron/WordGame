using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_GameLevelData" ,menuName ="ScriptableObjects/GameLevelData")]
public class GameLevelData : ScriptableObject
{
    [System.Serializable]
    public struct CategoryRecord
    {
        public string categoryName;
        public List<BoardData> BoardDatas;
    }

    public List<CategoryRecord> data;
}
