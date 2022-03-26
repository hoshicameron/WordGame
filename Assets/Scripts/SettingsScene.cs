using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScene : MonoBehaviour
{
    [SerializeField] private GameLevelData levelData;

    public void ClearGameData()
    {
        DataSaver.ClearGameData(levelData);
    }
}
