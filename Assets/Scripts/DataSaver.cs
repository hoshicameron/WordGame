using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DataSaver : MonoBehaviour
{
    public static int ReadCategoryCurrentIndexValues(string name)
    {
        var value = -1;
        if (PlayerPrefs.HasKey(name))
            value = PlayerPrefs.GetInt(name);

        return value;
    }

    public static void SaveCategoryData(string categoryName, int currentIndex)
    {
        PlayerPrefs.SetInt(categoryName,currentIndex);
        PlayerPrefs.Save();
    }

    public static void ClearGameData(GameLevelData levelData)
    {
        print("Clear");
        foreach (var record in levelData.data)
        {
            PlayerPrefs.SetInt(record.categoryName,-1);
        }

        // Unlock The first level
        PlayerPrefs.SetInt(levelData.data[0].categoryName,0);
        PlayerPrefs.Save();
    }
}
